using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record ProductReadDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public decimal Value { get; set; }
    [Required]
    public DateTime PublishedDate { get; set; }
    [Required]
    public bool Active { get; set; }
}

public record ProductCreateDto
{
    [Required(ErrorMessage = "Product name is required")]
    [MaxLength(100, ErrorMessage = "Product name must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters long")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Product description is required")]
    [MaxLength(500, ErrorMessage = "Product description must not exceed 500 characters")]
    [MinLength(5, ErrorMessage = "Product description must be at least 5 characters long")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Product value is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Product value must be greater than 0")]
    public decimal Value { get; set; }
}

public record ProductUpdateDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Product name must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters long")]
    public string Name { get; set; }
    
    [MaxLength(500, ErrorMessage = "Product description must not exceed 500 characters")]
    [MinLength(5, ErrorMessage = "Product description must be at least 5 characters long")]
    public string Description { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Product value must be greater than 0")]
    public decimal Value { get; set; }
}
