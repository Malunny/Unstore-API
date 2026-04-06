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
    [HttpGet("/v1/clients/get/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetAllAsync
        ([FromServices] AppDbContext context,
        [FromServices] ClientService service,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        System.Console.WriteLine(service == null);
        return Ok(await service.GetRangeAsync(skip, take));
    }
    
    [HttpGet("/v1/clients/get/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] AppDbContext context,
            [FromServices] ClientService service)
    {
        return Ok(await service.GetById(id));
    }
}