using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record UserUpdateDto
{
    [Required(ErrorMessage = "User ID is required.")]
    public int Id { get; set; }

    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long")]
    public string? Name { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
    public string? Username { get; set; }
}