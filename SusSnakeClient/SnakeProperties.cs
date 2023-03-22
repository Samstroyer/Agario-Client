using System.Text.Json.Serialization;

public class SnakeProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public float X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public float Y { get; set; }

    [JsonPropertyName("body"), JsonInclude]
    public List<(int X, int Y)> Body { get; set; } = new();
}
