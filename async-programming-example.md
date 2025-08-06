# Asynchronous Programming in C# - Comprehensive Guide

This guide demonstrates asynchronous programming patterns in C# with practical examples focusing on dependency injection, SQL Server, and REST API development.

## 🎯 Understanding Asynchronous Programming

### **Why Asynchronous Programming?**
- **Non-blocking operations**: UI remains responsive
- **Better resource utilization**: Efficient use of threads
- **Scalability**: Handle more concurrent operations
- **Performance**: Improved application responsiveness

### **Key Concepts**
- **async/await**: Modern C# async pattern
- **Task<T>**: Represents an asynchronous operation
- **ConfigureAwait()**: Control continuation context
- **CancellationToken**: Cancel long-running operations

## 📚 Basic Async/Await Patterns

### **Simple Async Method**

```csharp
public class AsyncExamples
{
    // Basic async method
    public async Task<string> GetDataAsync()
    {
        // Simulate async work
        await Task.Delay(1000);
        return "Data retrieved successfully";
    }

    // Async method with return value
    public async Task<int> CalculateAsync(int a, int b)
    {
        await Task.Delay(500); // Simulate work
        return a + b;
    }

    // Async void (use sparingly - mainly for event handlers)
    public async void HandleButtonClickAsync()
    {
        try
        {
            var result = await GetDataAsync();
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

### **Async Method with Exception Handling**

```csharp
public class AsyncErrorHandling
{
    public async Task<string> GetDataWithRetryAsync(int maxRetries = 3)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                // Simulate potentially failing operation
                await Task.Delay(1000);
                
                // Simulate random failure
                if (Random.Shared.Next(1, 4) == 1)
                {
                    throw new InvalidOperationException("Simulated failure");
                }
                
