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

    public async Task<List<TaskDto>> GetUserTasksAsync(Guid userId, TodoTaskStatus? status = null)
    {
        var query = _context.TodoTasks
            .Where(t => t.UserId == userId);

        if (status != null)
            query = query.Where(todoTask => todoTask.TodoTaskStatus == status);

        return await query
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                TodoTaskStatus = t.TodoTaskStatus,
                DueDate = t.DueDate
            })
            .ToListAsync();
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
