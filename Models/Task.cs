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

    public TodoTask(string title, string description, TaskStatus taskStatus, DateTime? dueDate = null)
{
    Id = Guid.NewGuid();
    Title = title;
    Description = description;
    TaskStatus = taskStatus;
    DueDate = dueDate ?? DateTime.Now.AddDays(1);
}

}
