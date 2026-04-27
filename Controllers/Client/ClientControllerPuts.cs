using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class ClientController
{
    
    [HttpPut("/v1/clients/")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] ClientUpdateDto clientUpdateDto,
        [FromServices] ClientService clientService)
    {
        var result = await clientService.UpdateAsync(ModelState, clientUpdateDto);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
    [HttpPut("/v1/clients/many")]
    public async Task<IActionResult> PutByIdAsync
        ([FromBody] IEnumerable<ClientUpdateDto> clientUpdateDtos, 
        [FromServices] ClientService clientService)
    {
        var result = await clientService.UpdateRangeAsync(ModelState, clientUpdateDtos);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        return Ok(result.Data);
    }
}