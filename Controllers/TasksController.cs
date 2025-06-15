using api.DTOs;
using api.Models;
using api.Repositories;
using api.Utils;
using api.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Interfaces;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public TasksController(ITaskRepository repository, ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", ModelState.ToApiErrors()));
        }

        var taskId = await _repository.CreateAsync(dto, _currentUser.GetUserId()!.Value);
        return Ok(ApiResponse<object>.SuccessResponse(new { taskId }, "Task created"));
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] TodoTaskStatus? status = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = "dueDate",
        [FromQuery] string? sortDir = "asc"
    )
    {
        var tasks = await _repository.GetUserTasksAsync(
            _currentUser.GetUserId()!.Value,
            status,
            search,
            sortBy,
            sortDir
        );

        return Ok(ApiResponse<List<TaskDto>>.SuccessResponse(tasks));
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _repository.GetTaskStatsAsync(_currentUser.GetUserId()!.Value);
        return Ok(ApiResponse<object>.SuccessResponse(stats, "Task stats fetched"));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", ModelState.ToApiErrors()));
        }

        var success = await _repository.UpdateAsync(id, dto, _currentUser.GetUserId()!.Value);
        if (!success) return NotFound(ApiResponse<object>.ErrorResponse("Task not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!, "Task updated"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _repository.DeleteAsync(id, _currentUser.GetUserId()!.Value);
        if (!success) return NotFound(ApiResponse<object>.ErrorResponse("Task not found"));

        return Ok(ApiResponse<object>.SuccessResponse(null!, "Task deleted"));
    }
}
