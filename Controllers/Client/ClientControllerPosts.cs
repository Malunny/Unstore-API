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
    ([FromBody] ClientCreateDto clientCreateDto,
    [FromServices] AppDbContext context,
    [FromServices] ClientService clientService)
    {
        var result = await clientService.CreateAsync(ModelState, clientCreateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Created("/v1/clients/post", result.Data);
    }
    
    [HttpPost("/v1/clients/many")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<ClientCreateDto> clientCreateDtos,
        [FromServices] ClientService clientService)
    {
        var result = await clientService.CreateRangeAsync(ModelState, clientCreateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Created($"/v1/clients/", result.Data);
    }
}