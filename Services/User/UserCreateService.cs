using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unstore.Data;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Services;

public partial class UserService : BaseService
{
    private readonly IServiceResultFactoryProvider _serviceResultFactoryProvider;
    public UserService(AppDbContext dbContext, IMapper mapper, IServiceResultFactoryProvider serviceResultFactoryProvider) : base(dbContext, mapper)
    {
        _serviceResultFactoryProvider = serviceResultFactoryProvider;
    }

    public async Task<IServiceResult<UserCreationDto>> Create(UserCreationDto createDto)
    {
        var serviceResultFactory = _serviceResultFactoryProvider.Create<UserCreationDto>();

        if (Context.Users.Any(x => x.Email == createDto.Email || x.Username == createDto.Username))
            return serviceResultFactory.Failure(OperationStatus.UserAlreadyExists);

        var newUser = Mapper.Map<UserCreationDto, User>(createDto);

        await Context.Users.AddAsync(newUser);
        await Context.SaveChangesAsync();

        return serviceResultFactory.Success(createDto);
    }
    
    public async Task<IServiceResult<IEnumerable<UserCreationDto>>> CreateRange(IEnumerable<UserCreationDto> createDtos)
    {
        var serviceResultFactory = _serviceResultFactoryProvider.Create<IEnumerable<UserCreationDto>>();
        
        var createDtosList = createDtos.ToList();
        var usersUsernamesExists = await Context.Users.AnyAsync(x => createDtosList.Select(x => x.Username).Contains(x.Username));
        var usersEmailsExists = await Context.Users.AnyAsync(x => createDtosList.Select(x => x.Email).Contains(x.Email));
        
        if (usersUsernamesExists || usersEmailsExists)
            return serviceResultFactory.Failure(OperationStatus.UserAlreadyExists);

        IEnumerable<User> newUsers = Mapper.Map<List<UserCreationDto>, List<User>>(createDtosList);

        await Context.Users.AddRangeAsync(newUsers);
        await Context.SaveChangesAsync();
        
        return serviceResultFactory.Success(createDtosList);
    }
}