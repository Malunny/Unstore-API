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

        CreateMap<Employee, EmployeeReadDto>();
        CreateMap<EmployeeCreateDto, Employee>();
        CreateMap<EmployeeUpdateDto, Employee>()
            .ForMember(dest => dest.PositionId, 
                opt => opt.PreCondition(src => src.PositionId.HasValue && src.PositionId > 0))
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Employee, EmployeeUpdateDto>();

        CreateMap<Position, PositionReadDto>();
        CreateMap<PositionCreateDto, Position>();
        CreateMap<PositionUpdateDto, Position>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Position, PositionUpdateDto>();

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

        CreateMap<Tool, ToolReadDto>();
        CreateMap<ToolCreateDto, Tool>();
        CreateMap<ToolUpdateDto, Tool>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Tool, ToolUpdateDto>();

        CreateMap<ToolTag, ToolTagReadDto>();
        CreateMap<ToolTagCreateDto, ToolTag>();
        CreateMap<ToolTagUpdateDto, ToolTag>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<ToolTag, ToolTagUpdateDto>();

        CreateMap<User, UserLoginDto>();
        CreateMap<UserLoginDto, User>();
        CreateMap<UserCreationDto, User>();
        CreateMap<User, UserReadDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<User, UserUpdateDto>();
    }
}