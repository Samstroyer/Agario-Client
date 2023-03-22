using System.Text.Json;
using System.Numerics;
using WebSocketSharp;
using Raylib_cs;

public class Engine
{
    NetworkController networkController;
    Player p;
    Vector2 res;

    public static bool listLock = false;
    List<Food> foodPoints = new();

    public Engine()
    {
        networkController = new();
        networkController.ws.OnMessage += MessageHandler;
        networkController.StartSocket();

        p = new();
        res = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }

    public void Start()
    {
        while (foodPoints.Count <= 0) ;
        while (!Raylib.WindowShouldClose())
        {
            MouseHandler();
            Logic();

            StartScene();

            Render();

            EndScene();
        }
    }

    private void Logic()
    {
        // This function should probably be named bloatware!

        // Send position to server
        networkController.SendPlayerData(p.playerProps);

        while (listLock) ;
        listLock = true;
        var indexes = p.Intersect(ref foodPoints);
        listLock = false;

        if (indexes.Count > 0) networkController.SendFoodData(indexes);
    }

    private void Render()
    {
        int spaceX = -Food.SpawnRadius - (int)p.playerProps.X + (int)res.X / 2 - 20;
        int spaceY = -Food.SpawnRadius - (int)p.playerProps.Y + (int)res.Y / 2 - 20;
        int size = 40 + Food.SpawnRadius * 2;
        Raylib.DrawRectangle(spaceX, spaceY, size, size, Color.WHITE);

        p.Draw();

        while (listLock) ;
        listLock = true;
        for (int i = 0; i < foodPoints.Count; i++)
        {
            foodPoints[i].Draw(p.playerProps.X - res.X / 2, p.playerProps.Y - res.Y / 2);
        }
        listLock = false;
    }

    private void EndScene()
    {
        Raylib.EndDrawing();
    }

    private void StartScene()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLACK);
    }

    private void MouseHandler()
    {
        var mousePos = Raylib.GetMousePosition();

        Movement(mousePos);
    }

    private void Movement(Vector2 mousePos)
    {
        Vector2 referencePos = Vector2.Subtract(mousePos, new(res.X / 2, res.Y / 2)) / 100;

        p.Move(referencePos.X, referencePos.Y);
    }

    private void MessageHandler(object sender, MessageEventArgs e)
    {
        SendInfo info = JsonSerializer.Deserialize<SendInfo>(e.Data);

        switch (info.MessageType)
        {
            case "PlayerPos":
                //Damn this will be a chunk here lul
                break;

            case "InitFood":
                while (listLock) ;
                listLock = true;
                foodPoints = JsonSerializer.Deserialize<List<Food>>(info.Content);
                listLock = false;
                break;

            case "FoodUpdate":
                while (listLock) ;
                listLock = true;

                List<FoodUpdate> newFood = JsonSerializer.Deserialize<List<FoodUpdate>>(info.Content);
                foreach (FoodUpdate item in newFood)
                {
                    foodPoints[item.Index] = item.Food;
                }

                listLock = false;
                break;

            default:
                Console.WriteLine("Received unknown message type: " + info.MessageType);
                Console.WriteLine("Content: " + info.Content);
                break;
        }
    }
}
