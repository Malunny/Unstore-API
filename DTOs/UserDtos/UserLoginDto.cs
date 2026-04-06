using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record UserLoginDto
{
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}