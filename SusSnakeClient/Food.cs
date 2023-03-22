using System.Text.Json.Serialization;
using System.Numerics;
using Raylib_cs;

public class Food
{
    [JsonIgnore]
    private static Random r = new();
    [JsonIgnore]
    public static int SpawnRadius = 1000;
    [JsonIgnore]
    private Color c = Color.RED;

    [JsonPropertyName("x"), JsonInclude]
    public int X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public int Y { get; set; }

    public Food()
    {
        X = r.Next(-SpawnRadius, SpawnRadius);
        Y = r.Next(-SpawnRadius, SpawnRadius);
    }

    public void Draw(float xOffset, float yOffset)
    {
        Raylib.DrawCircle(X - (int)xOffset, Y - (int)yOffset, 10, c);
    }
}
