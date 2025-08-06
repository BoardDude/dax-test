# Detailed Exercise Definitions

This document provides comprehensive definitions for each exercise in the C# curriculum, including specific requirements, implementation guidance, and code examples.

## Month 1: Foundations & Dependency Injection Basics

### Exercise 1: Personal Information Display with DI

**Objective**: Create a console application that collects and displays personal information using dependency injection.

**Core Requirements**:
- Collect name, age, city, favorite programming language, and years of experience
- Use dependency injection for input and display services
- Validate user input (age range 0-120)
- Handle null/empty input gracefully

**Required Interfaces**:
```csharp
public interface IUserInputService
{
    string GetStringInput(string prompt);
    int GetIntInput(string prompt);
    bool ValidateAge(int age);
}

public interface IDisplayService
{
    void DisplayWelcome();
    void DisplayPersonalInfo(string name, int age, string city, string language, int experience);
    void DisplayError(string message);
}
```

**Implementation Steps**:
1. Create the interfaces above
2. Implement `UserInputService` with input validation
3. Implement `DisplayService` with formatted output
4. Create `PersonalInfoCollector` class using constructor injection
5. Configure DI container in `Program.cs`
6. Handle edge cases (null input, invalid age)

**Expected Output**:
```
=== Personal Information Collector ===
Enter your name: John Doe
Enter your age: 25
Enter your city: New York
Enter your favorite programming language: C#
Enter your years of programming experience: 3

=== Your Information ===
Name: John Doe
Age: 25
City: New York
Favorite Programming Language: C#
Years of Experience: 3
```

### Exercise 2: Temperature Converter with Service Pattern

**Objective**: Build a temperature converter supporting Celsius, Fahrenheit, and Kelvin using the service pattern.

**Core Requirements**:
- Support conversions between C, F, and K
- Use proper mathematical formulas
- Display results with 2 decimal places
- Handle invalid input gracefully
- Implement strategy pattern for different conversion algorithms

**Required Interfaces**:
```csharp
public interface ITemperatureConverter
{
    double Convert(double value, string fromScale, string toScale);
    bool IsValidScale(string scale);
}

public interface ITemperatureDisplayService
{
    void DisplayConversion(double originalValue, string fromScale, double convertedValue, string toScale);
    void DisplayError(string message);
    void DisplayAvailableScales();
}
```

**Conversion Formulas**:
- C to F: `(C × 9/5) + 32`
- F to C: `(F - 32) × 5/9`
- C to K: `C + 273.15`
- K to C: `K - 273.15`

**Implementation Steps**:
1. Create the interfaces above
2. Implement `TemperatureConverterService` with conversion logic
3. Implement `TemperatureDisplayService` for formatted output
4. Create main application class using constructor injection
5. Add input validation for temperature scales
6. Handle conversion errors gracefully

### Exercise 3: Simple Calculator with DI Container

**Objective**: Create a calculator with basic arithmetic operations using dependency injection.

**Core Requirements**:
- Support addition, subtraction, multiplication, division
- Handle division by zero with proper error handling
- Use switch statements for operation selection
- Display formatted results
- Register services with appropriate lifetimes

**Required Interfaces**:
```csharp
public interface ICalculatorService
{
    double Calculate(double a, double b, string operation);
    bool IsValidOperation(string operation);
    string[] GetAvailableOperations();
}

public interface ICalculatorDisplayService
{
    void DisplayWelcome();
    void DisplayResult(double a, double b, string operation, double result);
    void DisplayError(string message);
    void DisplayAvailableOperations(string[] operations);
}
```

**Implementation Steps**:
1. Create the interfaces above
2. Implement `CalculatorService` with arithmetic operations
3. Implement `CalculatorDisplayService` for output formatting
4. Create main application class using constructor injection
5. Configure DI container with appropriate service lifetimes
6. Add comprehensive error handling for division by zero

## Month 2: Object-Oriented Programming & Advanced DI

### Exercise 7: Library Book Management with DI

**Objective**: Create a library management system with books and borrowing functionality using dependency injection.

**Core Requirements**:
- Book class with properties (Title, Author, ISBN, Available)
- Library class to manage books
- Methods to add, remove, search, and check out books
- Display library catalog
- Use repository pattern for data access

**Required Interfaces**:
```csharp
public interface IBookRepository
{
    IEnumerable<Book> GetAllBooks();
    Book? GetBookById(int id);
    Book? GetBookByISBN(string isbn);
    void AddBook(Book book);
    void RemoveBook(int id);
    void UpdateBook(Book book);
    IEnumerable<Book> SearchBooks(string searchTerm);
}

public interface ILibraryService
{
    void AddBook(string title, string author, string isbn);
    void RemoveBook(int id);
    void CheckOutBook(int id);
    void ReturnBook(int id);
    void DisplayAllBooks();
    void SearchBooks(string searchTerm);
    void DisplayBookDetails(int id);
}
```

