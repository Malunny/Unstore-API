using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static ProductPurchase MapToModel(this ProductPurchaseCreateDto dto)
        => new ProductPurchase
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

    public static ProductPurchaseReadDto MapToDto(this ProductPurchase model)
        => new ProductPurchaseReadDto
        {
            ProductId = model.ProductId,
            Quantity = model.Quantity
        };

    public static ICollection<ProductPurchaseReadDto> MapToDto(this ICollection<ProductPurchase> items)
        => items.Select(x => x.MapToDto()).ToList();
    public static ICollection<ProductPurchase> MapToModels(this ICollection<ProductPurchaseCreateDto> dtos)
        => dtos.Select(dto => dto.MapToModel()).ToList();
}
