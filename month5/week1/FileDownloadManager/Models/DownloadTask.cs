namespace FileDownloadManager.Models
{
    public class DownloadTask
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string LocalPath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public long DownloadedBytes { get; set; }
        public DownloadStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; } = 3;
        public double ProgressPercentage => FileSize > 0 ? (double)DownloadedBytes / FileSize * 100 : 0;
    }

    public enum DownloadStatus
    {
        Pending,
        Downloading,
        Completed,
        Failed,
        Cancelled
    }

    public class DownloadLog
    {
        public int Id { get; set; }
        public int DownloadTaskId { get; set; }
        public string Message { get; set; } = string.Empty;
        public LogLevel Level { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? ExceptionDetails { get; set; }
    }

    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }
} 