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

    public void Draw(Color c, float playerX, float playerY, int size)
    {
        Raylib.DrawCircle((int)(X - playerX) + Raylib.GetScreenWidth() / 2, (int)(Y - playerY) + Raylib.GetScreenHeight() / 2, size, c);
    }
}
