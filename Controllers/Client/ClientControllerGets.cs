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
        ([FromServices] ClientService clientService,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var result = await clientService.GetRangeAsync(skip, take);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
    
    [HttpGet("/v1/clients/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] ClientService clientService)
    {
        var result = await clientService.GetByIdAsync(id);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}