                return $"Success on attempt {attempt}";
            }
            catch (Exception ex)
            {
                if (attempt == maxRetries)
                {
                    throw new InvalidOperationException($"Failed after {maxRetries} attempts", ex);
                }
                
                // Wait before retry
                await Task.Delay(1000 * attempt);
            }
        }
        
        throw new InvalidOperationException("Unexpected failure");
    }
}
```

## 🗄️ Database Operations with Async/Await

### **Entity Framework Core Async Operations**

```csharp
public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(int id);
    Task<Contact> AddContactAsync(Contact contact);
    Task<Contact> UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(int id);
    Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm);
}

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext _context;

    public ContactRepository(ContactDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        return await _context.Contacts
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contact> AddContactAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> UpdateContactAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm)
    {
        return await _context.Contacts
            .Where(c => c.Name.Contains(searchTerm) || c.Email.Contains(searchTerm))
            .ToListAsync();
    }
}
```

### **Service Layer with Async Operations**

```csharp
public interface IContactService
{
    Task<IEnumerable<Contact>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(int id);
    Task<Contact> CreateContactAsync(CreateContactRequest request);
    Task<Contact> UpdateContactAsync(int id, UpdateContactRequest request);
    Task DeleteContactAsync(int id);
    Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm);
    Task<ContactStatistics> GetContactStatisticsAsync();
}

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IContactRepository repository, ILogger<ContactService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        _logger.LogInformation("Retrieving all contacts");
        
        try
        {
            var contacts = await _repository.GetAllContactsAsync();
            _logger.LogInformation("Retrieved {Count} contacts", contacts.Count());
            return contacts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contacts");
            throw;
        }
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving contact with ID: {Id}", id);
        
        if (id <= 0)
        {
            throw new ArgumentException("Invalid contact ID", nameof(id));
        }

        try
        {
            var contact = await _repository.GetContactByIdAsync(id);
            
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found", id);
            }
            
            return contact;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contact with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Contact> CreateContactAsync(CreateContactRequest request)
    {
        _logger.LogInformation("Creating new contact: {Name}", request.Name);
        
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required", nameof(request.Name));
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Email is required", nameof(request.Email));
            }

            var contact = new Contact
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                City = request.City,
                Category = request.Category,
                Birthday = request.Birthday,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdContact = await _repository.AddContactAsync(contact);
            _logger.LogInformation("Created contact with ID: {Id}", createdContact.Id);
            
            return createdContact;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contact: {Name}", request.Name);
            throw;
        }
    }

    public async Task<Contact> UpdateContactAsync(int id, UpdateContactRequest request)
    {
        _logger.LogInformation("Updating contact with ID: {Id}", id);
        
        try
        {
            var existingContact = await _repository.GetContactByIdAsync(id);
            if (existingContact == null)
            {
                throw new InvalidOperationException($"Contact with ID {id} not found");
            }

            // Update properties
            if (!string.IsNullOrWhiteSpace(request.Name))
                existingContact.Name = request.Name;
            
            if (!string.IsNullOrWhiteSpace(request.Email))
                existingContact.Email = request.Email;
            
            if (!string.IsNullOrWhiteSpace(request.Phone))
                existingContact.Phone = request.Phone;
            
            if (!string.IsNullOrWhiteSpace(request.City))
                existingContact.City = request.City;
            
            if (request.Category.HasValue)
                existingContact.Category = request.Category.Value;
            
            if (request.Birthday.HasValue)
                existingContact.Birthday = request.Birthday;

            existingContact.UpdatedAt = DateTime.UtcNow;

            var updatedContact = await _repository.UpdateContactAsync(existingContact);
            _logger.LogInformation("Updated contact with ID: {Id}", id);
            
            return updatedContact;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating contact with ID: {Id}", id);
            throw;
        }
    }

    public async Task DeleteContactAsync(int id)
    {
        _logger.LogInformation("Deleting contact with ID: {Id}", id);
        
        try
        {
            await _repository.DeleteContactAsync(id);
            _logger.LogInformation("Deleted contact with ID: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contact with ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm)
    {
        _logger.LogInformation("Searching contacts with term: {SearchTerm}", searchTerm);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Enumerable.Empty<Contact>();
        }

        try
        {
            var contacts = await _repository.SearchContactsAsync(searchTerm);
            _logger.LogInformation("Found {Count} contacts matching '{SearchTerm}'", 
                contacts.Count(), searchTerm);
            return contacts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching contacts with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<ContactStatistics> GetContactStatisticsAsync()
    {
        _logger.LogInformation("Generating contact statistics");
        
        try
        {
            var allContacts = await _repository.GetAllContactsAsync();
            var contacts = allContacts.ToList();

            var statistics = new ContactStatistics
            {
                TotalContacts = contacts.Count,
                ContactsByCategory = contacts.GroupBy(c => c.Category)
                    .ToDictionary(g => g.Key, g => g.Count()),
                AverageAge = contacts.Where(c => c.Birthday.HasValue)
                    .Select(c => DateTime.Now.Year - c.Birthday.Value.Year)
                    .DefaultIfEmpty(0)
                    .Average(),
                RecentContacts = contacts.Count(c => c.CreatedAt >= DateTime.UtcNow.AddDays(-7))
            };

            _logger.LogInformation("Generated statistics: {TotalContacts} total contacts", 
                statistics.TotalContacts);
            
            return statistics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating contact statistics");
            throw;
        }
    }
}
```

## 🌐 HTTP Client with Async/Await

### **Weather API Client**

```csharp
public interface IWeatherApiClient
{
    Task<WeatherForecast> GetWeatherAsync(string city);
    Task<IEnumerable<WeatherForecast>> GetWeatherHistoryAsync(string city, DateTime startDate, DateTime endDate);
    Task<double> ConvertTemperatureAsync(double temperature, string fromScale, string toScale);
}

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherApiClient> _logger;
    private readonly string _baseUrl;

    public WeatherApiClient(HttpClient httpClient, ILogger<WeatherApiClient> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["WeatherApi:BaseUrl"] ?? "https://api.weatherapi.com/v1";
    }

    public async Task<WeatherForecast> GetWeatherAsync(string city)
    {
        _logger.LogInformation("Fetching weather for city: {City}", city);
        
        try
        {
            var url = $"{_baseUrl}/current.json?key=your-api-key&q={Uri.EscapeDataString(city)}";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Weather API returned status code: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"Weather API returned {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(content);
            
            if (weatherData?.Current == null)
            {
                throw new InvalidOperationException("Invalid weather data received");
            }

            var forecast = new WeatherForecast
            {
                City = city,
                TemperatureC = weatherData.Current.TempC,
                TemperatureF = weatherData.Current.TempF,
                Description = weatherData.Current.Condition?.Text ?? "Unknown",
                Humidity = weatherData.Current.Humidity,
                WindSpeed = weatherData.Current.WindKph,
                LastUpdated = DateTime.UtcNow
            };

            _logger.LogInformation("Successfully fetched weather for {City}: {Temperature}°C", 
                city, forecast.TemperatureC);
            
            return forecast;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching weather for {City}", city);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for weather data from {City}", city);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching weather for {City}", city);
            throw;
        }
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherHistoryAsync(string city, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Fetching weather history for {City} from {StartDate} to {EndDate}", 
            city, startDate.ToShortDateString(), endDate.ToShortDateString());
        
        try
        {
            var forecasts = new List<WeatherForecast>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var url = $"{_baseUrl}/history.json?key=your-api-key&q={Uri.EscapeDataString(city)}&dt={currentDate:yyyy-MM-dd}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var historyData = JsonSerializer.Deserialize<WeatherHistoryResponse>(content);
                    
                    if (historyData?.Forecast?.ForecastDay?.Any() == true)
                    {
                        var dayForecast = historyData.Forecast.ForecastDay.First();
                        forecasts.Add(new WeatherForecast
                        {
                            City = city,
                            Date = currentDate,
                            TemperatureC = dayForecast.Day.AvgTempC,
                            TemperatureF = dayForecast.Day.AvgTempF,
                            Description = dayForecast.Day.Condition?.Text ?? "Unknown",
                            MaxTempC = dayForecast.Day.MaxTempC,
                            MinTempC = dayForecast.Day.MinTempC
                        });
                    }
                }

                currentDate = currentDate.AddDays(1);
                
                // Rate limiting - wait between requests
                await Task.Delay(100);
            }

            _logger.LogInformation("Retrieved {Count} historical weather records for {City}", 
                forecasts.Count, city);
            
            return forecasts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather history for {City}", city);
            throw;
        }
    }

    public async Task<double> ConvertTemperatureAsync(double temperature, string fromScale, string toScale)
    {
        _logger.LogInformation("Converting temperature {Temperature} from {FromScale} to {ToScale}", 
            temperature, fromScale, toScale);
        
        try
        {
            // Simulate API call for temperature conversion
            await Task.Delay(100);
            
            double convertedTemperature = fromScale.ToUpper() switch
            {
                "C" when toScale.ToUpper() == "F" => (temperature * 9/5) + 32,
                "F" when toScale.ToUpper() == "C" => (temperature - 32) * 5/9,
                "C" when toScale.ToUpper() == "K" => temperature + 273.15,
                "K" when toScale.ToUpper() == "C" => temperature - 273.15,
                "F" when toScale.ToUpper() == "K" => (temperature - 32) * 5/9 + 273.15,
                "K" when toScale.ToUpper() == "F" => (temperature - 273.15) * 9/5 + 32,
                _ => throw new ArgumentException($"Unsupported temperature conversion: {fromScale} to {toScale}")
            };

            _logger.LogInformation("Converted {FromTemp}°{FromScale} to {ToTemp}°{ToScale}", 
                temperature, fromScale, convertedTemperature, toScale);
            
            return Math.Round(convertedTemperature, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting temperature from {FromScale} to {ToScale}", 
                fromScale, toScale);
            throw;
        }
    }
}
```

## 🔄 Parallel and Concurrent Operations

### **Parallel Processing Example**

```csharp
public class ParallelProcessingService
{
    private readonly ILogger<ParallelProcessingService> _logger;