**Book Class Structure**:
```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public bool Available { get; set; } = true;
}
```

**Implementation Steps**:
1. Create the Book model class
2. Implement `IBookRepository` with in-memory storage
3. Implement `ILibraryService` with business logic
4. Create main application class using constructor injection
5. Add book validation (ISBN format, required fields)
6. Implement search functionality
7. Handle book availability status

### Exercise 8: Bank Account System with DI Container

**Objective**: Expand the bank account system with multiple account types using inheritance and dependency injection.

**Core Requirements**:
- Base Account class with common properties
- Savings and Checking account classes with inheritance
- Different interest rates and fees for each account type
- Transaction history tracking
- Account transfer functionality
- Use factory pattern for account creation

**Required Interfaces**:
```csharp
public interface IAccountService
{
    void CreateAccount(string accountType, string ownerName, decimal initialBalance);
    void Deposit(int accountId, decimal amount);
    void Withdraw(int accountId, decimal amount);
    void Transfer(int fromAccountId, int toAccountId, decimal amount);
    void DisplayAccountDetails(int accountId);
    void DisplayAllAccounts();
}

public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    IEnumerable<Transaction> GetTransactionsByAccount(int accountId);
    IEnumerable<Transaction> GetAllTransactions();
}
```

**Account Class Hierarchy**:
```csharp
public abstract class Account
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public decimal Balance { get; protected set; }
    public string AccountNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public abstract decimal GetInterestRate();
    public abstract decimal GetMonthlyFee();
    
    public virtual void Deposit(decimal amount)
    {
        if (amount > 0)
            Balance += amount;
    }
    
    public virtual bool Withdraw(decimal amount)
    {
        if (amount > 0 && Balance >= amount)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }
}

public class SavingsAccount : Account
{
    public override decimal GetInterestRate() => 0.02m; // 2%
    public override decimal GetMonthlyFee() => 0m; // No monthly fee
}

public class CheckingAccount : Account
{
    public override decimal GetInterestRate() => 0.001m; // 0.1%
    public override decimal GetMonthlyFee() => 5m; // $5 monthly fee
}
```

**Implementation Steps**:
1. Create the Account class hierarchy
2. Implement `IAccountService` with account management logic
3. Implement `ITransactionRepository` for transaction tracking
4. Create factory pattern for account creation
5. Add transfer validation and processing
6. Implement interest calculation and monthly fees
7. Add comprehensive error handling

## Month 3: Data & Collections with SQL Server Integration

### Exercise 13: Contact List Manager with SQL Server

**Objective**: Create a contact management system with SQL Server integration using Entity Framework Core.

**Core Requirements**:
- Contact categories (Family, Work, Friends, Other)
- Contact search by multiple criteria
- Contact import/export functionality
- Birthday reminders
- Contact statistics
- Use Entity Framework Core with SQL Server

**Required Interfaces**:
```csharp
public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(int id);
    Task<Contact?> GetContactByEmailAsync(string email);
    Task<IEnumerable<Contact>> GetContactsByCategoryAsync(ContactCategory category);
    Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm);
    Task<IEnumerable<Contact>> GetContactsWithBirthdayThisMonthAsync();
    Task<Contact> AddContactAsync(Contact contact);
    Task<Contact> UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(int id);
    Task<int> GetContactCountAsync();
    Task<IEnumerable<ContactCategory>> GetContactCategoriesAsync();
    Task<Dictionary<ContactCategory, int>> GetContactStatisticsAsync();
}
```

**Contact Model**:
```csharp
public class Contact
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? Phone { get; set; }
    
    [StringLength(100)]
    public string? City { get; set; }
    
    public ContactCategory Category { get; set; }
    
    public DateTime? Birthday { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum ContactCategory
{
    Family,
    Work,
    Friends,
    Other
}
```

**Database Schema**:
```sql
CREATE TABLE Contacts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    City NVARCHAR(100),
    Category NVARCHAR(20) NOT NULL,
    Birthday DATE,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX IX_Contacts_Email ON Contacts(Email);
CREATE INDEX IX_Contacts_Category ON Contacts(Category);
```

**Implementation Steps**:
1. Set up Entity Framework Core with SQL Server
2. Create Contact model with data annotations
3. Implement `IContactRepository` using Entity Framework
4. Add search functionality with LINQ
5. Implement birthday reminder functionality
6. Add contact statistics and reporting
7. Create import/export functionality
8. Add comprehensive error handling

