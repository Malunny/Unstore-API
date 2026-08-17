using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static AddressType MapToModel(this AddressTypeCreateDto dto)
        => new AddressType
        {
            Key = dto.Key,
            Description = dto.Description
        };

    public static AddressTypeReadDto MapToDto(this AddressType model)
        => new AddressTypeReadDto
        {
            Id = model.Id,
            Key = model.Key,
            Description = model.Description
        };

    public static void MapFromUpdateDto(this AddressType model, AddressTypeUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Key)) model.Key = dto.Key;
        if (!string.IsNullOrWhiteSpace(dto.Description)) model.Description = dto.Description;
    }
}
