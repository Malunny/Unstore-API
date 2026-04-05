using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ClientCreateDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Address must be between 3 and 150 characters")]
    public string Address { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Phone is required")]
    [Phone(ErrorMessage = "Invalid phone format")]
    public string ContactNumber { get; set; }
}