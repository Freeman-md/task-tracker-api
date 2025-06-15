namespace api.Models;

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}

public class TodoTask
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public required TaskStatus TaskStatus { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public TodoTask() {}
}