### Exercise 14: Inventory Tracking System with Database

**Objective**: Create an inventory management system for a store with SQL Server integration.

**Core Requirements**:
- Product class with properties (Name, SKU, Price, Category)
- Inventory tracking with quantities
- Low stock alerts
- Product categories
- Sales tracking
- Use Entity Framework Core

**Required Interfaces**:
```csharp
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductBySKUAsync(string sku);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
}

public interface IInventoryService
{
    Task UpdateStockLevelAsync(int productId, int quantity);
    Task<bool> ReserveStockAsync(int productId, int quantity);
    Task ReleaseStockAsync(int productId, int quantity);
    Task<IEnumerable<Product>> GetLowStockAlertsAsync();
    Task<Dictionary<string, int>> GetCategoryStatisticsAsync();
}
```

**Product Model**:
```csharp
public class Product
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string SKU { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;
    
    public int StockQuantity { get; set; }
    
    public int MinimumStockLevel { get; set; } = 10;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
```

**Implementation Steps**:
1. Set up Entity Framework Core with SQL Server
2. Create Product model with data annotations
3. Implement `IProductRepository` using Entity Framework
4. Implement `IInventoryService` with stock management logic
5. Add low stock alert functionality
6. Implement category statistics
7. Add sales tracking functionality
8. Create comprehensive reporting

## Month 4: REST API Development with Dependency Injection

### Exercise 19: Weather Service API with DI

**Objective**: Create a weather service API with mock data using dependency injection and proper REST practices.

**Core Requirements**:
- Weather forecast endpoints
- City-based weather lookup
- Temperature conversion endpoints
- Weather alerts system
- API documentation with Swagger
- Use constructor injection

**Required Interfaces**:
```csharp
public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();
    Task<WeatherForecast?> GetForecastByIdAsync(int id);
    Task<IEnumerable<WeatherForecast>> GetForecastsByCityAsync(string city);
    Task<IEnumerable<WeatherForecast>> GetForecastsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<WeatherForecast> CreateForecastAsync(CreateWeatherForecastRequest request);
    Task<WeatherForecast> UpdateForecastAsync(int id, UpdateWeatherForecastRequest request);
    Task DeleteForecastAsync(int id);
    Task<double> ConvertTemperatureAsync(double temperature, string fromScale, string toScale);
    Task<IEnumerable<WeatherAlert>> GetAlertsByCityAsync(string city);
    Task<WeatherAlert> CreateAlertAsync(WeatherAlert alert);
    Task<Dictionary<string, object>> GetWeatherStatisticsAsync();
}
```

**Weather Models**:
```csharp
public class WeatherForecast
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public double TemperatureC { get; set; }
    public double TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string Description { get; set; } = string.Empty;
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string WindDirection { get; set; } = string.Empty;
    public double Pressure { get; set; }
    public double Visibility { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class WeatherAlert
{
    public int Id { get; set; }
    public string City { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**API Endpoints**:
```
GET /api/weather - Get all forecasts
GET /api/weather/{id} - Get specific forecast
GET /api/weather/city/{city} - Get forecasts by city
GET /api/weather/range?startDate=2024-01-01&endDate=2024-01-31 - Get forecasts by date range
POST /api/weather - Create new forecast
PUT /api/weather/{id} - Update forecast
DELETE /api/weather/{id} - Delete forecast
GET /api/weather/convert?temperature=25&fromScale=C&toScale=F - Convert temperature
GET /api/weather/alerts/{city} - Get alerts by city
POST /api/weather/alerts - Create new alert
GET /api/weather/statistics - Get weather statistics
```

**Implementation Steps**:
1. Set up ASP.NET Core Web API project
2. Create weather models with data annotations
3. Implement `IWeatherService` with mock data
4. Create `WeatherController` with proper HTTP status codes
5. Add Swagger documentation
6. Implement comprehensive error handling
7. Add request/response logging middleware
8. Configure dependency injection in `Program.cs`

### Exercise 20: Blog API with DI and SQL Server

**Objective**: Create a blog API with posts and comments using dependency injection and SQL Server.

**Core Requirements**:
- CRUD operations for posts
- Comment system
- User authentication (basic)
- Post categories and tags
- Search functionality
- Use Entity Framework Core

**Required Interfaces**:
```csharp
public interface IBlogService
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByCategoryAsync(string category);
    Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm);
    Task<Post> CreatePostAsync(CreatePostRequest request);
    Task<Post> UpdatePostAsync(int id, UpdatePostRequest request);
    Task DeletePostAsync(int id);
    Task<Comment> AddCommentAsync(int postId, CreateCommentRequest request);
    Task<IEnumerable<Comment>> GetPostCommentsAsync(int postId);
}

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByCategoryAsync(string category);
    Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm);
    Task<Post> AddPostAsync(Post post);
    Task<Post> UpdatePostAsync(Post post);
    Task DeletePostAsync(int id);
}
```

**Blog Models**:
```csharp
public class Post
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;
    
    public List<string> Tags { get; set; } = new();
    
    public int AuthorId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public List<Comment> Comments { get; set; } = new();
}

