namespace TodoListAPI.DTOs;

public class TodoListResponse
{
    public required List<TodoItemResponse> Data { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
}
