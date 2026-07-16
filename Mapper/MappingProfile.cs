using AutoMapper;
using Microsoft.Extensions.Options;
using Unstore.DTOs;
using Unstore.Models;

namespace Unstore.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Product, ProductUpdateDto>();

        // Role mappings
        CreateMap<Role, RoleReadDto>();
        CreateMap<RoleCreateDto, Role>();
        CreateMap<RoleUpdateDto, Role>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Role, RoleUpdateDto>();

        // Service mappings
        CreateMap<Service, ServiceReadDto>();
        CreateMap<ServiceCreateDto, Service>();
        CreateMap<ServiceUpdateDto, Service>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Service, ServiceUpdateDto>();

        // ServiceOption mappings
        CreateMap<ServiceOption, ServiceServiceOptionReadDto>();
        CreateMap<ServiceServiceOptionCreateDto, ServiceOption>();
        CreateMap<ServiceServiceOptionUpdateDto, ServiceOption>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<ServiceOption, ServiceServiceOptionUpdateDto>();

        // ServiceRequest mappings
        CreateMap<ServiceRequest, ServiceServiceRequestReadDto>();
        CreateMap<ServiceServiceRequestCreateDto, ServiceRequest>();
        CreateMap<ServiceServiceRequestUpdateDto, ServiceRequest>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<ServiceRequest, ServiceServiceRequestUpdateDto>();

        // AddressType mappings
        CreateMap<AddressType, AddressTypeReadDto>();
        CreateMap<AddressTypeCreateDto, AddressType>();
        CreateMap<AddressTypeUpdateDto, AddressType>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<AddressType, AddressTypeUpdateDto>();

        // DocumentType mappings
        CreateMap<DocumentType, DocumentTypeReadDto>();
        CreateMap<DocumentTypeCreateDto, DocumentType>();
        CreateMap<DocumentTypeUpdateDto, DocumentType>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<DocumentType, DocumentTypeUpdateDto>();

        // ProductCategory mappings
        CreateMap<ProductCategory, ProductCategoryReadDto>();
        CreateMap<ProductCategoryCreateDto, ProductCategory>();
        CreateMap<ProductCategoryUpdateDto, ProductCategory>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<ProductCategory, ProductCategoryUpdateDto>();

        // Address mappings (User child entity)
        CreateMap<Address, UserAddressReadDto>();
        CreateMap<UserAddressCreateDto, Address>();
        CreateMap<UserAddressUpdateDto, Address>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Address, UserAddressUpdateDto>();

        // UserDocument mappings
        CreateMap<UserDocument, UserDocumentReadDto>();
        CreateMap<UserDocumentCreateDto, UserDocument>();
        CreateMap<UserDocumentUpdateDto, UserDocument>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<UserDocument, UserDocumentUpdateDto>();

        // CommercialUser mappings
        CreateMap<CommercialUser, CommercialUserReadDto>();
        CreateMap<CommercialUserCreateDto, CommercialUser>();
        CreateMap<CommercialUserUpdateDto, CommercialUser>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<CommercialUser, CommercialUserUpdateDto>();

        // Purchase mappings
        CreateMap<Purchase, PurchaseReadDto>();
        CreateMap<PurchaseCreateDto, Purchase>();
        CreateMap<PurchaseUpdateDto, Purchase>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<Purchase, PurchaseUpdateDto>();

        // ProductPurchase mappings (N:M)
        CreateMap<ProductPurchase, ProductPurchaseReadDto>();
        CreateMap<ProductPurchaseCreateDto, ProductPurchase>();

        // ProductAvaliation mappings (N:M)
        CreateMap<ProductAvaliation, ProductAvaliationReadDto>();
        CreateMap<ProductAvaliationCreateDto, ProductAvaliation>();

        // ServiceAvaliation mappings (N:M)
        CreateMap<ServiceAvaliation, ServiceAvaliationReadDto>();
        CreateMap<ServiceAvaliationCreateDto, ServiceAvaliation>();

        // User mappings
        CreateMap<User, UserLoginDto>();
        CreateMap<UserLoginDto, User>();
        CreateMap<UserCreateDtos, User>().ForMember(dest => dest.PasswordHash,
            opt => opt.MapFrom(x => x.Password));
        CreateMap<User, UserReadDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => string.Join(", ", src.Roles.Select(x => x.Name))));
        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(options => options
                .Condition((origin, dest, originValue) => originValue != null && (originValue is not string str || !string.IsNullOrWhiteSpace(str))));
        CreateMap<User, UserUpdateDto>();
    }
}