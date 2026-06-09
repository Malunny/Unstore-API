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

public partial class ProductController
{
    
    [HttpPost("/v1/products/")]
    public async Task<IActionResult> PostAsync
    ([FromBody] ProductCreateDto productCreateDto,
    [FromServices] AppDbContext context,
    [FromServices] ProductService productService)
    {
        var result = await productService.CreateAsync(ModelState, productCreateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Created("/v1/products/post", result.Data);
    }
    
    [HttpPost("/v1/products/many")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<ProductCreateDto> productCreateDtos,
        [FromServices] ProductService productService)
    {
        var result = await productService.CreateRangeAsync(ModelState, productCreateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Created($"/v1/products/", result.Data);
    }
}