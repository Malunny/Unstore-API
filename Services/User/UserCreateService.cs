using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services;

public partial class UserService : BaseService
{
    private readonly IServiceResultFactory _serviceResultFactory;
    public UserService(AppDbContext dbContext, IMapper mapper, IServiceResultFactory serviceResultFactoryProvider) : base(dbContext, mapper)
    {
        _serviceResultFactory = serviceResultFactoryProvider;
    }

    public async Task<IServiceResult<UserCreateDtos>> CreateAsync(UserCreateDtos createDto)
    {
        if (Context.Users.Any(x => x.Email == createDto.Email || x.Username == createDto.Username))
            return _serviceResultFactory.Failure<UserCreateDtos>(OperationStatus.UserAlreadyExists);

        var newUser = Mapper.Map<UserCreateDtos, Models.User>(createDto);

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

        IEnumerable<Models.User> newUsers = Mapper.Map<List<UserCreateDtos>, List<Models.User>>(createDtosList);

        await Context.Users.AddRangeAsync(newUsers);
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success<IEnumerable<UserCreateDtos>>(createDtosList);
    }
}