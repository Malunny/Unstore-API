using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record UserAddressReadDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string State { get; set; }
    [Required]
    [MaxLength(150)]
    public string City { get; set; }
    [Required]
    [MaxLength(150)]
    public string Street { get; set; }
    [Required]
    [MaxLength(10)]
    public string Number { get; set; }
    [Required]
    [MaxLength(25)]
    public string ZipCode { get; set; }
    [MaxLength(50)]
    public string? Complement { get; set; }
    [Required]
    public int TypeId { get; set; }
    [Required]
    public int UserId { get; set; }
}

public record UserAddressCreateDto
{
    [Required(ErrorMessage = "State is required")]
    [MaxLength(100, ErrorMessage = "State must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "State must be at least 2 characters long")]
    public string State { get; set; }
    
    [Required(ErrorMessage = "City is required")]
    [MaxLength(150, ErrorMessage = "City must not exceed 150 characters")]
    [MinLength(2, ErrorMessage = "City must be at least 2 characters long")]
    public string City { get; set; }
    
    [Required(ErrorMessage = "Street is required")]
    [MaxLength(150, ErrorMessage = "Street must not exceed 150 characters")]
    [MinLength(2, ErrorMessage = "Street must be at least 2 characters long")]
    public string Street { get; set; }
    
    [Required(ErrorMessage = "Number is required")]
    [MaxLength(10, ErrorMessage = "Number must not exceed 10 characters")]
    [MinLength(1, ErrorMessage = "Number is required")]
    public string Number { get; set; }
    
    [Required(ErrorMessage = "ZipCode is required")]
    [MaxLength(25, ErrorMessage = "ZipCode must not exceed 25 characters")]
    [MinLength(4, ErrorMessage = "ZipCode must be at least 4 characters long")]
    public string ZipCode { get; set; }
    
    [MaxLength(50, ErrorMessage = "Complement must not exceed 50 characters")]
    public string? Complement { get; set; }
    
    [Required(ErrorMessage = "Address Type ID is required")]
    public int TypeId { get; set; }
    
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }
}

public record UserAddressUpdateDto
{
    [Required(ErrorMessage = "Address ID is required")]
    public int Id { get; set; }

    [MaxLength(100, ErrorMessage = "State must not exceed 100 characters")]
    [MinLength(2, ErrorMessage = "State must be at least 2 characters long")]
    public string? State { get; set; }
    
    [MaxLength(150, ErrorMessage = "City must not exceed 150 characters")]
    [MinLength(2, ErrorMessage = "City must be at least 2 characters long")]
    public string? City { get; set; }
    
    [MaxLength(150, ErrorMessage = "Street must not exceed 150 characters")]
    [MinLength(2, ErrorMessage = "Street must be at least 2 characters long")]
    public string? Street { get; set; }
    
    [MaxLength(10, ErrorMessage = "Number must not exceed 10 characters")]
    [MinLength(1, ErrorMessage = "Number is required")]
    public string? Number { get; set; }
    
    [MaxLength(25, ErrorMessage = "ZipCode must not exceed 25 characters")]
    [MinLength(4, ErrorMessage = "ZipCode must be at least 4 characters long")]
    public string? ZipCode { get; set; }
    
    [MaxLength(50, ErrorMessage = "Complement must not exceed 50 characters")]
    public string? Complement { get; set; }
    
    public int? TypeId { get; set; }
}

