using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;

namespace Unstore.Controllers.Employee;

[ApiController]
public class EmployeeController : ControllerBase
{
    [HttpGet("/v1/employees/get-all")]
    public async Task<IActionResult> GetAllAsync
        ([FromServices] AppDbContext context)
    {
        return Ok(await context.Employees.ToListAsync());
    }
    [HttpGet("/v1/employees/get/{id:int}")]
    public async Task<IActionResult> GetByIdAsync
        ([FromRoute] int id, [FromServices] AppDbContext context)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (employee is null)
            return NotFound("Employee not found");
        return Ok(employee);
    }
}