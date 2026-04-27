using System.ComponentModel.DataAnnotations;

namespace Unstore.DTO;

public record ServiceCreateDto
{
    [Required(ErrorMessage = "Client ID is required")]
    public int ClientId { get; set; }
    
    [Required(ErrorMessage = "Employee ID is required")]
    public int EmployeeId { get; set; }
    
    [Required(ErrorMessage = "Details are required")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Details must be between 10 and 1000 characters")]
    public string Details { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Address must be between 3 and 150 characters")]
    public string Address { get; set; }
    [Required]
    public IEnumerable<int> ToolsIds { get; set; }
    [Required]
    public IEnumerable<int> ProductsIds { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Cost must be positive.")]
    public decimal Cost { get; set; }
}