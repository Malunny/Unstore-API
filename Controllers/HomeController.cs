using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.Services;

namespace Unstore.Controllers;

[ApiController]
[Authorize(Roles = "ADM, Gerente")]
public partial class HomeController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok("Welcome to Unstore API!");
    }
}