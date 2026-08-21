using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Unstore.DTOs;
public record UserCreateDtos
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 3 characters long")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}
public record UserUpdateDto
{
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long")]
    public string Name { get; set; }    
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
}
public class UserReadDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>();
    public ICollection<UserAddressReadDto> Addresses { get; set; } = new List<UserAddressReadDto>();
}
public record UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
