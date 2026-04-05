using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;

namespace Unstore.Controllers;

public partial class ClientController : ControllerBase
{
    [HttpGet("/v1/clients/get/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetAllAsync
        ([FromServices] AppDbContext context,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var clients = _mapper.Map<IEnumerable<ClientReadDto>>(
        await context.Clients
        .Skip(skip)
        .Take(take)
        .ToListAsync());
        return Ok(new ResultDto<IEnumerable<ClientReadDto>>(clients));    
    }
    
    [HttpGet("/v1/clients/get/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] AppDbContext context)
    {
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == id);
        return Ok(new ResultDto<ClientReadDto>(_mapper.Map<Client, ClientReadDto>(client!)));    
    }
}