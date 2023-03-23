using System.Text.Json;
using WebSocketSharp;
using Raylib_cs;

public class NetworkController
{
    public WebSocket ws;
    string ipAddress = "";

    public NetworkController()
    {
        ConnectClient();
    }

    public void StartSocket()
    {
        ws.Connect();
    }

    public void ConnectClient()
    {
        string socketStart = "ws://";
        string enteredAddress = "";
        bool failed = false;

        // ws = new("ws://192.168.10.177:3000/snake");
        // return;
        ws = new("ws://10.151.173.27:3000/snake");
        return;

    // Could probably be done without a goto, but that would require insane code indentation and more complexity. 
    // I rather just use a goto to make this work smoothly
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

    public void SendPlayerData(SnakeProperties playerProps)
    {
        string packet = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = "position",
            Content = JsonSerializer.Serialize<SnakeProperties>(playerProps)
        });

        ws.Send(packet);
    }

    public void SendFoodData(List<int> indexes)
    {
        string packet = JsonSerializer.Serialize<SendInfo>(new()
        {
            MessageType = "ate",
            Content = JsonSerializer.Serialize<List<int>>(indexes)
        });

        ws.Send(packet);
    }
}
