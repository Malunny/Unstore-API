using AutoMapper;
using Unstore.DTO;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, ClientReadDto>();
        CreateMap<ClientCreateDto, Client>();
        CreateMap<ClientUpdateDto, Client>(); 
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserCreationDto, User>();
        CreateMap<User, UserGetDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
    }
}