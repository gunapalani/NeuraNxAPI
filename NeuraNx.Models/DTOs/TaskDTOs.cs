using NeuraNx.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsTaskStatus = NeuraNx.Models.Enums.TaskStatus;
using ModelsTaskPriority = NeuraNx.Models.Enums.TaskPriority;

namespace NeuraNx.Models.DTOs
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ModelsTaskStatus Status { get; set; }
        public ModelsTaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid BoardId { get; set; }
        public string BoardName { get; set; } = string.Empty;
        public Guid? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public byte[]? RowVersion { get; set; }
        public int CommentCount { get; set; }
    }

    public class CreateTaskDto
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public ModelsTaskPriority Priority { get; set; } = ModelsTaskPriority.Medium;
        
        public DateTime? DueDate { get; set; }
        
        [Required]
        public Guid BoardId { get; set; }
        
        public Guid? AssignedToId { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public ModelsTaskStatus Status { get; set; }
        
        public ModelsTaskPriority Priority { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public Guid? AssignedToId { get; set; }
        
        [Required]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }

    public class UpdateTaskStatusDto
    {
        [Required]
        public ModelsTaskStatus Status { get; set; }
        
        [Required]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}