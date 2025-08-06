using FileDownloadManager.Models;

namespace FileDownloadManager.Services
{
    public interface IDownloadService
    {
        Task<DownloadTask> AddDownloadTaskAsync(string url, string fileName);
        Task<DownloadTask?> GetDownloadTaskAsync(int id);
        Task<IEnumerable<DownloadTask>> GetAllDownloadTasksAsync();
        Task<IEnumerable<DownloadTask>> GetDownloadTasksByStatusAsync(DownloadStatus status);
        Task StartDownloadAsync(int id);
        Task CancelDownloadAsync(int id);
        Task DeleteDownloadTaskAsync(int id);
        Task<IEnumerable<DownloadTask>> GetFailedDownloadsAsync();
        Task RetryFailedDownloadAsync(int id);
        Task<Dictionary<string, object>> GetDownloadStatisticsAsync();
    }

    public interface IFileValidator
    {
        Task<bool> ValidateUrlAsync(string url);
        Task<bool> ValidateFileSizeAsync(string url, long maxSizeBytes);
        Task<bool> ValidateFileTypeAsync(string url, string[] allowedExtensions);
        Task<string> GetFileNameFromUrlAsync(string url);
        Task<long> GetFileSizeAsync(string url);
    }

    public interface IDownloadProgressReporter
    {
        void ReportProgress(int taskId, long downloadedBytes, long totalBytes);
        void ReportStatus(int taskId, DownloadStatus status);
        void ReportError(int taskId, string errorMessage);
    }

    public interface IDownloadQueue
    {
        Task EnqueueAsync(DownloadTask task);
        Task<DownloadTask?> DequeueAsync();
        Task<int> GetQueueLengthAsync();
        Task<IEnumerable<DownloadTask>> GetQueuedTasksAsync();
    }
} 