    public ParallelProcessingService(ILogger<ParallelProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task<List<ProcessedData>> ProcessDataParallelAsync(IEnumerable<RawData> dataItems)
    {
        _logger.LogInformation("Starting parallel processing of {Count} items", dataItems.Count());
        
        var tasks = dataItems.Select(async item =>
        {
            try
            {
                return await ProcessSingleItemAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing item {Id}", item.Id);
                return new ProcessedData { Id = item.Id, IsError = true, ErrorMessage = ex.Message };
            }
        });

        var results = await Task.WhenAll(tasks);
        
        _logger.LogInformation("Completed parallel processing. Success: {SuccessCount}, Errors: {ErrorCount}", 
            results.Count(r => !r.IsError), results.Count(r => r.IsError));
        
        return results.ToList();
    }

    public async Task<ProcessedData> ProcessSingleItemAsync(RawData item)
    {
        _logger.LogDebug("Processing item {Id}", item.Id);
        
        // Simulate processing work
        await Task.Delay(Random.Shared.Next(100, 500));
        
        return new ProcessedData
        {
            Id = item.Id,
            ProcessedValue = item.Value * 2,
            ProcessedAt = DateTime.UtcNow,
            IsError = false
        };
    }

    public async Task<BatchProcessingResult> ProcessDataInBatchesAsync(IEnumerable<RawData> dataItems, int batchSize = 10)
    {
        _logger.LogInformation("Starting batch processing of {TotalCount} items in batches of {BatchSize}", 
            dataItems.Count(), batchSize);
        
        var allResults = new List<ProcessedData>();
        var batches = dataItems.Chunk(batchSize);
        var batchNumber = 1;

        foreach (var batch in batches)
        {
            _logger.LogInformation("Processing batch {BatchNumber}", batchNumber);
            
            var batchResults = await ProcessDataParallelAsync(batch);
            allResults.AddRange(batchResults);
            
            batchNumber++;
            
            // Small delay between batches to prevent overwhelming the system
            await Task.Delay(100);
        }

        var result = new BatchProcessingResult
        {
            TotalProcessed = allResults.Count,
            SuccessfulItems = allResults.Count(r => !r.IsError),
            FailedItems = allResults.Count(r => r.IsError),
            ProcessingTime = DateTime.UtcNow
        };

        _logger.LogInformation("Batch processing completed: {Result}", result);
        
        return result;
    }
}
```

## ⏱️ Cancellation and Timeout

### **Cancellation Token Example**

```csharp
public class CancellableOperations
{
    private readonly ILogger<CancellableOperations> _logger;

    public CancellableOperations(ILogger<CancellableOperations> logger)
    {
        _logger = logger;
    }

    public async Task<string> LongRunningOperationAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting long-running operation");
        
        try
        {
            // Simulate work with cancellation support
            for (int i = 0; i < 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                _logger.LogDebug("Processing step {Step}/10", i + 1);
                await Task.Delay(1000, cancellationToken);
            }
            
            _logger.LogInformation("Long-running operation completed successfully");
            return "Operation completed successfully";
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Long-running operation was cancelled");
            throw;
        }
    }

