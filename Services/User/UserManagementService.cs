using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services.User;

public class UserManagementService : BaseService
{
    public UserManagementService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IServiceResult<List<UserReadDto>>> GetAllAsync()
    {
        try
        {
            var users = await Context.Users
                .AsNoTracking()
                .Where(x => x.Active)
                .ToListAsync();
            var dtos = Mapper.Map<List<UserReadDto>>(users);
            return new DataServiceResult<List<UserReadDto>>(true, dtos);
        }
        catch (Exception)
        {
            return new DataServiceResult<List<UserReadDto>>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<UserReadDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<UserReadDto>(user);
            return new DataServiceResult<UserReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<UserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<UserReadDto>> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<UserReadDto>(user);
            return new DataServiceResult<UserReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<UserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<UserReadDto>> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

            var dto = Mapper.Map<UserReadDto>(user);
            return new DataServiceResult<UserReadDto>(true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<UserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<UserReadDto>> UpdateAsync(UserUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.Id <= 0)
            return new DataServiceResult<UserReadDto>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == updateDto.Id);

            if (user == null)
                return new DataServiceResult<UserReadDto>(OperationStatus.NotFound, false);

            if (updateDto.Username != null && updateDto.Username != user.Username)
            {
                var usernameExists = await Context.Users.AnyAsync(x => x.Username == updateDto.Username && x.Id != updateDto.Id);
                if (usernameExists)
                    return new DataServiceResult<UserReadDto>(OperationStatus.ValidationError, false);
            }

            if (updateDto.Email != null && updateDto.Email != user.Email)
            {
                var emailExists = await Context.Users.AnyAsync(x => x.Email == updateDto.Email && x.Id != updateDto.Id);
                if (emailExists)
                    return new DataServiceResult<UserReadDto>(OperationStatus.ValidationError, false);
            }

            Mapper.Map(updateDto, user);
            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            var dto = Mapper.Map<UserReadDto>(user);
            return new DataServiceResult<UserReadDto>(OperationStatus.Updated, true, dto);
        }
        catch (Exception)
        {
            return new DataServiceResult<UserReadDto>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeleteAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            Context.Users.Remove(user);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Deleted, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> DeactivateAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            user.Active = false;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Updated, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }

    public async Task<IServiceResult<bool>> ActivateAsync(int id)
    {
        if (id <= 0)
            return new DataServiceResult<bool>(OperationStatus.InvalidInput, false);

        try
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new DataServiceResult<bool>(OperationStatus.NotFound, false);

            user.Active = true;
            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return new DataServiceResult<bool>(OperationStatus.Updated, true, true);
        }
        catch (Exception)
        {
            return new DataServiceResult<bool>(OperationStatus.InternalServerError, false);
        }
    }
}
