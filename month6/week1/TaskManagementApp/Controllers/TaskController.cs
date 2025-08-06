using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models;
using TaskManagementApp.Services;

namespace TaskManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskNotificationService _notificationService;
        private readonly ITaskValidationService _validationService;
        private readonly ITaskSearchService _searchService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService taskService,
            ITaskNotificationService notificationService,
            ITaskValidationService validationService,
            ITaskSearchService searchService,
            ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _notificationService = notificationService;
            _validationService = validationService;
            _searchService = searchService;
            _logger = logger;
        }

        // GET: api/task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var tasks = await _taskService.GetTasksWithPaginationAsync(page, pageSize);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        // GET: api/task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the task");
            }
        }

        // GET: api/task/status/todo
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksByStatus(TaskStatus status)
        {
            try
            {
                var tasks = await _taskService.GetTasksByStatusAsync(status);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks by status {Status}", status);
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        // GET: api/task/priority/high
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksByPriority(TaskPriority priority)
        {
            try
            {
                var tasks = await _taskService.GetTasksByPriorityAsync(priority);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks by priority {Priority}", priority);
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        // GET: api/task/category/development
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksByCategory(TaskCategory category)
        {
            try
            {
                var tasks = await _taskService.GetTasksByCategoryAsync(category);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks by category {Category}", category);
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        // GET: api/task/assignee/5
        [HttpGet("assignee/{userId}")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksByAssignee(int userId)
        {
            try
            {
                var tasks = await _taskService.GetTasksByAssigneeAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks by assignee {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        // GET: api/task/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Task>>> GetOverdueTasks()
        {
            try
            {
                var tasks = await _taskService.GetOverdueTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving overdue tasks");
                return StatusCode(500, "An error occurred while retrieving overdue tasks");
            }
        }

        // GET: api/task/due-today
        [HttpGet("due-today")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasksDueToday()
        {
            try
            {
                var tasks = await _taskService.GetTasksDueTodayAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks due today");
                return StatusCode(500, "An error occurred while retrieving tasks due today");
            }
        }

        // GET: api/task/search?term=bug
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Task>>> SearchTasks([FromQuery] string term)
        {
            try
            {
                var tasks = await _taskService.SearchTasksAsync(term);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching tasks with term {Term}", term);
                return StatusCode(500, "An error occurred while searching tasks");
            }
        }

        // POST: api/task
        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask(CreateTaskRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // In a real application, you would get the user ID from the authentication context
                var createdByUserId = 1; // Simulated user ID

                var task = await _taskService.CreateTaskAsync(request, createdByUserId);

                // Send notification if task is assigned
                if (request.AssignedUserId.HasValue)
                {
                    await _notificationService.SendTaskAssignedNotificationAsync(task.Id, request.AssignedUserId.Value);
                }

                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "An error occurred while creating the task");
            }
        }

        // PUT: api/task/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get current task to check status changes
                var currentTask = await _taskService.GetTaskByIdAsync(id);
                if (currentTask == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                var oldStatus = currentTask.Status;
                var task = await _taskService.UpdateTaskAsync(id, request);

                // Send notification if status changed
                if (request.Status.HasValue && request.Status.Value != oldStatus)
                {
                    await _notificationService.SendTaskStatusChangedNotificationAsync(id, oldStatus, request.Status.Value);
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the task");
            }
        }

        // DELETE: api/task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the task");
            }
        }

        // POST: api/task/5/comments
        [HttpPost("{id}/comments")]
        public async Task<ActionResult<TaskComment>> AddComment(int id, [FromBody] string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return BadRequest("Comment content is required");
                }

                // In a real application, you would get the user ID from the authentication context
                var userId = 1; // Simulated user ID

                var comment = await _taskService.AddCommentAsync(id, content, userId);
                return CreatedAtAction(nameof(GetTaskComments), new { id }, comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to task {Id}", id);
                return StatusCode(500, "An error occurred while adding the comment");
            }
        }

        // GET: api/task/5/comments
        [HttpGet("{id}/comments")]
        public async Task<ActionResult<IEnumerable<TaskComment>>> GetTaskComments(int id)
        {
            try
            {
                var comments = await _taskService.GetTaskCommentsAsync(id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving comments for task {Id}", id);
                return StatusCode(500, "An error occurred while retrieving comments");
            }
        }

        // POST: api/task/5/attachments
        [HttpPost("{id}/attachments")]
        public async Task<ActionResult<TaskAttachment>> AddAttachment(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File is required");
                }

                // In a real application, you would save the file to a storage service
                var fileName = file.FileName;
                var filePath = Path.Combine("uploads", fileName);
                var fileSize = file.Length;
                var contentType = file.ContentType;

                // In a real application, you would get the user ID from the authentication context
                var userId = 1; // Simulated user ID

                var attachment = await _taskService.AddAttachmentAsync(id, fileName, filePath, fileSize, contentType, userId);
                return CreatedAtAction(nameof(GetTaskAttachments), new { id }, attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding attachment to task {Id}", id);
                return StatusCode(500, "An error occurred while adding the attachment");
            }
        }

        // GET: api/task/5/attachments
        [HttpGet("{id}/attachments")]
        public async Task<ActionResult<IEnumerable<TaskAttachment>>> GetTaskAttachments(int id)
        {
            try
            {
                var attachments = await _taskService.GetTaskAttachmentsAsync(id);
                return Ok(attachments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving attachments for task {Id}", id);
                return StatusCode(500, "An error occurred while retrieving attachments");
            }
        }

        // GET: api/task/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<Dictionary<string, object>>> GetTaskStatistics()
        {
            try
            {
                var statistics = await _taskService.GetTaskStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task statistics");
                return StatusCode(500, "An error occurred while retrieving statistics");
            }
        }

        // GET: api/task/advanced-search
        [HttpGet("advanced-search")]
        public async Task<ActionResult<IEnumerable<Task>>> AdvancedSearch([FromQuery] TaskSearchCriteria criteria)
        {
            try
            {
                var tasks = await _searchService.AdvancedSearchAsync(criteria);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing advanced search");
                return StatusCode(500, "An error occurred while performing advanced search");
            }
        }
    }
} 