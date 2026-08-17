using Unstore.Models;

namespace Unstore.DTOs.Mapping;

public static partial class DtoMapping
{
    // Service mappings
    public static Service MapToModel(this ServiceCreateDto dto)
        => new Service
        {
            Title = dto.Title,
            Description = dto.Description,
            LowestPrice = dto.LowestPrice,
            ProviderId = dto.ProviderId,
            AvailableAt = dto.AvailableAt,
            Active = true
        };

    public static ServiceReadDto MapToDto(this Service model)
        => new ServiceReadDto
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            LowestPrice = model.LowestPrice,
            AvailableAt = model.AvailableAt,
            Active = model.Active,
            ProviderId = model.ProviderId
        };

    public static void MapFromUpdateDto(this Service model, ServiceUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Title)) model.Title = dto.Title;
        if (!string.IsNullOrWhiteSpace(dto.Description)) model.Description = dto.Description;
        if (dto.LowestPrice.HasValue) model.LowestPrice = dto.LowestPrice.Value;
        if (dto.AvailableAt.HasValue) model.AvailableAt = dto.AvailableAt.Value;
        if (dto.Active.HasValue) model.Active = dto.Active.Value;
    }

    // ServiceOption mappings
    public static ServiceOption MapToModel(this ServiceServiceOptionCreateDto dto)
        => new ServiceOption
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            ServiceId = dto.ServiceId
        };

    public static ServiceServiceOptionReadDto MapToDto(this ServiceOption model)
        => new ServiceServiceOptionReadDto
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            Price = model.Price
        };

    public static void MapFromUpdateDto(this ServiceOption model, ServiceServiceOptionUpdateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Title)) model.Title = dto.Title;
        if (!string.IsNullOrWhiteSpace(dto.Description)) model.Description = dto.Description;
        if (dto.Price.HasValue) model.Price = dto.Price.Value;
    }

    // ServiceRequest mappings
    public static ServiceRequest MapToModel(this ServiceServiceRequestCreateDto dto)
        => new ServiceRequest
        {
            ServiceId = dto.ServiceId,
            RequesterId = dto.RequesterId,
            RequestedToDay = dto.RequestedToDay,
            RequestedAt = DateTime.UtcNow
        };

    public static ServiceServiceRequestReadDto MapToDto(this ServiceRequest model)
        => new ServiceServiceRequestReadDto
        {
            Id = model.Id,
            ServiceId = model.ServiceId,
            RequesterId = model.RequesterId,
            RequestedAt = model.RequestedAt,
            RequestedToDay = model.RequestedToDay
        };

    public static void MapFromUpdateDto(this ServiceRequest model, ServiceServiceRequestUpdateDto dto)
    {
        var requestedToDay = dto.RequestedToDay;
        model.RequestedToDay = requestedToDay ?? model.RequestedToDay;
    }
}
