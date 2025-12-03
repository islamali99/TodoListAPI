using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.DTOs;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodosController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }

    [HttpGet]
    public async Task<ActionResult<TodoListResponse>> GetTodos(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (page < 1) page = 1;
        if (limit < 1) limit = 10;
        if (limit > 100) limit = 100;

        var userId = GetUserId();

        var query = _context.TodoItems
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt);

        var total = await query.CountAsync();
        
        var items = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(t => new TodoItemResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(new TodoListResponse
        {
            Data = items,
            Page = page,
            Limit = limit,
            Total = total
        });
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemResponse>> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var userId = GetUserId();

        var todoItem = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            UserId = userId
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        var response = new TodoItemResponse
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            CreatedAt = todoItem.CreatedAt,
            UpdatedAt = todoItem.UpdatedAt
        };

        return CreatedAtAction(nameof(GetTodos), new { id = todoItem.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItemResponse>> UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
    {
        var userId = GetUserId();

        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound(new ErrorResponse { Message = "Todo item not found" });
        }

        if (todoItem.UserId != userId)
        {
            return StatusCode(403, new ErrorResponse { Message = "Forbidden" });
        }

        // Update only provided fields
        if (request.Title != null)
        {
            todoItem.Title = request.Title;
        }

        if (request.Description != null)
        {
            todoItem.Description = request.Description;
        }

        if (request.IsCompleted.HasValue)
        {
            todoItem.IsCompleted = request.IsCompleted.Value;
        }

        todoItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var response = new TodoItemResponse
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            CreatedAt = todoItem.CreatedAt,
            UpdatedAt = todoItem.UpdatedAt
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var userId = GetUserId();

        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound(new ErrorResponse { Message = "Todo item not found" });
        }

        if (todoItem.UserId != userId)
        {
            return StatusCode(403, new ErrorResponse { Message = "Forbidden" });
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
