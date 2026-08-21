using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static ProductCategory MapToModel(this ProductCategoryCreateDto dto)
        => new ProductCategory
        {
            Key = dto.Key,
            Description = dto.Description,
        };

    public static ProductCategoryReadDto MapToDto(this ProductCategory model)
        => new ProductCategoryReadDto
        {
            Id = model.Id,
            Key = model.Key,
            Description = model.Description
        };

    public static void MapFromUpdateDto(this ProductCategory model, ProductCategoryUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Key)) model.Key = dto.Key;
        if (!string.IsNullOrWhiteSpace(dto.Description)) model.Description = dto.Description;
    }
}
