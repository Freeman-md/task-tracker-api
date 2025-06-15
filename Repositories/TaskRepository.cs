using api.Data;
using api.DTOs;
using api.Models;
using api.enums;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;

namespace api.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(CreateTaskDto dto, Guid userId)
    {
        var task = new TodoTask
        {
            Title = dto.Title,
            Description = dto.Description,
            TodoTaskStatus = dto.TodoTaskStatus,
            DueDate = dto.DueDate ?? DateTime.UtcNow.AddDays(1),
            UserId = userId
        };

        _context.TodoTasks.Add(task);
        await _context.SaveChangesAsync();

        return task.Id;
    }

    public async Task<List<TaskDto>> GetUserTasksAsync(
        Guid userId,
    TodoTaskStatus? status = null,
    string? search = null,
    string? sortBy = "dueDate",
    string? sortDir = "asc"
    )
    {

        var query = _context.TodoTasks
            .Where(t => t.UserId == userId);

        if (status != null)
            query = query.Where(todoTask => todoTask.TodoTaskStatus == status);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(todoTask => todoTask.Title.ToLower().Contains(search.ToLower()) || todoTask.Description.ToLower().Contains(search.ToLower()));

        query = (sortBy?.ToLower(), sortDir?.ToLower()) switch
        {
            ("title", "desc") => query.OrderByDescending(todoTask => todoTask.Title),
            ("title", _) => query.OrderBy(todoTask => todoTask.Title),
            ("duedate", "desc") => query.OrderByDescending(todoTask => todoTask.DueDate),
            _ => query.OrderBy(todoTask => todoTask.DueDate),
        };

        return await query.Select(todoTask => new TaskDto
        {
            Id = todoTask.Id,
            Title = todoTask.Title,
            Description = todoTask.Description,
            TodoTaskStatus = todoTask.TodoTaskStatus,
            DueDate = todoTask.DueDate
        }).ToListAsync();

    }

    public async Task<object> GetTaskStatsAsync(Guid userId)
    {
        return new
        {
            Total = await _context.TodoTasks.CountAsync(todoTask => todoTask.UserId == userId),
            Completed = await _context.TodoTasks.CountAsync(todoTask =>
                todoTask.UserId == userId && todoTask.TodoTaskStatus == TodoTaskStatus.Completed),
            InProgress = await _context.TodoTasks.CountAsync(todoTask =>
                todoTask.UserId == userId && todoTask.TodoTaskStatus == TodoTaskStatus.InProgress),
            Pending = await _context.TodoTasks.CountAsync(todoTask =>
                todoTask.UserId == userId && todoTask.TodoTaskStatus == TodoTaskStatus.Pending)
        };
    }


    public async Task<bool> UpdateAsync(Guid id, UpdateTaskDto dto, Guid userId)
    {
        var task = await _context.TodoTasks.FirstOrDefaultAsync(todoTask => todoTask.Id == id && todoTask.UserId == userId);
        if (task == null) return false;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.TodoTaskStatus = dto.TodoTaskStatus;
        task.DueDate = dto.DueDate ?? task.DueDate;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var task = await _context.TodoTasks.FirstOrDefaultAsync(todoTask => todoTask.Id == id && todoTask.UserId == userId);
        if (task == null) return false;

        _context.TodoTasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}
