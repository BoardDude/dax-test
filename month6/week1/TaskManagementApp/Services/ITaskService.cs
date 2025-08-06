using TaskManagementApp.Models;

namespace TaskManagementApp.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Task>> GetAllTasksAsync();
        Task<Task?> GetTaskByIdAsync(int id);
        Task<IEnumerable<Task>> GetTasksByStatusAsync(TaskStatus status);
        Task<IEnumerable<Task>> GetTasksByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<Task>> GetTasksByCategoryAsync(TaskCategory category);
        Task<IEnumerable<Task>> GetTasksByAssigneeAsync(int userId);
        Task<IEnumerable<Task>> GetTasksByCreatorAsync(int userId);
        Task<IEnumerable<Task>> GetOverdueTasksAsync();
        Task<IEnumerable<Task>> GetTasksDueTodayAsync();
        Task<IEnumerable<Task>> SearchTasksAsync(string searchTerm);
        Task<Task> CreateTaskAsync(CreateTaskRequest request, int createdByUserId);
        Task<Task> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task DeleteTaskAsync(int id);
        Task<TaskComment> AddCommentAsync(int taskId, string content, int userId);
        Task<IEnumerable<TaskComment>> GetTaskCommentsAsync(int taskId);
        Task<TaskAttachment> AddAttachmentAsync(int taskId, string fileName, string filePath, long fileSize, string contentType, int userId);
        Task<IEnumerable<TaskAttachment>> GetTaskAttachmentsAsync(int taskId);
        Task<Dictionary<string, object>> GetTaskStatisticsAsync();
        Task<IEnumerable<Task>> GetTasksWithPaginationAsync(int page, int pageSize);
    }

    public interface ITaskNotificationService
    {
        Task SendTaskAssignedNotificationAsync(int taskId, int assignedUserId);
        Task SendTaskStatusChangedNotificationAsync(int taskId, TaskStatus oldStatus, TaskStatus newStatus);
        Task SendTaskDueDateReminderAsync(int taskId);
        Task SendTaskOverdueNotificationAsync(int taskId);
    }

    public interface ITaskValidationService
    {
        Task<bool> ValidateTaskAccessAsync(int taskId, int userId);
        Task<bool> ValidateTaskAssignmentAsync(int taskId, int assignedUserId);
        Task<bool> ValidateTaskStatusTransitionAsync(TaskStatus currentStatus, TaskStatus newStatus);
        Task<bool> ValidateTaskDueDateAsync(DateTime? dueDate);
    }

    public interface ITaskSearchService
    {
        Task<IEnumerable<Task>> SearchByTitleAsync(string title);
        Task<IEnumerable<Task>> SearchByDescriptionAsync(string description);
        Task<IEnumerable<Task>> SearchByAssigneeAsync(string assigneeName);
        Task<IEnumerable<Task>> SearchByCreatorAsync(string creatorName);
        Task<IEnumerable<Task>> AdvancedSearchAsync(TaskSearchCriteria criteria);
    }

    public class TaskSearchCriteria
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public TaskCategory? Category { get; set; }
        public int? AssignedUserId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
    }
} 