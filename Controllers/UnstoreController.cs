using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;

namespace Unstore.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public abstract class UnstoreController : ControllerBase
{
    // [NonAction]
    // public async Task<Models.User?> GetUserByUsername([FromServices] AppDbContext context)
    // {
    //     string? username = User.Identity?.Name;
    //
    //     if (string.IsNullOrEmpty(username))
    //         return null;
    //     
    //     Models.User? user = await context.Users.FirstOrDefaultAsync(user => user.Username == username);
    //     
    //     return user;
    // }
}