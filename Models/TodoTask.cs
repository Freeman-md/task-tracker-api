using api.enums;

namespace api.Models;
public class TodoTask
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public required TodoTaskStatus TodoTaskStatus { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public TodoTask() {}
}