    public async Task<string> OperationWithTimeoutAsync(TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        
        try
        {
            return await LongRunningOperationAsync(cts.Token);
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds");
        }
    }

    public async Task<IEnumerable<string>> ProcessWithCancellationAsync(
        IEnumerable<string> items, 
        CancellationToken cancellationToken = default)
    {
        var results = new List<string>();
        
        foreach (var item in items)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                var result = await ProcessItemAsync(item, cancellationToken);
                results.Add(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Processing cancelled while processing item: {Item}", item);
                throw;
            }
        }
        
        return results;
    }

    private async Task<string> ProcessItemAsync(string item, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Processing item: {Item}", item);
        
        // Simulate processing time
        await Task.Delay(Random.Shared.Next(100, 500), cancellationToken);
        
        return $"Processed: {item}";
    }
}
```

## 🎯 Real-World Example: File Processing Service

```csharp
public interface IFileProcessingService
{
    Task<ProcessingResult> ProcessFilesAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default);
    Task<FileAnalysis> AnalyzeFileAsync(string filePath, CancellationToken cancellationToken = default);
    Task<IEnumerable<FileInfo>> GetFileListAsync(string directory, string searchPattern = "*.*");
}

public class FileProcessingService : IFileProcessingService
{
    private readonly ILogger<FileProcessingService> _logger;
    private readonly SemaphoreSlim _semaphore;

    public FileProcessingService(ILogger<FileProcessingService> logger)
    {
        _logger = logger;
        _semaphore = new SemaphoreSlim(Environment.ProcessorCount); // Limit concurrent operations
    }

    public async Task<ProcessingResult> ProcessFilesAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default)
    {
        var fileList = filePaths.ToList();
        _logger.LogInformation("Starting to process {Count} files", fileList.Count);
        
        var results = new List<FileAnalysis>();
        var errors = new List<string>();
        
        var tasks = fileList.Select(async filePath =>
        {
            await _semaphore.WaitAsync(cancellationToken);
            
            try
            {
                return await AnalyzeFileAsync(filePath, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
                errors.Add($"{filePath}: {ex.Message}");
                return null;
            }
            finally
            {
                _semaphore.Release();
            }
        });

        var completedTasks = await Task.WhenAll(tasks);
        
        results.AddRange(completedTasks.Where(r => r != null)!);
        
        var result = new ProcessingResult
        {
            TotalFiles = fileList.Count,
            ProcessedFiles = results.Count,
            FailedFiles = errors.Count,
            ProcessingTime = DateTime.UtcNow,
            Errors = errors,
            FileAnalyses = results
        };
        
        _logger.LogInformation("File processing completed: {Result}", result);
        
        return result;
    }

    public async Task<FileAnalysis> AnalyzeFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Analyzing file: {FilePath}", filePath);
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var fileInfo = new FileInfo(filePath);
        
        try
        {
            var content = await File.ReadAllTextAsync(filePath, cancellationToken);
            
            var analysis = new FileAnalysis
            {
                FilePath = filePath,
                FileSize = fileInfo.Length,
                LineCount = content.Split('\n').Length,
                WordCount = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length,
                CharacterCount = content.Length,
                LastModified = fileInfo.LastWriteTime,
                AnalysisTime = DateTime.UtcNow
            };
            
            _logger.LogDebug("Analysis completed for {FilePath}: {LineCount} lines, {WordCount} words", 
                filePath, analysis.LineCount, analysis.WordCount);
            
            return analysis;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("File analysis cancelled for: {FilePath}", filePath);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing file: {FilePath}", filePath);
            throw;
        }
    }

    public async Task<IEnumerable<FileInfo>> GetFileListAsync(string directory, string searchPattern = "*.*")
    {
        _logger.LogDebug("Getting file list from directory: {Directory}", directory);
        
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directory}");
        }

        try
        {
            var files = Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories);
            var fileInfos = files.Select(f => new FileInfo(f)).ToList();
            
            _logger.LogDebug("Found {Count} files in {Directory}", fileInfos.Count, directory);
            
            return fileInfos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file list from directory: {Directory}", directory);
            throw;
        }
    }
}

