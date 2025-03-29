namespace PoorMansDeck.Server.Controllers;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using PoorMansDeck.Api;
using PoorMansDeck.Event;
using PoorMansDeck.Server.Hubs;

[ApiController]
[Route("api/[controller]/[action]")]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> messageHubContext;

    public ChatController(IHubContext<ChatHub> messageHubContext)
    {
        this.messageHubContext = messageHubContext;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] ChatSendRequest request)
    {
        await messageHubContext.Clients.All.SendAsync("ReceiveMessage", new ChatMessage { Text = request.Text, Timestamp = DateTime.Now }).ConfigureAwait(false);
        return Ok();
    }
}
