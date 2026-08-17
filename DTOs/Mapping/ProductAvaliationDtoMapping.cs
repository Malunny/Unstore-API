using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static ProductAvaliation MapToModel(this ProductAvaliationCreateDto dto)
        => new ProductAvaliation
        {
            UserId = dto.UserId,
            ProductId = dto.ProductId,
            Description = dto.Description,
            Stars = dto.Stars
        };

    public static ProductAvaliationReadDto MapToDto(this ProductAvaliation model)
        => new ProductAvaliationReadDto
        {
            UserId = model.UserId,
            ProductId = model.ProductId,
            Description = model.Description,
            Stars = model.Stars
        };

    public static ICollection<ProductAvaliationReadDto> MapToDto(this ICollection<ProductAvaliation> items)
        => items.Select(x => x.MapToDto()).ToList();
}
