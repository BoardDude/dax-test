# Automated Testing in C# - Comprehensive Guide

This guide covers automated testing strategies for C# applications with a focus on dependency injection, SQL Server, and REST API testing.

## 🎯 Testing Overview

### **Types of Testing**
1. **Unit Tests**: Test individual components in isolation
2. **Integration Tests**: Test component interactions
3. **API Tests**: Test REST API endpoints
4. **Database Tests**: Test data access layer
5. **End-to-End Tests**: Test complete user workflows

### **Testing Tools & Frameworks**
- **xUnit**: Primary testing framework
- **Moq**: Mocking framework
- **FluentAssertions**: Readable assertions
- **Microsoft.NET.Test.Sdk**: Test discovery and execution
- **Testcontainers**: Database testing
- **WebApplicationFactory**: API testing

## 📚 Unit Testing with Dependency Injection

### **Basic Unit Test Structure**

```csharp
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace YourProject.Tests
{
    public class CalculatorServiceTests
    {
        private readonly ICalculatorService _calculatorService;
        private readonly Mock<ILogger<CalculatorService>> _mockLogger;

        public CalculatorServiceTests()
        {
            _mockLogger = new Mock<ILogger<CalculatorService>>();
            _calculatorService = new CalculatorService(_mockLogger.Object);
        }

        [Fact]
        public void Add_ValidNumbers_ReturnsCorrectSum()
        {
            // Arrange
            double a = 5;
            double b = 3;
            double expected = 8;

            // Act
            double result = _calculatorService.Calculate(a, b, "+");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 3, "+", 8)]
        [InlineData(10, 5, "-", 5)]
        [InlineData(4, 2, "*", 8)]
        [InlineData(10, 2, "/", 5)]
        public void Calculate_ValidOperations_ReturnsCorrectResult(double a, double b, string operation, double expected)
        {
            // Act
            double result = _calculatorService.Calculate(a, b, operation);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Calculate_DivisionByZero_ThrowsException()
        {
            // Arrange
            double a = 10;
            double b = 0;

            // Act & Assert
            var exception = Assert.Throws<DivideByZeroException>(() => 
                _calculatorService.Calculate(a, b, "/"));
            
            exception.Message.Should().Contain("Cannot divide by zero");
        }
    }
}
```

### **Testing with Dependency Injection**

```csharp
public class WeatherServiceTests
{
    private readonly Mock<IWeatherRepository> _mockRepository;
    private readonly Mock<ILogger<WeatherService>> _mockLogger;
    private readonly WeatherService _weatherService;

    public WeatherServiceTests()
    {
        _mockRepository = new Mock<IWeatherRepository>();
        _mockLogger = new Mock<ILogger<WeatherService>>();
        _weatherService = new WeatherService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetForecastById_ValidId_ReturnsForecast()
    {
        // Arrange
        int forecastId = 1;
        var expectedForecast = new WeatherForecast
        {
            Id = forecastId,
            City = "New York",
            TemperatureC = 25,
            Description = "Sunny"
        };

        _mockRepository.Setup(r => r.GetForecastByIdAsync(forecastId))
            .ReturnsAsync(expectedForecast);

        // Act
        var result = await _weatherService.GetForecastByIdAsync(forecastId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedForecast);
        _mockRepository.Verify(r => r.GetForecastByIdAsync(forecastId), Times.Once);
    }

    [Fact]
    public async Task GetForecastById_InvalidId_ReturnsNull()
    {
        // Arrange
        int invalidId = 999;
        _mockRepository.Setup(r => r.GetForecastByIdAsync(invalidId))
            .ReturnsAsync((WeatherForecast?)null);

        // Act
        var result = await _weatherService.GetForecastByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }
}
```

## 🗄️ Database Testing

### **In-Memory Database Testing**

```csharp
public class ContactRepositoryTests
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
    public async Task AddContact_ValidContact_AddsToDatabase()
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
        
        var savedContact = await _context.Contacts.FindAsync(result.Id);
        savedContact.Should().NotBeNull();
        savedContact!.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetContactsByCategory_ValidCategory_ReturnsFilteredContacts()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Name = "John", Email = "john@example.com", Category = ContactCategory.Family },
            new Contact { Name = "Jane", Email = "jane@example.com", Category = ContactCategory.Work },
            new Contact { Name = "Bob", Email = "bob@example.com", Category = ContactCategory.Family }
        };

        await _context.Contacts.AddRangeAsync(contacts);
        await _context.SaveChangesAsync();

        // Act
        var familyContacts = await _repository.GetContactsByCategoryAsync(ContactCategory.Family);

        // Assert
        familyContacts.Should().HaveCount(2);
        familyContacts.Should().OnlyContain(c => c.Category == ContactCategory.Family);
    }

    [Fact]
    public async Task SearchContacts_ValidSearchTerm_ReturnsMatchingContacts()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Name = "John Smith", Email = "john@example.com" },
            new Contact { Name = "Jane Doe", Email = "jane@example.com" },
            new Contact { Name = "Bob Johnson", Email = "bob@example.com" }
        };

        await _context.Contacts.AddRangeAsync(contacts);
        await _context.SaveChangesAsync();

        // Act
        var searchResults = await _repository.SearchContactsAsync("john");

        // Assert
        searchResults.Should().HaveCount(2); // "John Smith" and "Bob Johnson"
        searchResults.Should().Contain(c => c.Name.Contains("John", StringComparison.OrdinalIgnoreCase));
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
```

