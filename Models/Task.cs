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
    public required string Text { get; set; }
    public required TaskStatus TaskStatus { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public TodoTask() {}

    public TodoTask(string text, TaskStatus taskStatus)
    {
        Id = Guid.NewGuid();
        Text = text;
        TaskStatus = taskStatus;
    }
}
