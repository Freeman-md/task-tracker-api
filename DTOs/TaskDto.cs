using api.enums;

namespace api.DTOs;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TodoTaskStatus TodoTaskStatus { get; set; }
    public DateTime DueDate { get; set; }
}