### **SQL Server Integration Testing with Testcontainers**

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
    public async Task AddContact_WithRealDatabase_SavesCorrectly()
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

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _container.DisposeAsync();
    }
}
```

## 🌐 API Testing

### **WebApplicationFactory for API Testing**

```csharp
public class WeatherApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetForecasts_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/weather");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetForecastById_ValidId_ReturnsForecast()
    {
        // Arrange
        int forecastId = 1;

        // Act
        var response = await _client.GetAsync($"/api/weather/{forecastId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var forecast = JsonSerializer.Deserialize<WeatherForecast>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        forecast.Should().NotBeNull();
        forecast!.Id.Should().Be(forecastId);
    }

    [Fact]
    public async Task GetForecastById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var response = await _client.GetAsync($"/api/weather/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateForecast_ValidData_ReturnsCreated()
    {
        // Arrange
        var forecast = new CreateWeatherForecastRequest
        {
            City = "Test City",
            Date = DateTime.Now,
            TemperatureC = 25,
            Description = "Sunny"
        };

        var json = JsonSerializer.Serialize(forecast);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/weather", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdForecast = JsonSerializer.Deserialize<WeatherForecast>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        createdForecast.Should().NotBeNull();
        createdForecast!.City.Should().Be("Test City");
    }
}
```

### **Custom WebApplicationFactory with Test Configuration**

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace real services with test doubles
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IWeatherService));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<IWeatherService, TestWeatherService>();

            // Use in-memory database for testing
            var dbDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ContactDbContext>));

            if (dbDescriptor != null)
            {
                services.Remove(dbDescriptor);
            }

            services.AddDbContext<ContactDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
        });
    }
}

public class TestWeatherService : IWeatherService
{
    private readonly List<WeatherForecast> _forecasts = new()
    {
        new WeatherForecast
        {
            Id = 1,
            City = "Test City",
            TemperatureC = 25,
            Description = "Sunny"
        }
    };

    public Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync()
    {
        return Task.FromResult(_forecasts.AsEnumerable());
    }

    public Task<WeatherForecast?> GetForecastByIdAsync(int id)
    {
        var forecast = _forecasts.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(forecast);
    }

    // Implement other interface methods...
}
```

## 🔧 Testing Configuration

### **Test Project Structure**

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
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
    <PackageReference Include="Testcontainers.MsSql" Version="3.7.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\YourProject\YourProject.csproj" />
  </ItemGroup>

</Project>
```

### **Test Categories and Organization**

```csharp
[Trait("Category", "Unit")]
public class CalculatorServiceTests
{
    // Unit tests
}

[Trait("Category", "Integration")]
public class ContactRepositoryIntegrationTests
{
    // Integration tests
}

[Trait("Category", "API")]
public class WeatherApiTests
{
    // API tests
}

[Trait("Category", "Database")]
public class DatabaseTests
{
    // Database tests
}
```

## 📊 Test Data Management

### **Test Data Builders**

```csharp
public class ContactBuilder
{
    private Contact _contact = new()
    {
        Name = "Test Contact",
        Email = "test@example.com",
        Phone = "555-0101",
        City = "Test City",
        Category = ContactCategory.Family,
        Birthday = DateTime.Now.AddYears(-30)
    };

    public ContactBuilder WithName(string name)
    {
        _contact.Name = name;
        return this;
    }

    public ContactBuilder WithEmail(string email)
    {
        _contact.Email = email;
        return this;
    }

    public ContactBuilder WithCategory(ContactCategory category)
    {
        _contact.Category = category;
        return this;
    }

    public ContactBuilder WithBirthday(DateTime birthday)
    {
        _contact.Birthday = birthday;
        return this;
    }

    public Contact Build()
    {
        return _contact;
    }
}

// Usage in tests
[Fact]
public async Task AddContact_ValidContact_AddsToDatabase()
{
    // Arrange
    var contact = new ContactBuilder()
        .WithName("John Doe")
        .WithEmail("john@example.com")
        .WithCategory(ContactCategory.Work)
        .Build();

    // Act & Assert...
}
```

### **Test Fixtures**

```csharp
public class TestDataFixture : IDisposable
{
    public List<Contact> TestContacts { get; }
    public List<WeatherForecast> TestForecasts { get; }

    public TestDataFixture()
    {
        TestContacts = new List<Contact>
        {
            new ContactBuilder().WithName("John Doe").WithEmail("john@example.com").Build(),
            new ContactBuilder().WithName("Jane Smith").WithEmail("jane@example.com").Build(),
            new ContactBuilder().WithName("Bob Wilson").WithEmail("bob@example.com").Build()
        };

        TestForecasts = new List<WeatherForecast>
        {
            new WeatherForecast { Id = 1, City = "New York", TemperatureC = 25 },
            new WeatherForecast { Id = 2, City = "Los Angeles", TemperatureC = 30 },
            new WeatherForecast { Id = 3, City = "Chicago", TemperatureC = 20 }
        };
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

public class ContactRepositoryTests : IClassFixture<TestDataFixture>
{
    private readonly TestDataFixture _fixture;

    public ContactRepositoryTests(TestDataFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllContacts_ReturnsAllContacts()
    {
        // Arrange
        // Use _fixture.TestContacts for test data

        // Act & Assert...
    }
}
```

## 🚀 Running Tests

### **Command Line**

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test category
dotnet test --filter "Category=Unit"

# Run tests in parallel
dotnet test --maxcpucount:4

# Run tests with verbose output
dotnet test --verbosity normal
```

### **Visual Studio**

1. **Test Explorer**: View and run tests
2. **Code Coverage**: Analyze test coverage
3. **Live Unit Testing**: Run tests as you code
4. **Test Results**: View detailed test results

### **CI/CD Integration**

```yaml
# GitHub Actions example
name: Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    - name: Upload coverage reports
      uses: codecov/codecov-action@v3
      with:
        file: ./coverage.opencover.xml
```

## 📈 Test Coverage and Quality

### **Coverage Analysis**

```csharp
[Fact]
[Trait("Category", "Coverage")]
public void Calculate_AllOperations_CoverageTest()
{
    // Test all arithmetic operations for coverage
    var operations = new[] { "+", "-", "*", "/" };
    
    foreach (var operation in operations)
    {
        var result = _calculatorService.Calculate(10, 5, operation);
        result.Should().NotBe(0); // Basic validation
    }
}
```

### **Performance Testing**

```csharp
[Fact]
[Trait("Category", "Performance")]
public async Task SearchContacts_LargeDataset_PerformanceTest()
{
    // Arrange
    var largeDataset = Enumerable.Range(1, 10000)
        .Select(i => new ContactBuilder()
            .WithName($"Contact {i}")
            .WithEmail($"contact{i}@example.com")
            .Build())
        .ToList();

    // Act
    var stopwatch = Stopwatch.StartNew();
    var results = await _repository.SearchContactsAsync("Contact");
    stopwatch.Stop();

    // Assert
    stopwatch.ElapsedMilliseconds.Should().BeLessThan(100); // Should complete within 100ms
    results.Should().HaveCountGreaterThan(0);
}
```

## 🎯 Best Practices

### **Test Naming Convention**
```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Test implementation
}

// Examples:
public void Calculate_ValidNumbers_ReturnsCorrectSum()
public void GetForecastById_InvalidId_ReturnsNull()
public void AddContact_ValidContact_AddsToDatabase()
```

### **AAA Pattern (Arrange, Act, Assert)**
```csharp
[Fact]
public void Calculate_ValidOperation_ReturnsCorrectResult()
{
    // Arrange
    double a = 5;
    double b = 3;
    double expected = 8;

    // Act
    double result = _calculatorService.Calculate(a, b, "+");

    // Assert
    result.Should().Be(expected);
}
```

### **Test Isolation**
```csharp
public class IsolatedTests : IDisposable
{
    private readonly TestDatabase _testDb;

    public IsolatedTests()
    {
        _testDb = new TestDatabase();
        _testDb.Initialize();
    }

    [Fact]
    public void TestMethod()
    {
        // Each test gets a clean database
    }

    public void Dispose()
    {
        _testDb.Cleanup();
    }
}
```

This comprehensive testing guide provides everything needed to implement automated testing for C# applications with dependency injection, SQL Server, and REST APIs. The examples cover unit testing, integration testing, API testing, and database testing with real-world patterns and best practices. 