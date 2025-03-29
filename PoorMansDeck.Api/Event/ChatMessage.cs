namespace PoorMansDeck.Event;

public class ChatMessage
{
    public string Text { get; set; } = default!;

    public DateTime Timestamp { get; set; }
}
