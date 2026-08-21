using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping 
{
    public static Address MapToModel(this UserAddressCreateDto dto) => new Address
    {
        State =  dto.State,
        City = dto.City,
        Street = dto.Street,
        Number = dto.Number,
        ZipCode = dto.ZipCode,
        Complement =  dto.Complement,
        TypeId = dto.TypeId,
        UserId = dto?.UserId ?? 0,
    };

    public static UserAddressReadDto MapToDto(this Address address) => new UserAddressReadDto()
    {
        Id =  address.Id,
        UserId =  address.UserId,
        State = address.State,
        City = address.City,
        Street = address.Street,
        Number = address.Number,
        ZipCode = address.ZipCode,
        Complement = address.Complement,
        TypeId = address.TypeId,
    };

    public static ICollection<UserAddressReadDto> MapToDto(this ICollection<Address> addresses)
        => addresses.Select(address => address.MapToDto()).ToList();

    public static void MapFromUpdateDto(this Address address, UserAddressUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.State)) address.State = dto.State;
        if (!string.IsNullOrWhiteSpace(dto.City)) address.City = dto.City;
        if (!string.IsNullOrWhiteSpace(dto.Street)) address.Street = dto.Street;
        if (!string.IsNullOrWhiteSpace(dto.Number)) address.Number = dto.Number;
        if (!string.IsNullOrWhiteSpace(dto.ZipCode)) address.ZipCode = dto.ZipCode;
        if (dto.Complement != null) address.Complement = dto.Complement;
    }
}