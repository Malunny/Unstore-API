using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record AddressTypeReadDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Key { get; set; }
    [Required]
    [MaxLength(200)]
    public string Description { get; set; }
}

public record AddressTypeCreateDto
{
    [Required(ErrorMessage = "Address type key is required")]
    [MaxLength(100, ErrorMessage = "Address type key must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Address type key must be at least 2 characters long")]
    public string Key { get; set; }
    
    [Required(ErrorMessage = "Address type description is required")]
    [MaxLength(200, ErrorMessage = "Address type description must not exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Address type description must be at least 3 characters long")]
    public string Description { get; set; }
}

public record AddressTypeUpdateDto
{
    [Required(ErrorMessage = "Address type ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "Address type key must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "Address type key must be at least 2 characters long")]
    public string? Key { get; set; }
    
    [MaxLength(200, ErrorMessage = "Address type description must not exceed 200 characters")]
    [MinLength(3, ErrorMessage = "Address type description must be at least 3 characters long")]
    public string? Description { get; set; }
}
