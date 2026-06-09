using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services;

public partial class UserService
{
    public async Task<IServiceResult<UserReadDto>> GetById(int id)
    {
        var serviceResultFactory = _serviceResultFactoryProvider.Create<UserReadDto>();
        if (id < 1)
            return serviceResultFactory.Failure(OperationStatus.InvalidInput);

        User? user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return serviceResultFactory.Failure(OperationStatus.NotFound);

        UserReadDto userDto = Mapper.Map<User, UserReadDto>(user);
        
        return serviceResultFactory.Success(userDto);
    }
    
    public async Task<IServiceResult<IEnumerable<UserReadDto>>> GetByIds(int[] ids)
    {
        var serviceResultFactory = _serviceResultFactoryProvider.Create<IEnumerable<UserReadDto>>();
        if (ids.Any(x => x < 1))
            return serviceResultFactory.Failure(OperationStatus.InvalidInput);

        List<User> users = await Context.Users.AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();

        if (users.Count < ids.Length)
            return serviceResultFactory.Failure(OperationStatus.NotFound);

        List<UserReadDto> userDtos = Mapper.Map<List<User>, List<UserReadDto>>(users);

        return serviceResultFactory.Success(userDtos);
    }

    public async Task<IServiceResult<UserReadDto>> GetByEmail(string email)
    {
        var emailChecker = new EmailAddressAttribute();
        var serviceResultFactory = _serviceResultFactoryProvider.Create<UserReadDto>();
        
        if (!emailChecker.IsValid(email))
            return serviceResultFactory.Failure(OperationStatus.InvalidInput);

        User? user = await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        
        if (user is null)
            return serviceResultFactory.Failure(OperationStatus.NotFound);

        UserReadDto userDto = Mapper.Map<User, UserReadDto>(user);
        
        return serviceResultFactory.Success(userDto);
    }

    public async Task<IServiceResult<UserReadDto>> GetByUsername(string username)
    {
        User? user = await Context.Users.FirstOrDefaultAsync(x => x.Username == username);
        var serviceResultFactory = _serviceResultFactoryProvider.Create<UserReadDto>();
        
        if (user is null)
            return serviceResultFactory.Failure(OperationStatus.NotFound);

        UserReadDto userDto = Mapper.Map<User, UserReadDto>(user);
        
        return serviceResultFactory.Success(userDto);
    }
}