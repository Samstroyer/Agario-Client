using System.Text.Json.Serialization;
using Raylib_cs;

/*
    Food class
    This class is hopefully used by the server to set the spawnradius of the server and playable area
    Possibly making it dynamic from amount of players present
*/

public class Food
{
    [JsonIgnore]
    public static int SpawnRadius = 1000;
    [JsonIgnore]
    private static Color c = Color.RED;
    [JsonIgnore]
    public static int Radius { get; set; } = 5;

    // Taken is so that you can't eat the same food multiple times (The server removes and adds a new food)
    [JsonIgnore]
    public bool taken = false;

    [JsonPropertyName("x"), JsonInclude]
    public int X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public int Y { get; set; }

    public void Draw(float xOffset, float yOffset)
    {
        Raylib.DrawCircle(X - (int)xOffset, Y - (int)yOffset, 5, c);
    }
}