public class FileAnalysis
{
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int LineCount { get; set; }
    public int WordCount { get; set; }
    public int CharacterCount { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime AnalysisTime { get; set; }
}

public class ProcessingResult
{
    public int TotalFiles { get; set; }
    public int ProcessedFiles { get; set; }
    public int FailedFiles { get; set; }
    public DateTime ProcessingTime { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<FileAnalysis> FileAnalyses { get; set; } = new();
}
```

## 🔧 Dependency Injection Setup

```csharp
// Program.cs or Startup.cs
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncServices(this IServiceCollection services)
    {
        // Register HTTP client for weather API
        services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.weatherapi.com/v1/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Register repositories and services
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IParallelProcessingService, ParallelProcessingService>();
        services.AddScoped<ICancellableOperations, CancellableOperations>();
        services.AddScoped<IFileProcessingService, FileProcessingService>();

        return services;
    }
}
```

## 🎯 Best Practices

### **1. Always Use ConfigureAwait(false) for Library Code**
```csharp
public async Task<string> GetDataAsync()
{
    var result = await SomeAsyncOperation().ConfigureAwait(false);
    return result;
}
```

### **2. Handle Exceptions Properly**
```csharp
public async Task<string> SafeOperationAsync()
{
    try
    {
        return await RiskyOperationAsync();
    }
    catch (HttpRequestException ex)
    {
        _logger.LogError(ex, "Network error occurred");
        return "Fallback value";
    }
    catch (OperationCanceledException)
    {
        _logger.LogWarning("Operation was cancelled");
        throw;
    }
}
```

### **3. Use Cancellation Tokens**
```csharp
public async Task ProcessDataAsync(CancellationToken cancellationToken = default)
{
    for (int i = 0; i < 100; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await ProcessItemAsync(i, cancellationToken);
    }
}
```

### **4. Avoid async void (except for event handlers)**
```csharp
// ❌ Bad
public async void BadMethod()
{
    await SomeAsyncOperation();
}

// ✅ Good
public async Task GoodMethodAsync()
{
    await SomeAsyncOperation();
}
```

### **5. Use Task.WhenAll for Parallel Operations**
```csharp
public async Task<IEnumerable<Result>> ProcessAllAsync(IEnumerable<Input> inputs)
{
    var tasks = inputs.Select(ProcessSingleAsync);
    return await Task.WhenAll(tasks);
}
```

This comprehensive guide demonstrates asynchronous programming patterns in C# with real-world examples that integrate with dependency injection, SQL Server operations, and REST API development. The examples show proper error handling, cancellation support, and performance optimization techniques.

## 🧪 Automated Testing for Async Code

### **Testing Async Methods with xUnit**

```csharp
public class AsyncExamplesTests
{
    private readonly AsyncExamples _asyncExamples;

    public AsyncExamplesTests()
    {
        _asyncExamples = new AsyncExamples();
    }

    [Fact]
    public async Task GetDataAsync_ReturnsExpectedResult()
    {
        // Act
        var result = await _asyncExamples.GetDataAsync();

        // Assert
        result.Should().Be("Data retrieved successfully");
    }

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(10, 5, 15)]
    [InlineData(-1, 1, 0)]
    public async Task CalculateAsync_ValidInputs_ReturnsCorrectSum(int a, int b, int expected)
    {
        // Act
        var result = await _asyncExamples.CalculateAsync(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public async Task GetDataWithRetryAsync_SucceedsOnFirstAttempt_ReturnsSuccess()
    {
        // Arrange
        var retryHandler = new AsyncErrorHandling();

        // Act
        var result = await retryHandler.GetDataWithRetryAsync(maxRetries: 3);

        // Assert
        result.Should().Contain("Success on attempt");
    }

    [Fact]
    public async Task GetDataWithRetryAsync_FailsMultipleTimes_ThrowsException()
    {
        // Arrange
        var retryHandler = new AsyncErrorHandling();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            retryHandler.GetDataWithRetryAsync(maxRetries: 1));

        exception.Message.Should().Contain("Failed after 1 attempts");
    }
}
```

### **Testing Database Operations with In-Memory Database**

```csharp
public class ContactRepositoryTests : IDisposable
{
    private readonly DbContextOptions<ContactDbContext> _options;
    private readonly ContactDbContext _context;
    private readonly ContactRepository _repository;

    public ContactRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ContactDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ContactDbContext(_options);
        _repository = new ContactRepository(_context);
    }

    [Fact]
    public async Task GetAllContactsAsync_EmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetAllContactsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddContactAsync_ValidContact_AddsToDatabase()
    {
        // Arrange
        var contact = new Contact
        {
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "555-0101",
            Category = ContactCategory.Family
        };

        // Act
        var result = await _repository.AddContactAsync(contact);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("John Doe");

        // Verify it was actually saved
        var savedContact = await _context.Contacts.FindAsync(result.Id);
        savedContact.Should().NotBeNull();
        savedContact!.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetContactByIdAsync_ValidId_ReturnsContact()
    {
        // Arrange
        var contact = new Contact
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            Category = ContactCategory.Work
        };
        await _repository.AddContactAsync(contact);

        // Act
        var result = await _repository.GetContactByIdAsync(contact.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Jane Smith");
        result.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public async Task GetContactByIdAsync_InvalidId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetContactByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task SearchContactsAsync_ValidSearchTerm_ReturnsMatchingContacts()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Name = "John Smith", Email = "john@example.com" },
            new Contact { Name = "Jane Doe", Email = "jane@example.com" },
            new Contact { Name = "Bob Johnson", Email = "bob@example.com" }
        };

        foreach (var contact in contacts)
        {
            await _repository.AddContactAsync(contact);
        }

        // Act
        var results = await _repository.SearchContactsAsync("john");

