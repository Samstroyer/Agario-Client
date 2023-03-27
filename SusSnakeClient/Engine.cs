using System.Diagnostics;
using System.Text.Json;
using System.Numerics;
using WebSocketSharp;
using System.Timers;
using Raylib_cs;

public class Engine
{
    Stopwatch stopWatch = new Stopwatch();

    // Class declarations
    NetworkController networkController;
    Player p;
    Vector2 res;

    // Instead of sending the position every frame it is sent 4 times a second, pretty fast still...
    System.Timers.Timer positionSender = new(250)
    {
        AutoReset = true,
        Enabled = true
    };

    // Transparent color for the leaderboard 😎
    Color c = new(0, 200, 0, 120);

    // List of all the other players
    public static bool otherListLock = false;
    Dictionary<string, PlayerProperties> othersData = new();
    // Dictionary<string, PlayerProperties> othersRender = new();

    // Variables for the food points
    public static bool foodListLock = false;
    List<Food> foodPoints = new();

    // This makes it possible to distinguish names, not 100% secure as you can change pc name and have the same!
    string ownID;

    public Engine()
    {
        // The id is from System.Environment.MachineName
        ownID = new SendInfo().ID;

        // Network controller keeps track of the websocket (mostly)
        networkController = new();
        networkController.ws.OnMessage += MessageHandler;
        networkController.StartSocket();

        // Create player and set resolution variable
        p = new();
        res = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        // Set the timer to send the position every 0.25 seconds
        positionSender.Elapsed += TimerSend;
        positionSender.Start();
    }

    public void Start()
    {
        // Wait until it has gotten all the food points and if it doesn't get food, you do need to restart the client...
        // Will not make it shutdown as I think for a slow internet and machine it may take some time
        while (foodPoints.Count <= 0) ;

        // Start the game - Main loop
        while (!Raylib.WindowShouldClose())
        {
            MouseHandler();
            Logic();

            StartScene();

            Render();

            EndScene();
        }

        networkController.Close();
    }

    public void TimerSend(Object source, ElapsedEventArgs e)
    {
        // Send position to server
        networkController.SendPlayerData(p.playerProps);
    }

    // This function should probably be named bloatware!
    private void Logic()
    {
        // Check all the food points and set them to a deactivated state (so you can't eat the same food more than once)
        while (foodListLock) ;
        foodListLock = true;
        var indexes = p.Intersect(ref foodPoints);
        foodListLock = false;
        // If some food indexes got eaten, send the indexes to the server for respawning 
        if (indexes.Count > 0) networkController.SendFoodData(indexes);


    }

    private void Render()
    {
        // Render playable area
        PlayArea();

        // Render the player
        p.Draw();

        // Render food
        RenderFood();

        // Render other players
        RenderOthers();

        // Render score board
        RenderScores();
    }

    private void RenderScores()
    {
        // Render a scoreboard with players (by score)
        int fontSize = 12;

        Raylib.DrawRectangle(600, 10, 190, 20 + (othersData.Count * 20), c);

        int yPos = 15;
        foreach (var kvp in othersData)
        {
            Raylib.DrawText($"{kvp.Key} : {kvp.Value.Size}", 610, yPos, fontSize, Color.BLACK);
            yPos += 15;
        }
    }

    private void RenderOthers()
    {
        // Render other players
        while (otherListLock) ;
        otherListLock = true;
        foreach (KeyValuePair<string, PlayerProperties> kvp in othersData)
        {
            /*
                Do not render yourself as it is done seperately (so you don't depend on server updates to render at your position...)
                server rendering" would be better if it was more competetive, but that is more network comptetetive instead of game competetive
                (The one with better connection would get the upper hand...)
            */
            if (kvp.Key == ownID) continue;
            kvp.Value.Draw(Color.GREEN, p.playerProps.X, p.playerProps.Y, kvp.Value.Size);
        }
        otherListLock = false;
    }

    private void RenderFood()
    {
        // Render food
        while (foodListLock) ;
        foodListLock = true;
        for (int i = 0; i < foodPoints.Count; i++)
        {
            foodPoints[i].Draw(p.playerProps.X - res.X / 2, p.playerProps.Y - res.Y / 2);
        }
        foodListLock = false;
    }

    private void PlayArea()
    {
        // Render playable area
        int spaceX = -Food.SpawnRadius - (int)p.playerProps.X + (int)res.X / 2;
        int spaceY = -Food.SpawnRadius - (int)p.playerProps.Y + (int)res.Y / 2;
        int size = Food.SpawnRadius * 2;
        Raylib.DrawRectangle(spaceX, spaceY, size, size, Color.WHITE);
    }

    // Ends the Raylib draw
    private void EndScene()
    {
        Raylib.EndDrawing();
    }

    // Starts the Raylib draw and sets background
    private void StartScene()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLACK);
    }

    private void MouseHandler()
    {
        // For movement
        var mousePos = Raylib.GetMousePosition();
        Movement(mousePos);
    }

    private void Movement(Vector2 mousePos)
    {
        // Gets mouse difference from center of the Raylib window - the further you move your mouse the faster you go
        Vector2 referencePos = Vector2.Subtract(mousePos, new(res.X / 2, res.Y / 2)) / 100;
        p.Move(referencePos.X, referencePos.Y);
    }

    // This handles all the messages that the websocket gets
    // It is in the Engine class as everything is easily accessible from it without making static variables
    private void MessageHandler(object sender, MessageEventArgs e)
    {

        SendInfo info = JsonSerializer.Deserialize<SendInfo>(e.Data);

        switch (info.MessageType)
        {
            case MessageType.OtherPlayers:
                // Get other players position and size
                while (otherListLock) ;
                otherListLock = true;
                othersData = JsonSerializer.Deserialize<Dictionary<string, PlayerProperties>>(info.Content);
                otherListLock = false;
                break;

            case MessageType.GetFood:
                // When the socket connects it recieves the food locations
                while (foodListLock) ;
                foodListLock = true;
                foodPoints = JsonSerializer.Deserialize<List<Food>>(info.Content);
                foodListLock = false;
                break;

            case MessageType.Food:
                // Instead of sending the whole list it just sends the individual food updates
                // Saves a lot of network and pc power when large lists are used (Around 7500 food points max - then the socket crashes from message length)
                while (foodListLock) ;
                foodListLock = true;
                List<FoodUpdate> newFood = JsonSerializer.Deserialize<List<FoodUpdate>>(info.Content);
                foreach (FoodUpdate item in newFood)
                {
                    foodPoints[item.Index] = item.Food;
                }
                foodListLock = false;
                break;

            default:
                // This is if the message dies...
                Console.WriteLine("Received unknown message type: " + info.MessageType);
                Console.WriteLine("Content: " + info.Content);
                break;
        }
    }
}
