using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;
using Unstore.Models;

namespace Unstore.Services;

public partial class UserService
{
    public async Task<IServiceResult<UserReadDto>> GetByIdAsync(int id)
    {
        if (id < 1)
            return _serviceResultFactory.Failure<UserReadDto>(OperationStatus.InvalidInput);

        Models.User? user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return _serviceResultFactory.Failure<UserReadDto>(OperationStatus.NotFound);

        UserReadDto userDto = user.MapToDto();
        
        return _serviceResultFactory.Success(userDto);
    }
    
    public async Task<IServiceResult<IEnumerable<UserReadDto>>> GetByIdsAsync(int[] ids)
    {
        if (ids.Any(x => x < 1))
            return _serviceResultFactory.Failure<IEnumerable<UserReadDto>>(OperationStatus.InvalidInput);

        List<Models.User> users = await Context.Users.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();

        if (users.Count < ids.Length)
            return _serviceResultFactory.Failure<IEnumerable<UserReadDto>>(OperationStatus.NotFound);

        List<UserReadDto> userDtos = users.Select(x => x.MapToDto()).ToList();

        return _serviceResultFactory.Success<IEnumerable<UserReadDto>>(userDtos);
    }

    public async Task<IServiceResult<UserReadDto>> GetByEmailAsync(string email)
    {
        var emailChecker = new EmailAddressAttribute();
        
        if (!emailChecker.IsValid(email))
            return _serviceResultFactory.Failure<UserReadDto>(OperationStatus.InvalidInput);

        Models.User? user = await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        
        if (user is null)
            return _serviceResultFactory.Failure<UserReadDto>(OperationStatus.NotFound);

        UserReadDto userDto = user.MapToDto();
        
        return _serviceResultFactory.Success(userDto);
    }

    public async Task<IServiceResult<UserReadDto>> GetByUsernameAsync(string username)
    {   
        Models.User? user = await Context.Users
            .Include(user => user.Roles)
            .Include(user => user.Addresses)
            .FirstOrDefaultAsync(x => x.Username == username);
        
        if (user is null)
            return _serviceResultFactory.Failure<UserReadDto>(OperationStatus.NotFound);

        UserReadDto userDto = user.MapToDto();
        
        return _serviceResultFactory.Success(userDto);
    }
}