        // Assert
        results.Should().HaveCount(2); // "John Smith" and "Bob Johnson"
        results.Should().Contain(c => c.Name.Contains("John", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateContactAsync_ValidContact_UpdatesSuccessfully()
    {
        // Arrange
        var contact = new Contact
        {
            Name = "Original Name",
            Email = "original@example.com",
            Category = ContactCategory.Family
        };
        await _repository.AddContactAsync(contact);

        contact.Name = "Updated Name";
        contact.Email = "updated@example.com";

        // Act
        var result = await _repository.UpdateContactAsync(contact);

        // Assert
        result.Name.Should().Be("Updated Name");
        result.Email.Should().Be("updated@example.com");

        // Verify it was actually updated in database
        var updatedContact = await _context.Contacts.FindAsync(contact.Id);
        updatedContact!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteContactAsync_ValidId_RemovesFromDatabase()
    {
        // Arrange
        var contact = new Contact
        {
            Name = "To Delete",
            Email = "delete@example.com",
            Category = ContactCategory.Other
        };
        await _repository.AddContactAsync(contact);
        var contactId = contact.Id;

        // Act
        await _repository.DeleteContactAsync(contactId);

        // Assert
        var deletedContact = await _context.Contacts.FindAsync(contactId);
        deletedContact.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
```

### **Testing Service Layer with Mocked Dependencies**

```csharp
public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockRepository;
    private readonly Mock<ILogger<ContactService>> _mockLogger;
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _mockRepository = new Mock<IContactRepository>();
        _mockLogger = new Mock<ILogger<ContactService>>();
        _service = new ContactService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllContactsAsync_ValidData_ReturnsContacts()
    {
        // Arrange
        var expectedContacts = new List<Contact>
        {
            new Contact { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new Contact { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
        };

        _mockRepository.Setup(r => r.GetAllContactsAsync())
            .ReturnsAsync(expectedContacts);

        // Act
        var result = await _service.GetAllContactsAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedContacts);
        _mockRepository.Verify(r => r.GetAllContactsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetContactByIdAsync_ValidId_ReturnsContact()
    {
        // Arrange
        var expectedContact = new Contact
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Category = ContactCategory.Family
        };

        _mockRepository.Setup(r => r.GetContactByIdAsync(1))
            .ReturnsAsync(expectedContact);

        // Act
        var result = await _service.GetContactByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(expectedContact);
        _mockRepository.Verify(r => r.GetContactByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetContactByIdAsync_InvalidId_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetContactByIdAsync(0));

        exception.ParamName.Should().Be("id");
    }

    [Fact]
    public async Task CreateContactAsync_ValidRequest_CreatesContact()
    {
        // Arrange
        var request = new CreateContactRequest
        {
            Name = "New Contact",
            Email = "new@example.com",
            Phone = "555-0101",
            Category = ContactCategory.Work
        };

        var expectedContact = new Contact
        {
            Id = 1,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Category = request.Category
        };

        _mockRepository.Setup(r => r.AddContactAsync(It.IsAny<Contact>()))
            .ReturnsAsync(expectedContact);

        // Act
        var result = await _service.CreateContactAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
        _mockRepository.Verify(r => r.AddContactAsync(It.IsAny<Contact>()), Times.Once);
    }

    [Fact]
    public async Task CreateContactAsync_InvalidRequest_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateContactRequest
        {
            Name = "", // Invalid
            Email = "test@example.com"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateContactAsync(request));

        exception.ParamName.Should().Be("request.Name");
    }

    [Fact]
    public async Task SearchContactsAsync_ValidTerm_ReturnsMatchingContacts()
    {
        // Arrange
        var searchTerm = "john";
        var expectedContacts = new List<Contact>
        {
            new Contact { Id = 1, Name = "John Doe", Email = "john@example.com" }
        };

        _mockRepository.Setup(r => r.SearchContactsAsync(searchTerm))
            .ReturnsAsync(expectedContacts);

        // Act
        var result = await _service.SearchContactsAsync(searchTerm);

        // Assert
        result.Should().BeEquivalentTo(expectedContacts);
        _mockRepository.Verify(r => r.SearchContactsAsync(searchTerm), Times.Once);
    }

    [Fact]
    public async Task SearchContactsAsync_EmptyTerm_ReturnsEmptyList()
    {
        // Act
        var result = await _service.SearchContactsAsync("");

        // Assert
        result.Should().BeEmpty();
        _mockRepository.Verify(r => r.SearchContactsAsync(It.IsAny<string>()), Times.Never);
    }
}
```

### **Testing HTTP Client with Mocked HttpClient**

```csharp
public class WeatherApiClientTests
{
    private readonly Mock<ILogger<WeatherApiClient>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly HttpClient _httpClient;
    private readonly WeatherApiClient _client;

    public WeatherApiClientTests()
    {
        _mockLogger = new Mock<ILogger<WeatherApiClient>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _httpClient = new HttpClient();
        _client = new WeatherApiClient(_httpClient, _mockLogger.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task GetWeatherAsync_ValidResponse_ReturnsWeatherForecast()
    {
        // Arrange
        var expectedResponse = new WeatherApiResponse
        {
            Current = new CurrentWeather
            {
                TempC = 25.0,
                TempF = 77.0,
                Condition = new WeatherCondition { Text = "Sunny" },
                Humidity = 60,
                WindKph = 15.0
            }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse);
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var client = new WeatherApiClient(httpClient, _mockLogger.Object, _mockConfiguration.Object);

        // Act
        var result = await client.GetWeatherAsync("New York");

        // Assert
        result.Should().NotBeNull();
        result.TemperatureC.Should().Be(25.0);
        result.TemperatureF.Should().Be(77.0);
        result.Description.Should().Be("Sunny");
    }

    [Fact]
    public async Task GetWeatherAsync_HttpError_ThrowsHttpRequestException()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var client = new WeatherApiClient(httpClient, _mockLogger.Object, _mockConfiguration.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            client.GetWeatherAsync("Invalid City"));

        exception.Message.Should().Contain("404");
    }

    [Fact]
    public async Task ConvertTemperatureAsync_ValidConversion_ReturnsCorrectResult()
    {
        // Act
        var result = await _client.ConvertTemperatureAsync(25.0, "C", "F");

        // Assert
        result.Should().Be(77.0);
    }

    [Fact]
    public async Task ConvertTemperatureAsync_InvalidConversion_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.ConvertTemperatureAsync(25.0, "C", "X"));

        exception.Message.Should().Contain("Unsupported temperature conversion");
    }
}
```

### **Testing Parallel Processing**

```csharp
public class ParallelProcessingServiceTests
{
    private readonly Mock<ILogger<ParallelProcessingService>> _mockLogger;
    private readonly ParallelProcessingService _service;

    public ParallelProcessingServiceTests()
    {
        _mockLogger = new Mock<ILogger<ParallelProcessingService>>();
        _service = new ParallelProcessingService(_mockLogger.Object);
    }

    [Fact]
    public async Task ProcessDataParallelAsync_ValidData_ProcessesAllItems()
    {
        // Arrange
        var dataItems = new List<RawData>
        {
            new RawData { Id = 1, Value = 10 },
            new RawData { Id = 2, Value = 20 },
            new RawData { Id = 3, Value = 30 }
        };

        // Act
        var results = await _service.ProcessDataParallelAsync(dataItems);

        // Assert
        results.Should().HaveCount(3);
        results.Should().OnlyContain(r => !r.IsError);
        results.Should().Contain(r => r.Id == 1 && r.ProcessedValue == 20);
        results.Should().Contain(r => r.Id == 2 && r.ProcessedValue == 40);
        results.Should().Contain(r => r.Id == 3 && r.ProcessedValue == 60);
    }

    [Fact]
    public async Task ProcessDataInBatchesAsync_ValidData_ProcessesInBatches()
    {
        // Arrange
        var dataItems = Enumerable.Range(1, 25)
            .Select(i => new RawData { Id = i, Value = i })
            .ToList();

        // Act
        var result = await _service.ProcessDataInBatchesAsync(dataItems, batchSize: 10);

        // Assert
        result.TotalProcessed.Should().Be(25);
        result.SuccessfulItems.Should().Be(25);
        result.FailedItems.Should().Be(0);
    }
}
```

### **Testing Cancellation and Timeout**

```csharp
public class CancellableOperationsTests
{
    private readonly Mock<ILogger<CancellableOperations>> _mockLogger;
    private readonly CancellableOperations _operations;

    public CancellableOperationsTests()
    {
        _mockLogger = new Mock<ILogger<CancellableOperations>>();
        _operations = new CancellableOperations(_mockLogger.Object);
    }

    [Fact]
    public async Task LongRunningOperationAsync_NoCancellation_CompletesSuccessfully()
    {
        // Act
        var result = await _operations.LongRunningOperationAsync();

        // Assert
        result.Should().Be("Operation completed successfully");
    }

    [Fact]
    public async Task LongRunningOperationAsync_Cancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() =>
            _operations.LongRunningOperationAsync(cts.Token));

        exception.Should().BeOfType<OperationCanceledException>();
    }

    [Fact]
    public async Task OperationWithTimeoutAsync_TimeoutExceeded_ThrowsTimeoutException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<TimeoutException>(() =>
            _operations.OperationWithTimeoutAsync(TimeSpan.FromMilliseconds(100)));

        exception.Message.Should().Contain("timed out");
    }

    [Fact]
    public async Task ProcessWithCancellationAsync_Cancelled_ThrowsOperationCanceledException()
    {
        // Arrange
        var items = new[] { "item1", "item2", "item3" };
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() =>
            _operations.ProcessWithCancellationAsync(items, cts.Token));

        exception.Should().BeOfType<OperationCanceledException>();
    }
}
```

### **Testing File Processing Service**

```csharp
public class FileProcessingServiceTests : IDisposable
{
    private readonly Mock<ILogger<FileProcessingService>> _mockLogger;
    private readonly FileProcessingService _service;
    private readonly string _testDirectory;

