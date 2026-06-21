using System.ComponentModel.DataAnnotations;
using Unstore.Models;

namespace Unstore.DTOs;

public record CommercialUserCreateDto
{
    [Required]
    [MaxLength(100)]
    public string CommercialName { get; set; }
    [Required]
    [MaxLength(500)]
    public string About { get; set; }
    [Required]
    public int OriginalUserId { get; set; }
}

public record CommercialUserUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string CommercialName { get; set; }
    [Required]
    [MaxLength(500)]
    public string About { get; set; }
}

public record CommercialUserReadDto
{
    public int Id { get; set; }
    public string CommercialName { get; set; }
    public string About { get; set; }
}