using System.Text.Json.Serialization;
using System.Numerics;

public class SnakeProperties
{
    [JsonPropertyName("x"), JsonInclude]
    public float X { get; set; }

    [JsonPropertyName("y"), JsonInclude]
    public float Y { get; set; }

    [JsonPropertyName("pointsArr"), JsonInclude]
    public (int x, int y)[] PreviousPointsArray
    {
        get
        {
            return PreviousPointsArray;
        }
        set
        {
            PreviousPointsArray = new (int x, int y)[PreviousPointsList.Count];
            int index = 0;
            foreach (Vector2 v in PreviousPointsList)
            {
                PreviousPointsList[index] = new((int)v.X, (int)v.Y);
                index++;
            }
        }
    }

    [JsonIgnore]
    public List<Vector2> PreviousPointsList { get; set; } = new();
}
