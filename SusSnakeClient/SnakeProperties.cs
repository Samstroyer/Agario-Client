using System.Text.Json.Serialization;
using Raylib_cs;

/*
    This function keeps track of position and size for players
*/

public class PlayerProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public float X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public float Y { get; set; }

    [JsonPropertyName("size"), JsonInclude]
    public int Size { get; set; } = 10;

    // The draw is able to draw other players mainly
    public void Draw(Color c, float playerX, float playerY, int size)
    {
        Raylib.DrawCircle((int)(X - playerX) + Raylib.GetScreenWidth() / 2, (int)(Y - playerY) + Raylib.GetScreenHeight() / 2, size, c);
    }
}
