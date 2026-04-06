using AutoMapper;
using Microsoft.Extensions.Options;
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
        CreateMap<ClientUpdateDto, Client>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str)))); 
        CreateMap<Client, ClientUpdateDto>();

        CreateMap<User, UserLoginDto>();
        CreateMap<UserLoginDto, User>();
        CreateMap<UserCreationDto, User>();
        CreateMap<User, UserReadDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
    }
}