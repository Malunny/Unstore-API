using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record ProductPurchaseReadDto
{
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int PurchaseId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}

public record ProductPurchaseCreateDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public int ProductId { get; set; }
    
    [Required(ErrorMessage = "Purchase ID is required")]
    public int PurchaseId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}
