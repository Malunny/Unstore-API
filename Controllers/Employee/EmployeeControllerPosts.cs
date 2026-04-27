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
    
    [HttpPost("/v1/employees/")]
    public async Task<IActionResult> PostAsync
    ([FromBody] EmployeeCreateDto employeeCreateDto,
    [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.CreateAsync(ModelState, employeeCreateDto);
        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);
        else
            return Created("/v1/employees/post", result.Data);
    }
    
    [HttpPost("/v1/employees/many")]
    public async Task<IActionResult> PostRangeAsync
        ([FromBody] IEnumerable<EmployeeCreateDto> employeeCreateDtos,
        [FromServices] EmployeeService employeeService)
    {
        var result = await employeeService.CreateRangeAsync(ModelState, employeeCreateDtos);

        if (result.IsBadResult())
            return BadRequest(result.StatusMessage);

        return Created($"/v1/employees/", result.Data);
    }
}