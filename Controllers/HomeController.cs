using Microsoft.AspNetCore.Mvc;

namespace Unstore.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok("Welcome to Unstore API!");
    }
}