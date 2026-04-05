using System.ComponentModel.DataAnnotations;

namespace Unstore.DTOs;

public record UserDto
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; }
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}