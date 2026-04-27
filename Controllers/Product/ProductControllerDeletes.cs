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
    
    [HttpDelete("/v1/products/{id:int}")]
    public async Task<IActionResult> DeleteAsync
    ([FromRoute] int id,
    [FromServices] ProductService productService)
    {
        var result = await productService.RemoveAsync(id);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return NoContent();
    }
    
    [HttpDelete("/v1/products/many")]
    public async Task<IActionResult> DeleteRangeAsync
        ([FromBody] IEnumerable<int> ids,
        [FromServices] ProductService productService)
    {
        var result = await productService.RemoveRangeAsync(ids);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return NoContent();
    }
}