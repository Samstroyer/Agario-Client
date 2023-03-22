using System.Text.Json.Serialization;

public class FoodUpdate
{
    [JsonPropertyName("food"), JsonInclude]
    public Food Food { get; set; }

    [JsonPropertyName("index"), JsonInclude]
    public int Index { get; set; }
}
