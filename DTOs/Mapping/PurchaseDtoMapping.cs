using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    public static PurchaseReadDto MapToReadDto(this Purchase userPurchase)
    {
        var dto = new PurchaseReadDto();

        dto.Id = userPurchase.Id;
        dto.UserId = userPurchase.UserId;
        dto.AddressId = userPurchase.Address.Id;
        dto.BoughtDate = userPurchase.BoughtDate;
        dto.TotalValue = userPurchase.TotalValue;

        return dto;
    }
}