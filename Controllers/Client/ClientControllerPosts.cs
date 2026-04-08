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
    
    [HttpPost("/v1/clients/")]
    public async Task<IActionResult> PostAsync
    ([FromBody] ClientCreateDto client,
    [FromServices] AppDbContext context,
    [FromServices] ClientService service)
    {
        var result = await service.CreateAsync(ModelState, client);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Created("/v1/clients/post", result.Data);
    }
    
    [HttpPost("/v1/clients/many")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<ClientCreateDto> clients,
        [FromServices] ClientService service)
    {
        var result = await service.CreateRangeAsync(ModelState, clients);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Created($"/v1/clients/", result.Data);
    }
}