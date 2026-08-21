using System.Linq;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.DTOs.Mapping;
using Unstore.Models;

namespace Unstore.Services;

public partial class UserService : BaseService
{
    private readonly IServiceResultFactory _serviceResultFactory;
    public UserService(AppDbContext dbContext, IServiceResultFactory serviceResultFactoryProvider) : base(dbContext)
    {
        _serviceResultFactory = serviceResultFactoryProvider;
    }

    public async Task<IServiceResult<UserCreateDtos>> CreateAsync(UserCreateDtos createDto)
    {
        if (Context.Users.Any(x => x.Email == createDto.Email || x.Username == createDto.Username))
            return _serviceResultFactory.Failure<UserCreateDtos>(OperationStatus.UserAlreadyExists);

        var newUser = createDto.MapToModel();

        await Context.Users.AddAsync(newUser);
        await Context.SaveChangesAsync();

        return _serviceResultFactory.Success(createDto);
    }
    
    public async Task<IServiceResult<IEnumerable<UserCreateDtos>>> CreateAsyncRange(IEnumerable<UserCreateDtos> createDtos)
    {
        var createDtosList = createDtos.ToList();
        var usersUsernamesExists = await Context.Users.AnyAsync(x => createDtosList.Select(y => y.Username).Contains(x.Username));
        var usersEmailsExists = await Context.Users.AnyAsync(x => createDtosList.Select(y => y.Email).Contains(x.Email));
        
        if (usersUsernamesExists || usersEmailsExists)
            return _serviceResultFactory.Failure<IEnumerable<UserCreateDtos>>(OperationStatus.UserAlreadyExists);

        IEnumerable<Models.User> newUsers = createDtosList.Select(x => x.MapToModel()).ToList();

        await Context.Users.AddRangeAsync(newUsers);
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success<IEnumerable<UserCreateDtos>>(createDtosList);
    }
}