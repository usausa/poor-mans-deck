namespace PoorMansDeck.Server.Hubs;

using Microsoft.AspNetCore.SignalR;

using PoorMansDeck.Event;

public class DeckHub : Hub
{
    public Task AddMessage(string text)
    {
        return Clients.All.SendAsync("ReceiveMessage", new ChatMessage { Text = text, Timestamp = DateTime.Now });
    }
}
