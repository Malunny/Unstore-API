using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class HomeController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok("Welcome to Unstore API!");
    }
}