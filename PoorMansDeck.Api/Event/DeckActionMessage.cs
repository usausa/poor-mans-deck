namespace PoorMansDeck.Event;

public sealed class DeckActionMessage
{
    public string Command { get; set; } = default!;

    public int Page { get; set; }

    public int X { get; set; }

    public int Y { get; set; }
}
