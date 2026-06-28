using Microsoft.AspNetCore.Mvc;
using Unstore.DTOs;
using Unstore.Services;
using Unstore.Services.Product;

namespace Unstore.Controllers.Products;

[ApiController]
[Route("/api/products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("/api/products/{id:int}")]
    public async Task<IActionResult> GetProductByIdAsync
        ([FromRoute] int id,
         [FromServices] ProductService service)
    {
        var product = await service.GetByIdAsync(id);
        if (!product.Ok)
            return NotFound();
        return Ok(product);
    }

    [HttpGet]
    [Route("/api/products/{begin:int}/{end:int}")]
    public async Task<IActionResult> GetRangeAsync([FromServices] ProductService service,
        [FromServices] AuthorizationService authorizationService,
        [FromRoute] int begin, [FromRoute] int end)
    {
        var isAuthorized = authorizationService.IsManagerOrAdmin(User);
        var result = await service.GetRangeAsync(begin, end, isAuthorized);
        
        if (!result.Ok)
            return BadRequest(result.OperationStatus);
        
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreateDto dto,
        [FromServices] ProductService service)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var username = User?.Identity?.Name;
        
        if (username is null)
            return BadRequest();
        var serviceResult = await service.CreateAsync(dto, username);
        
        if (!serviceResult.Ok)
            return BadRequest(serviceResult.OperationStatus);
        
        return Created("/api/products", serviceResult.Data);
    }

    [HttpPut]
    [Route("/api/products")]
    public async Task<IActionResult> UpdateProductAsync([FromBody] ProductUpdateDto dto, [FromServices] ProductService service)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var serviceResult = await service.UpdateAsync(dto);

        if (!serviceResult.Ok)
            return BadRequest(serviceResult.OperationStatus);
        
        return Ok(serviceResult.Data);
    }

    [HttpDelete]
    [Route("/api/products/{id:int}")]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] int id, [FromServices] ProductService service)
    {
        var serviceResult = await service.DeleteAsync(id);

        if (!serviceResult.Ok)
            return BadRequest(serviceResult.Data);

        return NoContent();
    }
}