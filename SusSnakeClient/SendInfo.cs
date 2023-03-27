using System.Text.Json.Serialization;

/*
    This is a custom way of sending messages between server and client
*/

// Different types of messages 
// It is unecessary but good practice to set number manually, if you have other coders accidentally switching the order :)
public enum MessageType
{
    Position = 1,
    OtherPlayers = 2,
    Food = 3,
    GetFood = 4,
    Close = 1001
}

// The send info sends what type of message, the content and the ID of sender
// ID is really only important for the server so that it can tell users apart
public class SendInfo
{
    [JsonPropertyName("type"), JsonInclude]
    public MessageType MessageType { get; set; }

    [JsonPropertyName("content"), JsonInclude]
    public string Content { get; set; }

    [JsonPropertyName("id"), JsonInclude]
    public string ID = "System.Environment.MachineName";

    // Publishing with another name makes same pc be able to make multiple clients
    // [JsonPropertyName("id"), JsonInclude]
    // public string ID = "DEBUGNAME";
}
