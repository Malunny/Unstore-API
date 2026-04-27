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

public partial class EmployeeController
{
    
    [HttpDelete("/v1/employees/{id:int}")]
    public async Task<IActionResult> DeleteAsync
    ([FromRoute] int id,
    [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.RemoveAsync(id);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return NoContent();
    }
    
    [HttpDelete("/v1/employees/many")]
    public async Task<IActionResult> DeleteRangeAsync
        ([FromBody] IEnumerable<int> ids,
        [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.RemoveRangeAsync(ids);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return NoContent();
    }
}