using System.Text.Json.Serialization;
using System.Net;

public class SendInfo
{
    [JsonPropertyName("type"), JsonInclude]
    public string MessageType { get; set; }

    [JsonPropertyName("content"), JsonInclude]
    public string Content { get; set; }

    [JsonPropertyName("id"), JsonInclude]
    public string ID = System.Environment.MachineName;
}
