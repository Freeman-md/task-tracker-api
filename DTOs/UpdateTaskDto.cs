using System.ComponentModel.DataAnnotations;
using api.enums;

namespace api.DTOs;

public class UpdateTaskDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [EnumDataType(typeof(TodoTaskStatus))]
    public TodoTaskStatus TodoTaskStatus { get; set; } = TodoTaskStatus.Pending;

    public DateTime? DueDate { get; set; }
}