public class Comment
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    public int PostId { get; set; }
    
    public int AuthorId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**Implementation Steps**:
1. Set up Entity Framework Core with SQL Server
2. Create Post and Comment models
3. Implement `IPostRepository` using Entity Framework
4. Implement `IBlogService` with business logic
5. Create `BlogController` with proper HTTP status codes
6. Add authentication middleware
7. Implement search functionality with pagination
8. Add comment moderation features

## Month 5: Advanced Dependency Injection & SQL Server

### Exercise 25: File Download Manager with DI and Database Logging

**Objective**: Create a file download manager with progress tracking and database logging using advanced DI patterns.

**Core Requirements**:
- Download multiple files concurrently
- Progress reporting
- Error handling and retry logic
- Download queue management
- File validation
- Log all operations to SQL Server

**Required Interfaces**:
```csharp
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
```

**Download Models**:
```csharp
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
```

**Implementation Steps**:
1. Set up Entity Framework Core for download logging
2. Create DownloadTask and DownloadLog models
3. Implement `IDownloadService` with download management
4. Implement `IFileValidator` for file validation
5. Implement `IDownloadProgressReporter` for progress tracking
6. Implement `IDownloadQueue` for queue management
7. Create background service for download processing
8. Add comprehensive error handling and retry logic
9. Implement database logging for all operations

### Exercise 26: API Client with Error Handling and DI

**Objective**: Create a robust API client for external services using dependency injection and advanced error handling patterns.

**Core Requirements**:
- HTTP client with retry logic
- Comprehensive error handling
- Rate limiting
- Response caching
- Logging and monitoring
- Store API call logs in SQL Server

**Required Interfaces**:
```csharp
public interface IHttpClientService
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, object data);
    Task<HttpResponseMessage> PutAsync(string url, object data);
    Task<HttpResponseMessage> DeleteAsync(string url);
    Task<T?> GetAsync<T>(string url);
    Task<T?> PostAsync<T>(string url, object data);
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
}

public interface IRateLimiter
{
    Task<bool> ShouldAllowRequestAsync(string endpoint);
    Task RecordRequestAsync(string endpoint);
    Task<TimeSpan> GetWaitTimeAsync(string endpoint);
}
```

**API Client Models**:
```csharp
public class ApiCallLog
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
}

public class CircuitBreakerState
{
    public string Endpoint { get; set; } = string.Empty;
    public CircuitBreakerStatus Status { get; set; }
    public int FailureCount { get; set; }
    public DateTime LastFailureTime { get; set; }
    public DateTime OpenUntil { get; set; }
}

public enum CircuitBreakerStatus
{
    Closed,
    Open,
    HalfOpen
}
```

**Implementation Steps**:
1. Set up Entity Framework Core for API call logging
2. Create ApiCallLog and CircuitBreakerState models
3. Implement `IHttpClientService` with HttpClientFactory
4. Implement `ICacheService` with memory caching
5. Implement `IRateLimiter` for rate limiting
6. Add circuit breaker pattern implementation
7. Create comprehensive error handling and retry logic
8. Add request/response logging to database
9. Implement caching strategies

## Month 6: Real-World Projects with Full DI & SQL Server Stack

### Exercise 31: Task Management Application with DI

**Objective**: Create a full-stack task management application using dependency injection and SQL Server with comprehensive features.

**Core Requirements**:
- User authentication with JWT
- Task CRUD operations
- Task categories and priorities
- Due date reminders
- Team collaboration features
- Use Entity Framework Core
- Implement CQRS pattern with MediatR

**Required Interfaces**:
```csharp
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
```

**Task Models**:
```csharp
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
```

**Implementation Steps**:
1. Set up Entity Framework Core with SQL Server
2. Create Task, TaskComment, and TaskAttachment models
3. Implement all service interfaces with business logic
4. Create comprehensive REST API controllers
5. Add JWT authentication and authorization
6. Implement CQRS pattern with MediatR
7. Add real-time notifications with SignalR
8. Implement file upload functionality
9. Add comprehensive search and filtering
10. Create reporting and statistics features

This comprehensive exercise definition provides detailed guidance for implementing each exercise with proper dependency injection, SQL Server integration, and REST API development patterns. 