using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FileDownloadManager.Services;
using FileDownloadManager.Models;

namespace FileDownloadManager
{
    public class FileValidator : IFileValidator
    {
        private readonly ILogger<FileValidator> _logger;

        public FileValidator(ILogger<FileValidator> logger)
        {
            _logger = logger;
        }

        public Task<bool> ValidateUrlAsync(string url)
        {
            var isValid = Uri.TryCreate(url, UriKind.Absolute, out _);
            _logger.LogInformation("URL validation result: {Url} - {IsValid}", url, isValid);
            return Task.FromResult(isValid);
        }

        public Task<bool> ValidateFileSizeAsync(string url, long maxSizeBytes)
        {
            // Simulate file size check
            var random = new Random();
            var fileSize = random.Next(1024, 50 * 1024 * 1024); // 1KB to 50MB
            var isValid = fileSize <= maxSizeBytes;
            _logger.LogInformation("File size validation: {FileSize} bytes, Max: {MaxSize}, Valid: {IsValid}", fileSize, maxSizeBytes, isValid);
            return Task.FromResult(isValid);
        }

        public Task<bool> ValidateFileTypeAsync(string url, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(url).ToLower();
            var isValid = allowedExtensions.Contains(extension);
            _logger.LogInformation("File type validation: {Extension}, Valid: {IsValid}", extension, isValid);
            return Task.FromResult(isValid);
        }

        public Task<string> GetFileNameFromUrlAsync(string url)
        {
            var fileName = Path.GetFileName(url);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"download_{DateTime.Now:yyyyMMdd_HHmmss}";
            }
            return Task.FromResult(fileName);
        }

