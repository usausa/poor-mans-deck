namespace PoorMansDeck.Server.Controllers;

using Microsoft.AspNetCore.Mvc;

using PoorMansDeck.Server.Handlers.Media;

public class TestExecuteRequest
{
    public string Command { get; set; } = default!;
}

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{
    [HttpPost]
    public IActionResult Execute([FromBody] TestExecuteRequest request)
    {
        var handler = new MediaHandler();
        // ReSharper disable StringLiteralTypo
        switch (request.Command)
        {
            case "VolumeUp":
                handler.VolumeUp();
                break;
            case "VolumeDown":
                handler.VolumeDown();
                break;
            case "Mute":
                handler.Mute();
                break;
            case "Unmute":
                handler.Unmute();
                break;
            default:
                return BadRequest();
        }

        return Ok();
    }
}
