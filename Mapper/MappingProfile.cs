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
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Product, ProductUpdateDto>();

        CreateMap<Role, RoleReadDto>();
        CreateMap<RoleCreateDto, Role>();
        CreateMap<RoleUpdateDto, Role>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Role, RoleUpdateDto>();

        CreateMap<Service, ServiceReadDto>();
        CreateMap<ServiceCreateDto, Service>();
        CreateMap<ServiceUpdateDto, Service>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Service, ServiceUpdateDto>();

        CreateMap<User, UserLoginDto>();
        CreateMap<UserLoginDto, User>();
        CreateMap<UserCreationDto, User>().ForMember(dest => dest.PasswordHash,
            opt => opt.MapFrom(x => x.Password));
        CreateMap<User, UserReadDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => string.Join(", ", src.Roles.Select(x => x.Name))));
        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<User, UserUpdateDto>();
    }
}