using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AsyncFileProcessor
{
    public class FileProcessor
    {
        private readonly string _logFilePath;
        
        public FileProcessor(string logFilePath = "processing.log")
        {
            _logFilePath = logFilePath;
        }
        
        // Process multiple files asynchronously
        public async Task<List<ProcessingResult>> ProcessFilesAsync(List<string> filePaths)
        {
            var tasks = filePaths.Select(filePath => ProcessFileAsync(filePath));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }
        
        // Process a single file asynchronously
        public async Task<ProcessingResult> ProcessFileAsync(string filePath)
        {
            var result = new ProcessingResult
            {
                FilePath = filePath,
                StartTime = DateTime.Now
            };
            
            try
            {
                await LogAsync($"Starting to process file: {filePath}");
                
                // Check if file exists
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }
                
                // Read file content asynchronously
                string content = await File.ReadAllTextAsync(filePath);
                result.FileSize = content.Length;
                
                // Simulate some processing time
                await Task.Delay(1000); // Simulate work
                
                // Process the content
                result.LineCount = content.Split('\n').Length;
                result.WordCount = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                result.CharacterCount = content.Length;
                
                // Calculate some statistics
                result.Statistics = await CalculateStatisticsAsync(content);
                
                result.IsSuccess = true;
                result.EndTime = DateTime.Now;
                result.Duration = result.EndTime - result.StartTime;
                
                await LogAsync($"Successfully processed file: {filePath} in {result.Duration.TotalMilliseconds:F2}ms");
            }
            catch (FileNotFoundException ex)
            {
                await HandleErrorAsync(result, ex, "File not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleErrorAsync(result, ex, "Access denied");
            }
            catch (IOException ex)
            {
                await HandleErrorAsync(result, ex, "IO error");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(result, ex, "Unexpected error");
            }
            
            return result;
        }
        
        // Download file from URL asynchronously
        public async Task<ProcessingResult> DownloadAndProcessAsync(string url, string localPath)
        {
            var result = new ProcessingResult
            {
                FilePath = localPath,
                StartTime = DateTime.Now
            };
            
            try
            {
                await LogAsync($"Starting download from: {url}");
                
                using var httpClient = new HttpClient();
                var content = await httpClient.GetStringAsync(url);
                
                // Save to local file
                await File.WriteAllTextAsync(localPath, content);
                
                // Process the downloaded file
                result = await ProcessFileAsync(localPath);
                result.IsDownloaded = true;
                
                await LogAsync($"Successfully downloaded and processed: {url}");
            }
            catch (HttpRequestException ex)
            {
                await HandleErrorAsync(result, ex, "Network error");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(result, ex, "Download error");
            }
            
            return result;
        }
        
        // Calculate file statistics asynchronously
        private async Task<FileStatistics> CalculateStatisticsAsync(string content)
        {
            return await Task.Run(() =>
            {
                var lines = content.Split('\n');
                var words = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                
                return new FileStatistics
                {
                    AverageLineLength = lines.Length > 0 ? lines.Average(l => l.Length) : 0,
                    LongestLine = lines.Length > 0 ? lines.Max(l => l.Length) : 0,
                    ShortestLine = lines.Length > 0 ? lines.Min(l => l.Length) : 0,
                    UniqueWords = words.Distinct().Count(),
                    MostCommonWords = words
                        .GroupBy(w => w.ToLower())
                        .OrderByDescending(g => g.Count())
                        .Take(5)
                        .Select(g => new WordFrequency { Word = g.Key, Count = g.Count() })
                        .ToList()
                };
            });
        }
        
        // Handle errors asynchronously
        private async Task HandleErrorAsync(ProcessingResult result, Exception ex, string errorType)
        {
            result.IsSuccess = false;
            result.ErrorMessage = $"{errorType}: {ex.Message}";
            result.EndTime = DateTime.Now;
            result.Duration = result.EndTime - result.StartTime;
            
            await LogAsync($"Error processing {result.FilePath}: {result.ErrorMessage}");
        }
        
        // Log messages asynchronously
        private async Task LogAsync(string message)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
        }
    }
    
    public class ProcessingResult
    {
        public string FilePath { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public bool IsDownloaded { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int FileSize { get; set; }
        public int LineCount { get; set; }
        public int WordCount { get; set; }
        public int CharacterCount { get; set; }
        public FileStatistics? Statistics { get; set; }
    }
    
    public class FileStatistics
    {
        public double AverageLineLength { get; set; }
        public int LongestLine { get; set; }
        public int ShortestLine { get; set; }
        public int UniqueWords { get; set; }
        public List<WordFrequency> MostCommonWords { get; set; } = new List<WordFrequency>();
    }
    
    public class WordFrequency
    {
        public string Word { get; set; } = string.Empty;
        public int Count { get; set; }
    }
    
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Async File Processor ===");
            
            var processor = new FileProcessor();
            
            // Create some sample files
            await CreateSampleFilesAsync();
            
            // Process files asynchronously
            var filePaths = new List<string>
            {
                "sample1.txt",
                "sample2.txt",
                "sample3.txt"
            };
            
            Console.WriteLine("Processing files asynchronously...");
            var results = await processor.ProcessFilesAsync(filePaths);
            
            // Display results
            Console.WriteLine("\n=== Processing Results ===");
            foreach (var result in results)
            {
                DisplayResult(result);
            }
            
            // Download and process a file
            Console.WriteLine("\n=== Download and Process ===");
            try
            {
                var downloadResult = await processor.DownloadAndProcessAsync(
                    "https://raw.githubusercontent.com/microsoft/dotnet/main/README.md",
                    "downloaded_readme.md"
                );
                DisplayResult(downloadResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Download failed: {ex.Message}");
            }
        }
        
        static async Task CreateSampleFilesAsync()
        {
            var sampleTexts = new[]
            {
                "This is the first sample file.\nIt contains multiple lines.\nEach line has different content.",
                "Second sample file with different content.\nMore lines here.\nAnd even more content.",
                "Third file with some interesting content.\nProgramming is fun.\nC# is awesome.\nAsync programming rocks!"
            };
            
            for (int i = 0; i < sampleTexts.Length; i++)
            {
                await File.WriteAllTextAsync($"sample{i + 1}.txt", sampleTexts[i]);
            }
        }
        
        static void DisplayResult(ProcessingResult result)
        {
            Console.WriteLine($"\nFile: {result.FilePath}");
            Console.WriteLine($"Success: {result.IsSuccess}");
            Console.WriteLine($"Duration: {result.Duration.TotalMilliseconds:F2}ms");
            
            if (result.IsSuccess)
            {
                Console.WriteLine($"Size: {result.FileSize} characters");
                Console.WriteLine($"Lines: {result.LineCount}");
                Console.WriteLine($"Words: {result.WordCount}");
                Console.WriteLine($"Characters: {result.CharacterCount}");
                
                if (result.Statistics != null)
                {
                    Console.WriteLine($"Average line length: {result.Statistics.AverageLineLength:F1}");
                    Console.WriteLine($"Unique words: {result.Statistics.UniqueWords}");
                    Console.WriteLine("Most common words:");
                    foreach (var word in result.Statistics.MostCommonWords)
                    {
                        Console.WriteLine($"  {word.Word}: {word.Count}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Error: {result.ErrorMessage}");
            }
        }
    }
} 