    public FileProcessingServiceTests()
    {
        _mockLogger = new Mock<ILogger<FileProcessingService>>();
        _service = new FileProcessingService(_mockLogger.Object);
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public async Task AnalyzeFileAsync_ValidFile_ReturnsFileAnalysis()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "test.txt");
        var content = "This is a test file.\nIt has multiple lines.\nAnd some content.";
        await File.WriteAllTextAsync(filePath, content);

        // Act
        var result = await _service.AnalyzeFileAsync(filePath);

        // Assert
        result.Should().NotBeNull();
        result.FilePath.Should().Be(filePath);
        result.LineCount.Should().Be(3);
        result.WordCount.Should().Be(10);
        result.CharacterCount.Should().Be(content.Length);
        result.FileSize.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AnalyzeFileAsync_FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonExistentFile = Path.Combine(_testDirectory, "nonexistent.txt");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FileNotFoundException>(() =>
            _service.AnalyzeFileAsync(nonExistentFile));

        exception.FileName.Should().Be(nonExistentFile);
    }

    [Fact]
    public async Task ProcessFilesAsync_MultipleFiles_ProcessesAllFiles()
    {
        // Arrange
        var files = new List<string>();
        for (int i = 1; i <= 3; i++)
        {
            var filePath = Path.Combine(_testDirectory, $"file{i}.txt");
            await File.WriteAllTextAsync(filePath, $"Content for file {i}");
            files.Add(filePath);
        }

        // Act
        var result = await _service.ProcessFilesAsync(files);

        // Assert
        result.TotalFiles.Should().Be(3);
        result.ProcessedFiles.Should().Be(3);
        result.FailedFiles.Should().Be(0);
        result.FileAnalyses.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetFileListAsync_ValidDirectory_ReturnsFileList()
    {
        // Arrange
        var files = new List<string>();
        for (int i = 1; i <= 3; i++)
        {
            var filePath = Path.Combine(_testDirectory, $"file{i}.txt");
            await File.WriteAllTextAsync(filePath, $"Content for file {i}");
            files.Add(filePath);
        }

        // Act
        var result = await _service.GetFileListAsync(_testDirectory);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(f => f.Name == "file1.txt");
        result.Should().Contain(f => f.Name == "file2.txt");
        result.Should().Contain(f => f.Name == "file3.txt");
    }

    [Fact]
    public async Task GetFileListAsync_InvalidDirectory_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        var invalidDirectory = Path.Combine(_testDirectory, "nonexistent");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DirectoryNotFoundException>(() =>
            _service.GetFileListAsync(invalidDirectory));

        exception.Message.Should().Contain("Directory not found");
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}
```

### **Integration Tests with Testcontainers**

```csharp
public class ContactRepositoryIntegrationTests : IAsyncDisposable
{
    private readonly MsSqlContainer _container;
    private readonly ContactDbContext _context;
    private readonly ContactRepository _repository;

