using System.Text.Json.Serialization;
using Raylib_cs;

public class SnakeProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public int X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public int Y { get; set; }

    [JsonPropertyName("size"), JsonInclude]
    public int Size { get; set; } = 10;

    public void Draw(Color c, float playerX, float playerY)
    {
        Raylib.DrawCircle((int)(X) + Raylib.GetScreenWidth() / 2, (int)(Y) + Raylib.GetScreenHeight() / 2, 10, c);
    }
}
