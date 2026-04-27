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
    
    [HttpPut("/v1/products/")]
    public async Task<IActionResult> PutAsync
    ([FromBody] ProductUpdateDto productUpdateDto,
    [FromServices] ProductService productService)
    {
        var result = await productService.UpdateAsync(ModelState, productUpdateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Ok(result.Data);
    }
    
    [HttpPut("/v1/products/many")]
    public async Task<IActionResult> PutRangeAsync
        ([FromBody] IEnumerable<ProductUpdateDto> productUpdateDtos,
        [FromServices] ProductService productService)
    {
        var result = await productService.UpdateRangeAsync(ModelState, productUpdateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}