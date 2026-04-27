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
    
    [HttpPut("/v1/employees/")]
    public async Task<IActionResult> PutAsync
    ([FromBody] EmployeeUpdateDto employeeUpdateDto,
    [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.UpdateAsync(ModelState, employeeUpdateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Ok(result.Data);
    }
    
    [HttpPut("/v1/employees/many")]
    public async Task<IActionResult> PutRangeAsync
        ([FromBody] IEnumerable<EmployeeUpdateDto> employeeUpdateDtos,
        [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.UpdateRangeAsync(ModelState, employeeUpdateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Ok(result.Data);
    }
}