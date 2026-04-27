using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}