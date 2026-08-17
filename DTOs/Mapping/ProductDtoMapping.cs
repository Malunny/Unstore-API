using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static class ProductDtoMapping
{
    public static Product MapToModel(this ProductCreateDto dto, int commercialUserId)
    =>  new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Value = dto.Value,
            Active = true,
            SellerId = commercialUserId,
            PublishedDate = DateTime.UtcNow

        };

    public static ProductReadDto MapToDto(this Product product)
        => new ProductReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
            Active = product.Active,
            PublishedDate = product.PublishedDate
        };

    public static void MapFromUpdateDto(this Product product, ProductUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name)) product.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Description)) product.Description = dto.Description;
        product.Value = dto.Value;
    }
}