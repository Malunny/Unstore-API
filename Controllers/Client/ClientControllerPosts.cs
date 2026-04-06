using Microsoft.AspNetCore.Mvc;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Microsoft.AspNetCore.Authorization;
using Unstore.Services;
using System.Runtime.InteropServices;

namespace Unstore.Controllers;

public partial class ClientController
{
    
    [HttpPost("/v1/clients/post")]
    public async Task<IActionResult> PostAsync
    ([FromBody] ClientCreateDto client,
    [FromServices] AppDbContext context,
    [FromServices] ClientService service)
    {
        var result = await service.Create(ModelState, client);
        if (result.IsBadResult())
            return BadRequest(result);
        else
            return Created("/v1/clients/post", result);
    }
    
    [HttpPost("/v1/clients/post-range")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<ClientCreateDto> clients,
        [FromServices] AppDbContext context,
        [FromServices] ClientService service)
    {
        var result = await service.CreateRange(ModelState, clients);
        if (result.IsBadResult())
            return BadRequest(result);
        return Created("/v1/clients/post-range", result);
    }
}