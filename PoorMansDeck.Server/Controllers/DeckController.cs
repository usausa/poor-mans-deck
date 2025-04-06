namespace PoorMansDeck.Server.Controllers;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using PoorMansDeck.Api;
using PoorMansDeck.Event;
using PoorMansDeck.Server.Hubs;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeckController : ControllerBase
{
    private readonly IHubContext<DeckHub> hubContext;

    public DeckController(IHubContext<DeckHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] ChatSendRequest request)
    {
        await hubContext.Clients.All.SendAsync("ReceiveMessage", new ChatMessage { Text = request.Text, Timestamp = DateTime.Now }).ConfigureAwait(false);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public IActionResult Page()
    {
        return Ok("TODO implement");
    }

    [HttpGet]
    [Authorize]
    public IActionResult Image()
    {
        return Ok("TODO implement");
    }
}
