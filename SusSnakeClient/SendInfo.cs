using System.Text.Json.Serialization;
using System.Net;

public class SendInfo
{
    [JsonInclude]
    public string MessageType { get; set; }
    [JsonInclude]
    public string Content { get; set; }
    [JsonInclude]
    private static string ID = System.Environment.MachineName;
}
