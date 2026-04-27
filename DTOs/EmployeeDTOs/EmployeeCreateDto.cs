using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record EmployeeCreateDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Contact number is required")]
    [Phone(ErrorMessage = "Invalid phone format")]
    public string ContactNumber { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Position ID is required")]
    public int PositionId { get; set; }
    
    [Required(ErrorMessage = "Started at date is required")]
    public DateTime StartedAt { get; set; }
    
    public bool Active { get; set; } = true;
}