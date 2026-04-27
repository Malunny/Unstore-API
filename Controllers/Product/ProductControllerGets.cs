using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Extensions;
using Unstore.Data;
using Unstore.Models;
using Unstore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Unstore.Controllers;

public partial class ProductController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("/v1/products/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetSkipTake
        ([FromServices] ProductService productService,
        [FromServices] IMemoryCache cache,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var result = await productService.GetRangeAsync(skip, take, cache);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
    [AllowAnonymous]
    [HttpGet("/v1/products/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] ProductService productService)
    {
        var result = await productService.GetByIdAsync(id);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}