using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }
}
