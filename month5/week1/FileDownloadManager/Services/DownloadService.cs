using FileDownloadManager.Models;
using Microsoft.Extensions.Logging;

namespace FileDownloadManager.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IFileValidator _fileValidator;
        private readonly IDownloadProgressReporter _progressReporter;
        private readonly IDownloadQueue _downloadQueue;
        private readonly ILogger<DownloadService> _logger;
        private readonly List<DownloadTask> _downloadTasks;
        private int _nextTaskId = 1;

        public DownloadService(
            IFileValidator fileValidator,
            IDownloadProgressReporter progressReporter,
            IDownloadQueue downloadQueue,
            ILogger<DownloadService> logger)
        {
            _fileValidator = fileValidator;
            _progressReporter = progressReporter;
            _downloadQueue = downloadQueue;
            _logger = logger;
            _downloadTasks = new List<DownloadTask>();
        }

        public async Task<DownloadTask> AddDownloadTaskAsync(string url, string fileName)
        {
            _logger.LogInformation("Adding download task for URL: {Url}", url);

            // Validate URL
            if (!await _fileValidator.ValidateUrlAsync(url))
            {
                throw new ArgumentException("Invalid URL provided.");
            }

            // Get file size
            var fileSize = await _fileValidator.GetFileSizeAsync(url);

            // Validate file size (max 100MB)
            if (!await _fileValidator.ValidateFileSizeAsync(url, 100 * 1024 * 1024))
            {
                throw new ArgumentException("File size exceeds maximum allowed size.");
            }

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".txt", ".doc", ".docx", ".jpg", ".png", ".zip" };
            if (!await _fileValidator.ValidateFileTypeAsync(url, allowedExtensions))
            {
                throw new ArgumentException("File type not allowed.");
            }

            var task = new DownloadTask
            {
                Id = _nextTaskId++,
                Url = url,
                FileName = fileName,
                LocalPath = Path.Combine("Downloads", fileName),
                FileSize = fileSize,
                Status = DownloadStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _downloadTasks.Add(task);
            await _downloadQueue.EnqueueAsync(task);

            _logger.LogInformation("Download task {TaskId} added successfully", task.Id);
            return task;
        }

        public Task<DownloadTask?> GetDownloadTaskAsync(int id)
        {
            var task = _downloadTasks.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(task);
        }

        public Task<IEnumerable<DownloadTask>> GetAllDownloadTasksAsync()
        {
            return Task.FromResult(_downloadTasks.AsEnumerable());
        }

        public Task<IEnumerable<DownloadTask>> GetDownloadTasksByStatusAsync(DownloadStatus status)
        {
            var tasks = _downloadTasks.Where(t => t.Status == status);
            return Task.FromResult(tasks);
        }

        public async Task StartDownloadAsync(int id)
        {
            var task = await GetDownloadTaskAsync(id);
            if (task == null)
            {
                throw new ArgumentException("Download task not found.");
            }

            if (task.Status != DownloadStatus.Pending)
            {
                throw new InvalidOperationException("Task is not in pending status.");
            }

            task.Status = DownloadStatus.Downloading;
            task.StartedAt = DateTime.UtcNow;
            _progressReporter.ReportStatus(id, DownloadStatus.Downloading);

            _logger.LogInformation("Starting download for task {TaskId}", id);

            // Simulate download process
            await SimulateDownloadAsync(task);
        }

        public async Task CancelDownloadAsync(int id)
        {
            var task = await GetDownloadTaskAsync(id);
            if (task == null)
            {
                throw new ArgumentException("Download task not found.");
            }

            if (task.Status == DownloadStatus.Downloading)
            {
                task.Status = DownloadStatus.Cancelled;
                _progressReporter.ReportStatus(id, DownloadStatus.Cancelled);
                _logger.LogInformation("Download cancelled for task {TaskId}", id);
            }
        }

        public async Task DeleteDownloadTaskAsync(int id)
        {
            var task = await GetDownloadTaskAsync(id);
            if (task == null)
            {
                throw new ArgumentException("Download task not found.");
            }

            _downloadTasks.Remove(task);
            _logger.LogInformation("Download task {TaskId} deleted", id);
        }

        public Task<IEnumerable<DownloadTask>> GetFailedDownloadsAsync()
        {
            var failedTasks = _downloadTasks.Where(t => t.Status == DownloadStatus.Failed);
            return Task.FromResult(failedTasks);
        }

        public async Task RetryFailedDownloadAsync(int id)
        {
            var task = await GetDownloadTaskAsync(id);
            if (task == null)
            {
                throw new ArgumentException("Download task not found.");
            }

            if (task.Status != DownloadStatus.Failed)
            {
                throw new InvalidOperationException("Task is not in failed status.");
            }

            if (task.RetryCount >= task.MaxRetries)
            {
                throw new InvalidOperationException("Maximum retry attempts exceeded.");
            }

            task.RetryCount++;
            task.Status = DownloadStatus.Pending;
            task.ErrorMessage = null;
            await _downloadQueue.EnqueueAsync(task);

            _logger.LogInformation("Retrying download for task {TaskId} (attempt {RetryCount})", id, task.RetryCount);
        }

        public Task<Dictionary<string, object>> GetDownloadStatisticsAsync()
        {
            var stats = new Dictionary<string, object>
            {
                ["TotalTasks"] = _downloadTasks.Count,
                ["PendingTasks"] = _downloadTasks.Count(t => t.Status == DownloadStatus.Pending),
                ["DownloadingTasks"] = _downloadTasks.Count(t => t.Status == DownloadStatus.Downloading),
                ["CompletedTasks"] = _downloadTasks.Count(t => t.Status == DownloadStatus.Completed),
                ["FailedTasks"] = _downloadTasks.Count(t => t.Status == DownloadStatus.Failed),
                ["CancelledTasks"] = _downloadTasks.Count(t => t.Status == DownloadStatus.Cancelled),
                ["TotalDownloadedBytes"] = _downloadTasks.Where(t => t.Status == DownloadStatus.Completed).Sum(t => t.DownloadedBytes),
                ["AverageDownloadTime"] = _downloadTasks.Where(t => t.CompletedAt.HasValue).Average(t => (t.CompletedAt!.Value - t.StartedAt!.Value).TotalSeconds)
            };

            return Task.FromResult(stats);
        }

        private async Task SimulateDownloadAsync(DownloadTask task)
        {
            try
            {
                var random = new Random();
                var chunkSize = 1024 * 1024; // 1MB chunks
                var totalChunks = (int)Math.Ceiling((double)task.FileSize / chunkSize);

                for (int i = 0; i < totalChunks; i++)
                {
                    if (task.Status == DownloadStatus.Cancelled)
                    {
                        break;
                    }

                    // Simulate network delay
                    await Task.Delay(random.Next(100, 500));

                    var bytesToDownload = Math.Min(chunkSize, task.FileSize - task.DownloadedBytes);
                    task.DownloadedBytes += bytesToDownload;

                    _progressReporter.ReportProgress(task.Id, task.DownloadedBytes, task.FileSize);

                    // Simulate occasional failures
                    if (random.Next(1, 100) <= 5) // 5% chance of failure
                    {
                        throw new Exception("Simulated network error");
                    }
                }

                if (task.Status != DownloadStatus.Cancelled)
                {
                    task.Status = DownloadStatus.Completed;
                    task.CompletedAt = DateTime.UtcNow;
                    _progressReporter.ReportStatus(task.Id, DownloadStatus.Completed);
                    _logger.LogInformation("Download completed for task {TaskId}", task.Id);
                }
            }
            catch (Exception ex)
            {
                task.Status = DownloadStatus.Failed;
                task.ErrorMessage = ex.Message;
                _progressReporter.ReportError(task.Id, ex.Message);
                _logger.LogError(ex, "Download failed for task {TaskId}", task.Id);
            }
        }
    }
} 