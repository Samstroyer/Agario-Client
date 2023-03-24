using System.Text.Json;
using WebSocketSharp;
using Raylib_cs;

/*
    This class is a small container for the websocket, making the Engine class not as bloated
*/

public class NetworkController
{
    public WebSocket ws;
    string ipAddress = "";

    // When creating a NetworkController it connects client
    public NetworkController()
    {
        ConnectClient();
    }

    // When you connect the client the food data is sent back directly
    // This funtion is here to make sure everything else is ready for recieving the food data
    public void StartSocket()
    {
        ws.Connect();
    }

    // This is a simple interface for connecting to a server with an IP
    public void ConnectClient()
    {
        string socketStart = "ws://";
        string enteredAddress = "";
        bool failed = false;

        // FOR 
        ws = new("ws://192.168.10.240:3000/snake");
        return;
    // ws = new("ws://10.151.173.27:3000/snake");
    // return;

    // Could probably be done without a goto, but that would require insane code indentation and more complexity
    // I rather just use a goto to make this simple and work smoothly 
    RedoAddress:
        while (true)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            Raylib.DrawText("Current server endpoint:\n" + socketStart + enteredAddress + "\n(do not enter port)", 100, 100, 32, Color.RED);

            if (failed)
            {
                Raylib.DrawText("Error on IP: " + ipAddress, 100, 500, 32, Color.RED);
            }

            Raylib.EndDrawing();

            KeyboardKey key = (KeyboardKey)Raylib.GetKeyPressed();

            if (key == KeyboardKey.KEY_ESCAPE) Raylib.CloseWindow();

            if (key == KeyboardKey.KEY_ENTER && enteredAddress.Length > 0) break;
            if (key == KeyboardKey.KEY_NULL) continue;
            if (key == KeyboardKey.KEY_BACKSPACE)
            {
                if (enteredAddress.Length > 0) enteredAddress = enteredAddress.Remove(enteredAddress.Length - 1);
                continue;
            }

            enteredAddress += (char)(int)key;
        }
        try
        {
            // Try to connect to the IP
            ipAddress = socketStart + enteredAddress + ":3000/snake";
            ws = new(ipAddress);
            Console.WriteLine("Using: " + ipAddress + " for connecting the socket!");
        }
        catch (Exception e)
        {
            // If exception is thrown, log it internally and try again
            Console.WriteLine("Failed the call with exception: " + e.Message);
            failed = true;
            goto RedoAddress;
        }
    }

    // This function sends the player data
    public void SendPlayerData(PlayerProperties playerProps)
    {
        string packet = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = MessageType.Position,
            Content = JsonSerializer.Serialize<PlayerProperties>(playerProps)
        });

        ws.Send(packet);
    }

    //This function sends the eaten food data
    public void SendFoodData(List<int> indexes)
    {
        string packet = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = MessageType.Food,
            Content = JsonSerializer.Serialize<List<int>>(indexes)
        });

        ws.Send(packet);
    }
}
