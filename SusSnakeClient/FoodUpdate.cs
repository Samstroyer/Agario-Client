using System.Text.Json.Serialization;

/*
    This class is made to easily update food positions between server and client
*/

public class FoodUpdate
{
    [JsonPropertyName("food"), JsonInclude]
    public Food Food { get; set; }

    [JsonPropertyName("index"), JsonInclude]
    public int Index { get; set; }
}
