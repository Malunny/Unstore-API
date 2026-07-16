using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;

namespace Unstore.Services.User;

public class UserVerificationService([FromServices] AppDbContext context)
{
    public async Task<int> VerifyUserExistenceAsync(string? username)
    {
        if (string.IsNullOrEmpty(username))
            return -1;
        
        var userIdUsername = await context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, x.Username })
            .FirstOrDefaultAsync(x => x.Username == username);

        
        if (userIdUsername != null)
            return userIdUsername.Id;

        return -1;
    }

    public async Task<int> VerifyUserExistenceByEmailAsync(string email)
    {
        var userIdEmail = await context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, x.Email })
            .FirstOrDefaultAsync(x => x.Email == email);
        
        if (userIdEmail != null)
            return userIdEmail.Id;

        return -1;
    }

    public async Task<int> VerifyUserExistenceAsync(int userId)
    {
        var userIdEmail = await context.Users
            .AsNoTracking()
            .Select(x => x.Id)
            .FirstOrDefaultAsync(x => x == userId);
        
        if (userIdEmail != 0)
            return userIdEmail;

        return -1;
    }
}