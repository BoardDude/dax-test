using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class Task
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public TaskPriority Priority { get; set; }
        
        public TaskStatus Status { get; set; }
        
        public TaskCategory Category { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public int? AssignedUserId { get; set; }
        
        public int CreatedByUserId { get; set; }
        
        public List<TaskComment> Comments { get; set; } = new();
        
        public List<TaskAttachment> Attachments { get; set; } = new();
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TaskStatus
    {
        Todo,
        InProgress,
        Review,
        Done,
        Cancelled
    }

    public enum TaskCategory
    {
        Development,
        Design,
        Testing,
        Documentation,
        Bug,
        Feature,
        Maintenance,
        Other
    }

    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class TaskAttachment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int UploadedByUserId { get; set; }
    }

    public class CreateTaskRequest
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        public TaskCategory Category { get; set; } = TaskCategory.Other;
        
        public DateTime? DueDate { get; set; }
        
        public int? AssignedUserId { get; set; }
    }

    public class UpdateTaskRequest
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public TaskPriority? Priority { get; set; }
        
        public TaskStatus? Status { get; set; }
        
        public TaskCategory? Category { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public int? AssignedUserId { get; set; }
    }
} 