using Microsoft.AspNetCore.Mvc;
using Unstore.Services;

namespace Unstore.Controllers;

public partial class EmployeeController : ControllerBase
{
    [HttpGet("/v1/employees/{skip:int}/{take:int}")]
    public async Task<IActionResult> GetSkipTakeAsync
        ([FromServices] EmployeeService employeeService,
        [FromRoute] int skip,
        [FromRoute] int take)
    {
        var result = await employeeService.GetRangeAsync(skip, take);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
    
    [HttpGet("/v1/employees/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
            ([FromRoute] int id,
            [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.GetByIdAsync(id);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}