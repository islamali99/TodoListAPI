using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.DTOs;

public class CreateTodoRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public required string Title { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
}
