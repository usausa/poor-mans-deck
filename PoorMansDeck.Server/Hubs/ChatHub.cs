namespace PoorMansDeck.Server.Hubs;

using Microsoft.AspNetCore.SignalR;

using PoorMansDeck.Event;

public class ChatHub : Hub
{
    public async Task AddMessage(string text)
    {
        await Clients.All.SendAsync("ReceiveMessage", new ChatMessage { Text = text, Timestamp = DateTime.Now }).ConfigureAwait(false);
    }
}
