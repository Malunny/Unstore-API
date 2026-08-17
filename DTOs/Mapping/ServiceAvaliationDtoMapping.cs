using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static ServiceAvaliation MapToModel(this ServiceAvaliationCreateDto dto)
        => new ServiceAvaliation
        {
            ClientId = dto.ClientId,
            ServiceId = dto.ServiceId,
            Stars = dto.Stars,
            Title = dto.Title,
            Description = dto.Description
        };

    public static ServiceAvaliationReadDto MapToDto(this ServiceAvaliation model)
        => new ServiceAvaliationReadDto
        {
            ClientId = model.ClientId,
            ServiceId = model.ServiceId,
            Stars = model.Stars,
            Title = model.Title,
            Description = model.Description
        };

    public static ICollection<ServiceAvaliationReadDto> MapToDto(this ICollection<ServiceAvaliation> items)
        => items.Select(x => x.MapToDto()).ToList();
}
