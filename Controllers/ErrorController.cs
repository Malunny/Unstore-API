using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unstore.Controllers;

[ApiController]
[Route("/error")]
public class ErrorController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Error()
    {
        throw new Exception("Internal Server Error");
    }
}