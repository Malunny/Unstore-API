using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Extensions;
using Unstore.Data;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpGet("/v1/clients/get-all")]
    public async Task<IActionResult> GetAllAsync
        ([FromServices] AppDbContext context)
    {
        return Ok();
    }
}