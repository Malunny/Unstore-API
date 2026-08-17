using System.Linq;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;
using Unstore.Models;

namespace Unstore.Services.User;

public class UserManagementService : BaseService
{
    public UserManagementService(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IServiceResult<List<UserReadDto>>> GetAllAsync()
    {
        var users = await Context.Users
            .AsNoTracking()
            .Where(x => x.Active)
            .ToListAsync();
        var dtos = users.Select(x => x.MapToDto()).ToList();
        return new DataServiceResult<List<UserReadDto>>(true, dtos);
    }

    public async Task<IServiceResult<UserReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

        var dto = user.MapToDto();
        return new DataServiceResult<UserReadDto>(true, dto);
    }

    public async Task<IServiceResult<UserReadDto>> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

        var dto = user.MapToDto();
        return new DataServiceResult<UserReadDto>(true, dto);
    }

    public async Task<IServiceResult<UserReadDto>> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

        var dto = user.MapToDto();
        return new DataServiceResult<UserReadDto>(true, dto);
    }
    
    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        Context.Users.Remove(user);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
    }

    public async Task<IServiceResult<bool>> DeactivateAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        user.Active = false;
        Context.Users.Update(user);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Updated, true, true);
    }

    public async Task<IServiceResult<bool>> ActivateAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return new DataServiceResult<bool>(OperationStatus.NotFound, false);

        user.Active = true;
        Context.Users.Update(user);
        await Context.SaveChangesAsync();

        return new DataServiceResult<bool>(OperationStatus.Updated, true, true);
    }
}
