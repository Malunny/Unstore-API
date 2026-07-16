using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record ServiceReadDto
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public decimal LowestPrice { get; set; }
    [Required]
    public DateOnly AvailableAt { get; set; }
    [Required]
    public bool Active { get; set; }
    [Required]
    public int ProviderId { get; set; }
}

public record ServiceCreateDto
{
    [Required(ErrorMessage = "Service title is required")]
    [MaxLength(100, ErrorMessage = "Service title must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Service title must be at least 2 characters long")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Service description is required")]
    [MaxLength(500, ErrorMessage = "Service description must not exceed 500 characters")]
    [MinLength(5, ErrorMessage = "Service description must be at least 5 characters long")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Lowest price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Lowest price must be greater than 0")]
    public decimal LowestPrice { get; set; }
    
    [Required]
    public int ProviderId { get; set; }
    
    [Required(ErrorMessage = "Available date is required")]
    public DateOnly AvailableAt { get; set; }
}

public record ServiceUpdateDto
{
    [Required(ErrorMessage = "Service ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Service title must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Service title must be at least 2 characters long")]
    public string? Title { get; set; }
    
    [MaxLength(500, ErrorMessage = "Service description must not exceed 500 characters")]
    [MinLength(5, ErrorMessage = "Service description must be at least 5 characters long")]
    public string? Description { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Lowest price must be greater than 0")]
    public decimal? LowestPrice { get; set; }
    
    public DateOnly? AvailableAt { get; set; }
    
    public bool? Active { get; set; }
}

// Service child entities DTOs

public record ServiceServiceOptionReadDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
}

public record ServiceServiceOptionCreateDto
{
    [Required(ErrorMessage = "Service option title is required")]
    [MaxLength(100, ErrorMessage = "Service option title must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Service option title must be at least 2 characters long")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Service option description is required")]
    [MaxLength(500, ErrorMessage = "Service option description must not exceed 500 characters")]
    [MinLength(3, ErrorMessage = "Service option description must be at least 3 characters long")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Service ID is required")]
    public int ServiceId { get; set; }
}

public record ServiceServiceOptionUpdateDto
{
    [Required(ErrorMessage = "Service option ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Service option title must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Service option title must be at least 2 characters long")]
    public string? Title { get; set; }
    
    [MaxLength(500, ErrorMessage = "Service option description must not exceed 500 characters")]
    [MinLength(3, ErrorMessage = "Service option description must be at least 3 characters long")]
    public string? Description { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal? Price { get; set; }
}

public record ServiceServiceRequestReadDto
{
    public int Id { get; set; }
    [Required]
    public int ServiceId { get; set; }
    [Required]
    public int RequesterId { get; set; }
    [Required]
    public DateTime RequestedAt { get; set; }
    [Required]
    public DateTime RequestedToDay { get; set; }
}

public record ServiceServiceRequestCreateDto
{
    [Required(ErrorMessage = "Service ID is required")]
    public int ServiceId { get; set; }
    
    [Required(ErrorMessage = "Requester ID is required")]
    public int RequesterId { get; set; }
    
    [Required(ErrorMessage = "Requested date is required")]
    public DateTime RequestedToDay { get; set; }
}

public record ServiceServiceRequestUpdateDto
{
    [Required(ErrorMessage = "Service request ID is required")]
    public int Id { get; set; }

    public DateTime? RequestedToDay { get; set; }
}
