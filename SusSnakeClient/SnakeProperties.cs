using System.Text.Json.Serialization;
using Raylib_cs;

public class SnakeProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public float X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public float Y { get; set; }

    [JsonPropertyName("body"), JsonInclude]
    public List<BodyClass> Body { get; set; } = new();

    public void Draw(Color c)
    {
        Raylib.DrawCircle((int)X, (int)Y, 10, c);
    }
}