        public Task<long> GetFileSizeAsync(string url)
        {
            // Simulate getting file size
            var random = new Random();
            var fileSize = random.Next(1024, 10 * 1024 * 1024); // 1KB to 10MB
            return Task.FromResult((long)fileSize);
        }
    }

    public class DownloadProgressReporter : IDownloadProgressReporter
    {
        private readonly ILogger<DownloadProgressReporter> _logger;

        public DownloadProgressReporter(ILogger<DownloadProgressReporter> logger)
        {
            _logger = logger;
        }

        public void ReportProgress(int taskId, long downloadedBytes, long totalBytes)
        {
            var percentage = totalBytes > 0 ? (double)downloadedBytes / totalBytes * 100 : 0;
            _logger.LogInformation("Download progress for task {TaskId}: {DownloadedBytes}/{TotalBytes} bytes ({Percentage:F1}%)", 
                taskId, downloadedBytes, totalBytes, percentage);
        }

        public void ReportStatus(int taskId, DownloadStatus status)
        {
            _logger.LogInformation("Download status for task {TaskId}: {Status}", taskId, status);
        }

        public void ReportError(int taskId, string errorMessage)
        {
            _logger.LogError("Download error for task {TaskId}: {ErrorMessage}", taskId, errorMessage);
        }
    }

    public class DownloadQueue : IDownloadQueue
    {
        private readonly Queue<DownloadTask> _queue = new();
        private readonly object _lock = new();
        private readonly ILogger<DownloadQueue> _logger;

        public DownloadQueue(ILogger<DownloadQueue> logger)
        {
            _logger = logger;
        }

        public Task EnqueueAsync(DownloadTask task)
        {
            lock (_lock)
            {
                _queue.Enqueue(task);
                _logger.LogInformation("Task {TaskId} enqueued. Queue length: {QueueLength}", task.Id, _queue.Count);
            }
            return Task.CompletedTask;
        }

        public Task<DownloadTask?> DequeueAsync()
        {
            lock (_lock)
            {
                if (_queue.TryDequeue(out var task))
                {
                    _logger.LogInformation("Task {TaskId} dequeued. Queue length: {QueueLength}", task.Id, _queue.Count);
                    return Task.FromResult<DownloadTask?>(task);
                }
                return Task.FromResult<DownloadTask?>(null);
            }
        }

        public Task<int> GetQueueLengthAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_queue.Count);
            }
        }

        public Task<IEnumerable<DownloadTask>> GetQueuedTasksAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_queue.AsEnumerable());
            }
        }
    }

    public class DownloadManagerApp
    {
        private readonly IDownloadService _downloadService;
        private readonly ILogger<DownloadManagerApp> _logger;

        public DownloadManagerApp(IDownloadService downloadService, ILogger<DownloadManagerApp> logger)
        {
            _downloadService = downloadService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("=== File Download Manager with DI ===");
            Console.WriteLine("Commands: add, list, start, cancel, delete, retry, failed, stats, quit");

            while (true)
            {
                Console.Write("\nEnter command: ");
                string command = Console.ReadLine()?.ToLower() ?? string.Empty;

                try
                {
                    switch (command)
                    {
                        case "add":
                            await AddDownloadAsync();
                            break;
                        case "list":
                            await ListDownloadsAsync();
                            break;
                        case "start":
                            await StartDownloadAsync();
                            break;
                        case "cancel":
                            await CancelDownloadAsync();
                            break;
                        case "delete":
                            await DeleteDownloadAsync();
                            break;
                        case "retry":
                            await RetryDownloadAsync();
                            break;
                        case "failed":
                            await ListFailedDownloadsAsync();
                            break;
                        case "stats":
                            await ShowStatisticsAsync();
                            break;
                        case "quit":
                            return;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    _logger.LogError(ex, "Error executing command: {Command}", command);
                }
            }
        }

        private async Task AddDownloadAsync()
        {
            Console.Write("Enter URL: ");
            string url = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Enter filename: ");
            string fileName = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("URL and filename are required.");
                return;
            }

            var task = await _downloadService.AddDownloadTaskAsync(url, fileName);
            Console.WriteLine($"Download task {task.Id} added successfully.");
        }

        private async Task ListDownloadsAsync()
        {
            var tasks = await _downloadService.GetAllDownloadTasksAsync();
            Console.WriteLine($"\n=== All Downloads ({tasks.Count()}) ===");
            Console.WriteLine($"{"ID",-3} {"Status",-12} {"Progress",-10} {"File",-20} {"Size",-10}");
            Console.WriteLine(new string('-', 70));

            foreach (var task in tasks)
            {
                Console.WriteLine($"{task.Id,-3} {task.Status,-12} {task.ProgressPercentage:F1}% {task.FileName,-20} {task.FileSize / 1024 / 1024:F1}MB");
            }
        }

        private async Task StartDownloadAsync()
        {
            Console.Write("Enter task ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            await _downloadService.StartDownloadAsync(id);
            Console.WriteLine($"Download started for task {id}.");
        }

        private async Task CancelDownloadAsync()
        {
            Console.Write("Enter task ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            await _downloadService.CancelDownloadAsync(id);
            Console.WriteLine($"Download cancelled for task {id}.");
        }

        private async Task DeleteDownloadAsync()
        {
            Console.Write("Enter task ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            await _downloadService.DeleteDownloadTaskAsync(id);
            Console.WriteLine($"Download task {id} deleted.");
        }

        private async Task RetryDownloadAsync()
        {
            Console.Write("Enter task ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            await _downloadService.RetryFailedDownloadAsync(id);
            Console.WriteLine($"Retrying download for task {id}.");
        }

        private async Task ListFailedDownloadsAsync()
        {
            var failedTasks = await _downloadService.GetFailedDownloadsAsync();
            Console.WriteLine($"\n=== Failed Downloads ({failedTasks.Count()}) ===");
            
            foreach (var task in failedTasks)
            {
                Console.WriteLine($"ID: {task.Id}, File: {task.FileName}, Error: {task.ErrorMessage}");
            }
        }

        private async Task ShowStatisticsAsync()
        {
            var stats = await _downloadService.GetDownloadStatisticsAsync();
            Console.WriteLine($"\n=== Download Statistics ===");
            
            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.Key}: {stat.Value}");
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up dependency injection with advanced patterns
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Register services with different lifetimes
                    services.AddSingleton<IFileValidator, FileValidator>();
                    services.AddScoped<IDownloadProgressReporter, DownloadProgressReporter>();
                    services.AddSingleton<IDownloadQueue, DownloadQueue>();
                    services.AddScoped<IDownloadService, DownloadService>();
                    services.AddScoped<DownloadManagerApp>();

                    // Add logging
                    services.AddLogging(builder =>
                    {
                        builder.AddConsole();
                        builder.SetMinimumLevel(LogLevel.Information);
                    });
                })
                .Build();

            // Get the service and run the application
            var app = host.Services.GetRequiredService<DownloadManagerApp>();
            await app.RunAsync();
        }
    }
} 