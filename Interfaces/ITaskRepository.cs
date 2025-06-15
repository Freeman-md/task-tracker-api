using api.DTOs;
using api.enums;
using api.Models;

namespace api.Interfaces;

public interface ITaskRepository
{
       Task<Guid> CreateAsync(CreateTaskDto dto, Guid userId);
    Task<List<TaskDto>> GetUserTasksAsync(Guid userId, TodoTaskStatus? status = null);
    Task<bool> UpdateAsync(Guid id, UpdateTaskDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
