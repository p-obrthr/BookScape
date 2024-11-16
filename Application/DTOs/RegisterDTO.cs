using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class RegisterDTO
{
    [Required]
    public string? Name { get; set; } = "";
    [Required, EmailAddress]
    public string? Email { get; set; } = "";
    [Required]
     public string? Password { get; set; } = "";
    [Required, Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; } = "";
}