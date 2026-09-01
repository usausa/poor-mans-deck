#pragma warning disable CA1716
namespace PoorMansDeck.Event;

public class ChatMessage
{
    public string Text { get; set; } = default!;

    public DateTime Timestamp { get; set; }
}
#pragma warning restore CA1716
