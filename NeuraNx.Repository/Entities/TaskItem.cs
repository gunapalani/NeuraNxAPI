using NeuraNx.Repository.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTaskStatus = NeuraNx.Repository.Enums.TaskStatus;
using RepositoryTaskPriority = NeuraNx.Repository.Enums.TaskPriority;

namespace NeuraNx.Repository.Entities
{
    public class TaskItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string? Description { get; set; }
        
        [Required]
        public RepositoryTaskStatus Status { get; set; } = RepositoryTaskStatus.Todo;
        
        [Required]
        public RepositoryTaskPriority Priority { get; set; } = RepositoryTaskPriority.Medium;
        
        public DateTime? DueDate { get; set; }
        
        // Foreign Keys
        [Required]
        public Guid BoardId { get; set; }
        
        public Guid? AssignedToId { get; set; }
        
        // Concurrency control
        [Timestamp]
        public byte[]? RowVersion { get; set; }
        
        // Navigation properties
        public virtual Board Board { get; set; } = null!;
        public virtual Employee? AssignedTo { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}