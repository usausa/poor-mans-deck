namespace PoorMansDeck.Server.Hubs;

// TODO
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using PoorMansDeck.Event;

//[Authorize]
public class DeckHub : Hub
{
    public Task AddMessage(string text)
    {
        return Clients.All.SendAsync("ReceiveMessage", new ChatMessage { Text = text, Timestamp = DateTime.Now });
    }

#pragma warning disable CA1822
    public Task UpdateStatus()
    {
        // TODO
        return Task.CompletedTask;
    }

    public Task Trigger(string command, int page, int x, int y)
    {
        // TODO
        return Task.CompletedTask;
    }
}
