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

    public async Task<IServiceResult<UserCreationDto>> Create(UserCreationDto createDto)
    {
        if (Context.Users.Any(x => x.Email == createDto.Email || x.Username == createDto.Username))
            return _serviceResultFactory.Failure<UserCreationDto>(OperationStatus.UserAlreadyExists);

        var newUser = Mapper.Map<UserCreationDto, User>(createDto);

        await Context.Users.AddAsync(newUser);
        await Context.SaveChangesAsync();

        return _serviceResultFactory.Success(createDto);
    }
    
    public async Task<IServiceResult<IEnumerable<UserCreationDto>>> CreateRange(IEnumerable<UserCreationDto> createDtos)
    {
        var createDtosList = createDtos.ToList();
        var usersUsernamesExists = await Context.Users.AnyAsync(x => createDtosList.Select(x => x.Username).Contains(x.Username));
        var usersEmailsExists = await Context.Users.AnyAsync(x => createDtosList.Select(x => x.Email).Contains(x.Email));
        
        if (usersUsernamesExists || usersEmailsExists)
            return _serviceResultFactory.Failure<IEnumerable<UserCreationDto>>(OperationStatus.UserAlreadyExists);

        IEnumerable<User> newUsers = Mapper.Map<List<UserCreationDto>, List<User>>(createDtosList);

        await Context.Users.AddRangeAsync(newUsers);
        await Context.SaveChangesAsync();
        
        return _serviceResultFactory.Success<IEnumerable<UserCreationDto>>(createDtosList);
    }
}