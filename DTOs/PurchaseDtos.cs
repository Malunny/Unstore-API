using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record PurchaseReadDto
{
    public int Id { get; set; }
    [Required]
    public DateTime BoughtDate { get; set; }
    [Required]
    public ICollection<ProductPurchaseReadDto> ProductsPurchases { get; set; } = new List<ProductPurchaseReadDto>();
    [Required]
    public decimal TotalValue { get; set; }
    [Required]
    public int AddressId { get; set; }
    [Required]
    public int UserId { get; set; }
}

public record PurchaseCreateDto
{
    [Required(ErrorMessage = "Address ID is required")]
    public int AddressId { get; set; }
    
    [Required(ErrorMessage = "Total value is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total value must be greater than 0")]
    public decimal TotalValue { get; set; }
    
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
}

public record PurchaseUpdateDto
{
    [Required(ErrorMessage = "Purchase ID is required")]
    public int Id { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Total value must be greater than 0")]
    public decimal? TotalValue { get; set; }
    
    public int? AddressId { get; set; }
}
