using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpGet("/v1/clients/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetSkipTake
        ([FromServices] ClientService service,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var clients = await service.GetRangeAsync(skip, take);
        return Ok(clients.Data);
    }
    
    [HttpGet("/v1/clients/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] ClientService service)
    {
        var client = await service.GetByIdAsync(id);
        return Ok(client.Data);
    }
}