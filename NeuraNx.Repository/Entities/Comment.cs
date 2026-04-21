using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository.Entities
{
    public class Comment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
        
        // Foreign Keys
        [Required]
        public Guid TaskItemId { get; set; }
        
        [Required]
        public Guid CreatedById { get; set; }
        
        // Navigation properties
        public virtual TaskItem TaskItem { get; set; } = null!;
        public virtual Employee CreatedBy { get; set; } = null!;
    }
}