    public ContactRepositoryIntegrationTests()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithPassword("Your_password123!")
            .Build();

        _container.StartAsync().Wait();

        var options = new DbContextOptionsBuilder<ContactDbContext>()
            .UseSqlServer(_container.GetConnectionString())
            .Options;

        _context = new ContactDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new ContactRepository(_context);
    }

    [Fact]
    public async Task AddContactAsync_WithRealDatabase_SavesCorrectly()
    {
        // Arrange
        var contact = new Contact
        {
            Name = "Integration Test",
            Email = "integration@test.com",
            Category = ContactCategory.Work
        };

        // Act
        var result = await _repository.AddContactAsync(contact);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);

        // Verify it was actually saved to the database
        var savedContact = await _context.Contacts.FindAsync(result.Id);
        savedContact.Should().NotBeNull();
        savedContact!.Name.Should().Be("Integration Test");
    }

    [Fact]
    public async Task SearchContactsAsync_WithRealDatabase_ReturnsMatchingContacts()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Name = "John Smith", Email = "john@example.com" },
            new Contact { Name = "Jane Doe", Email = "jane@example.com" },
            new Contact { Name = "Bob Johnson", Email = "bob@example.com" }
        };

        foreach (var contact in contacts)
        {
            await _repository.AddContactAsync(contact);
        }

        // Act
        var results = await _repository.SearchContactsAsync("john");

        // Assert
        results.Should().HaveCount(2); // "John Smith" and "Bob Johnson"
        results.Should().Contain(c => c.Name.Contains("John", StringComparison.OrdinalIgnoreCase));
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _container.DisposeAsync();
    }
}
```

### **Test Configuration and Setup**

```xml
<!-- YourProject.Tests.csproj -->
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.6.6" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="Moq" Version="4.20.70" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
    <PackageReference Include="Testcontainers.MsSql" Version="3.7.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\YourProject\YourProject.csproj" />
  </ItemGroup>

</Project>
```

### **Running Async Tests**

```bash
# Run all async tests
dotnet test --filter "Category=Async"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~ContactRepositoryTests"

# Run tests in parallel
dotnet test --maxcpucount:4
```

This comprehensive testing section provides complete coverage for all the async programming examples, including unit tests, integration tests, and real-world testing scenarios with proper mocking, error handling, and database testing. 