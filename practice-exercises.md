# Practice Exercises by Month

This document contains practice exercises for each month of the C# curriculum, with a focus on **Dependency Injection**, **SQL Server**, and **REST API** development. Complete these exercises to reinforce your learning and build practical skills.

## 🎯 How to Use This Document

### **Before Starting Each Exercise:**
1. **Read the requirements carefully** - understand what you need to build
2. **Plan your DI architecture** - identify services and interfaces needed
3. **Design your database schema** - plan SQL Server tables (if applicable)
4. **Set up your project structure** - organize code with proper DI container
5. **Review the example code** - check the corresponding example in the repository

### **While Working:**
1. **Start with interfaces** - define your service contracts first
2. **Configure DI container** - register all services properly
3. **Test frequently** - use unit tests with mocked dependencies
4. **Use version control** - commit your progress regularly
5. **Document your DI setup** - explain service registrations

### **After Completion:**
1. **Review your DI implementation** - ensure proper service lifetimes
2. **Optimize database queries** - use Entity Framework efficiently
3. **Add error handling** - make it robust with proper DI
4. **Test edge cases** - handle unexpected inputs
5. **Document your solution** - explain your DI and API design

## 📚 Monthly Exercises

## Month 1: Foundations & Dependency Injection Basics

### Week 1-2: Introduction to Programming & C# Basics

#### Exercise 1: Personal Information Display with DI
**Difficulty**: ⭐⭐  
**Focus**: Basic DI setup, constructor injection, service interfaces

**Requirements:**
- Use different data types (string, int)
- Use string interpolation
- Handle user input validation
- Implement `IUserInputService` interface
- Use constructor injection for services
- Create `IDisplayService` interface for output formatting

**Implementation Tips:**
- Start by defining your interfaces before implementing classes
- Use `Microsoft.Extensions.DependencyInjection` for DI container
- Validate age input (0-120 range)
- Handle null/empty input gracefully

**Example Structure:**
```csharp
public interface IUserInputService
{
    string GetStringInput(string prompt);
    int GetIntInput(string prompt);
    bool ValidateAge(int age);
}
```

#### Exercise 2: Temperature Converter with Service Pattern
**Difficulty**: ⭐⭐  
**Focus**: Service pattern, strategy pattern, multiple implementations

**Requirements:**
- Support all three temperature scales (Celsius, Fahrenheit, Kelvin)
- Use proper mathematical formulas
- Display results with appropriate decimal places
- Handle invalid input gracefully
- Create `ITemperatureConverter` interface
- Implement different converter strategies

**Implementation Tips:**
- Use switch expressions for temperature conversions
- Implement strategy pattern for different conversion algorithms
- Add input validation for temperature scales
- Use string interpolation for formatted output

#### Exercise 3: Simple Calculator with DI Container
**Difficulty**: ⭐⭐  
**Focus**: DI container setup, service registration, error handling

**Requirements:**
- Support addition, subtraction, multiplication, division
- Handle division by zero
- Use switch statements for operation selection
- Display formatted results
- Create `ICalculatorService` interface
- Use Microsoft.Extensions.DependencyInjection

**Implementation Tips:**
- Register services with appropriate lifetimes
- Use exception handling for division by zero
- Validate numeric input
- Implement operation validation

### Week 3-4: Control Structures & Functions with DI

#### Exercise 4: Number Guessing Game with Services
**Difficulty**: ⭐⭐⭐  
**Focus**: Random number generation, game state management, DI services

**Requirements:**
- Generate random number between 1-100
- Give hints (higher/lower)
- Count number of attempts
- Ask if player wants to play again
- Track high score (lowest attempts)
- Create `IGameService` and `IRandomNumberGenerator` interfaces

**Implementation Tips:**
- Use `Random` class for number generation
- Implement game state management
- Track statistics across multiple games
- Use scoped services for game sessions

#### Exercise 5: Grade Calculator with Repository Pattern
**Difficulty**: ⭐⭐⭐  
**Focus**: Repository pattern, data persistence, service layer

**Requirements:**
- Input multiple test scores
- Calculate average
- Assign letter grade (A, B, C, D, F)
- Handle grade ranges (A: 90-100, B: 80-89, etc.)
- Display detailed breakdown
- Create `IGradeRepository` interface
- Implement in-memory and file-based repositories

**Implementation Tips:**
- Use repository pattern for data access
- Implement different storage strategies
- Add grade validation logic
- Use strategy pattern for different grading algorithms

#### Exercise 6: Menu System with Service Layer
**Difficulty**: ⭐⭐⭐  
**Focus**: Service layer pattern, menu management, order processing

**Requirements:**
- Display menu items with prices
- Allow user to select items
- Calculate total bill
- Apply tax and tip
- Handle invalid selections
- Create `IMenuService` and `IOrderService` interfaces
- Use service layer pattern

**Implementation Tips:**
- Separate menu display from order processing
- Implement tax calculation service
- Add order validation
- Use decimal for currency calculations

## Month 2: Object-Oriented Programming & Advanced DI

### Week 1-2: Classes, Objects, and Methods with DI

#### Exercise 7: Library Book Management with DI
**Difficulty**: ⭐⭐⭐  
**Focus**: Repository pattern, entity management, CRUD operations

**Requirements:**
- Book class with properties (Title, Author, ISBN, Available)
- Library class to manage books
- Methods to add, remove, search, and check out books
- Display library catalog
- Create `IBookRepository` and `ILibraryService` interfaces
- Use scoped and singleton services

**Implementation Tips:**
- Use repository pattern for data access
- Implement book validation
- Add search functionality
- Handle book availability status

#### Exercise 8: Bank Account System with DI Container
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Inheritance, polymorphism, factory pattern, transaction management

**Requirements:**
- Base Account class
- Savings and Checking account classes
- Different interest rates and fees
- Transaction history
- Account transfer functionality
- Create `IAccountService` and `ITransactionRepository` interfaces
- Use factory pattern for account creation

**Implementation Tips:**
- Use abstract base class for Account
- Implement different account types with inheritance
- Add transaction logging
- Implement transfer validation

#### Exercise 9: Student Grade Management with Services
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Complex business logic, service composition, data aggregation

**Requirements:**
- Student class with properties
- Course class with assignments
- Grade calculation methods
- GPA calculation
- Academic standing determination
- Create `IStudentService` and `IGradeCalculationService` interfaces
- Use strategy pattern for different grading algorithms

**Implementation Tips:**
- Implement weighted grade calculations
- Add academic standing rules
- Use strategy pattern for different grading systems
- Implement GPA calculation service

### Week 3-4: Inheritance, Polymorphism, and Interfaces with DI

#### Exercise 10: Shape Hierarchy with DI
**Difficulty**: ⭐⭐⭐  
**Focus**: Abstract classes, virtual methods, factory pattern

**Requirements:**
- Abstract Shape class
- Circle, Rectangle, Triangle classes
- Virtual methods for area/perimeter
- Shape collection with polymorphic behavior
- Display shape information
- Create `IShapeCalculator` interface
- Use factory pattern for shape creation

**Implementation Tips:**
- Use abstract base class with virtual methods
- Implement area and perimeter calculations
- Add shape validation
- Use factory pattern for shape instantiation

#### Exercise 11: Animal Classification System with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Interfaces, decorator pattern, behavior composition

**Requirements:**
- Base Animal class
- Mammal, Bird, Fish classes
- Interface for flying/swimming behavior
- Animal zoo management
- Animal sound simulation
- Create `IAnimalService` and `IZooManager` interfaces
- Use decorator pattern for animal behaviors

**Implementation Tips:**
- Use interfaces for behaviors (IFlyable, ISwimmable)
- Implement decorator pattern for behavior composition
- Add animal sound simulation
- Create zoo management system

#### Exercise 12: Employee Management System with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Complex inheritance hierarchy, salary calculations, performance management

**Requirements:**
- Base Employee class
- Manager, Developer, Intern classes
- Salary calculation with bonuses
- Performance review system
- Department assignment
- Create `IEmployeeService` and `ISalaryCalculator` interfaces
- Use strategy pattern for salary calculations

**Implementation Tips:**
- Implement different salary calculation strategies
- Add performance review system
- Use factory pattern for employee creation
- Implement department management

## Month 3: Data & Collections with SQL Server Integration

### Week 1-2: Arrays, Lists, and Collections with Database

#### Exercise 13: Contact List Manager with SQL Server
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Entity Framework Core, SQL Server integration, CRUD operations

**Requirements:**
- Contact categories (Family, Work, Friends)
- Contact search by multiple criteria
- Contact import/export functionality
- Birthday reminders
- Contact statistics
- Create `IContactRepository` interface
- Use Entity Framework Core with SQL Server
- Implement repository pattern

**Implementation Tips:**
- Set up Entity Framework Core with SQL Server
- Create Contact model with data annotations
- Implement repository pattern
- Add search functionality with LINQ
- Use async/await for database operations

**Database Schema:**
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
```

#### Exercise 14: Inventory Tracking System with Database
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Complex entity relationships, inventory management, low stock alerts

**Requirements:**
- Product class with properties
- Inventory tracking with quantities
- Low stock alerts
- Product categories
- Sales tracking
- Create `IProductRepository` and `IInventoryService` interfaces
- Use Entity Framework Core
- Implement unit of work pattern

**Implementation Tips:**
- Create Product and Inventory entities
- Implement low stock alert system
- Add sales tracking functionality
- Use unit of work pattern for transactions

#### Exercise 15: Word Frequency Counter with Database Storage
**Difficulty**: ⭐⭐⭐  
**Focus**: Text processing, database storage, LINQ aggregation

**Requirements:**
- Read text from file or input
- Count word occurrences
- Display top N most frequent words
- Ignore common words (the, and, or, etc.)
- Generate word cloud data
- Store results in SQL Server
- Create `ITextAnalyzerService` interface
- Use async/await for database operations

**Implementation Tips:**
- Use LINQ for word frequency counting
- Implement stop word filtering
- Store analysis results in database
- Add text preprocessing

### Week 3-4: LINQ and Data Processing with SQL Server

#### Exercise 16: Data Analysis Tool with Database
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: LINQ to Entities, data analysis, reporting

**Requirements:**
- Load data from CSV file to SQL Server
- Calculate statistics (mean, median, mode)
- Generate reports using LINQ
- Data filtering and sorting
- Export analysis results
- Create `IDataAnalysisService` interface
- Use Entity Framework Core with LINQ
- Implement data transfer objects (DTOs)

**Implementation Tips:**
- Use LINQ for statistical calculations
- Implement data import from CSV
- Create DTOs for report data
- Add data validation

#### Exercise 17: File Processing Utility with Database Logging
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: File processing, database logging, batch operations

**Requirements:**
- Read multiple text files
- Perform operations (count lines, words, characters)
- Generate summary reports
- File comparison functionality
- Batch processing
- Log all operations to SQL Server
- Create `IFileProcessorService` interface
- Use Entity Framework Core for logging

**Implementation Tips:**
- Implement file processing pipeline
- Add database logging for all operations
- Use async/await for file operations
- Implement batch processing

#### Exercise 18: Report Generator with Database Backend
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Complex reporting, stored procedures, data aggregation

**Requirements:**
- Process sales/employee data
- Generate different report types
- Use LINQ for data aggregation
- Export to different formats
- Customizable report templates
- Store report configurations in SQL Server
- Create `IReportService` interface
- Use stored procedures for complex queries

**Implementation Tips:**
- Implement report template system
- Use stored procedures for complex queries
- Add report scheduling
- Implement export functionality

## Month 4: REST API Development with Dependency Injection

### Week 1-2: ASP.NET Core Web APIs with DI

#### Exercise 19: Weather Service API with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: REST API design, dependency injection, HTTP status codes

**Requirements:**
- Weather forecast endpoints
- City-based weather lookup
- Temperature conversion endpoints
- Weather alerts system
- API documentation with Swagger
- Create `IWeatherService` interface
- Use constructor injection
- Implement service layer pattern
- Add middleware for logging

**Implementation Tips:**
- Use proper HTTP status codes
- Implement comprehensive error handling
- Add request/response logging
- Use Swagger for API documentation

**API Endpoints:**
```
GET /api/weather - Get all forecasts
GET /api/weather/{id} - Get specific forecast
GET /api/weather/city/{city} - Get forecasts by city
POST /api/weather - Create new forecast
PUT /api/weather/{id} - Update forecast
DELETE /api/weather/{id} - Delete forecast
```

#### Exercise 20: Blog API with DI and SQL Server
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Complex API design, database relationships, authentication

**Requirements:**
- CRUD operations for posts
- Comment system
- User authentication (basic)
- Post categories and tags
- Search functionality
- Use Entity Framework Core
- Create `IBlogService` and `IPostRepository` interfaces
- Implement repository pattern
- Add API versioning

**Implementation Tips:**
- Implement proper authentication
- Add comment moderation
- Use API versioning
- Implement search with pagination

#### Exercise 21: E-commerce API with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Complex business logic, shopping cart, order management

**Requirements:**
- Product catalog
- Shopping cart functionality
- Order management
- User profiles
- Payment processing simulation
- Create `IProductService`, `ICartService`, `IOrderService` interfaces
- Use scoped services for user sessions
- Implement unit of work pattern
- Add request/response logging

**Implementation Tips:**
- Implement shopping cart with session management
- Add order validation
- Use unit of work for transactions
- Implement payment processing simulation

### Week 3-4: Database Integration with Entity Framework and DI

#### Exercise 22: Book Management System with SQL Server and DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Complete CRUD operations, database relationships, advanced queries

**Requirements:**
- Entity Framework Core setup with SQL Server
- Book and Author entities
- Database migrations
- CRUD operations
- Search and filtering
- Create `IBookRepository`, `IAuthorRepository` interfaces
- Use repository pattern with unit of work
- Implement service layer
- Add database transaction management

**Implementation Tips:**
- Set up Entity Framework Core with SQL Server
- Create proper entity relationships
- Implement database migrations
- Add advanced search functionality

#### Exercise 23: User Registration and Authentication with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Authentication, password hashing, JWT tokens

**Requirements:**
- User registration and login
- Password hashing
- JWT token authentication
- User roles and permissions
- Profile management
- Use SQL Server for user storage
- Create `IUserService`, `IAuthService` interfaces
- Implement JWT service with DI
- Add role-based authorization

**Implementation Tips:**
- Use BCrypt for password hashing
- Implement JWT token generation
- Add role-based authorization
- Implement user profile management

#### Exercise 24: E-commerce Product Catalog with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Product management, image handling, category management

**Requirements:**
- Product and category entities
- Image upload functionality
- Product search and filtering
- Shopping cart persistence
- Order tracking
- Use Entity Framework Core
- Create `IProductService`, `ICategoryService` interfaces
- Implement file service for image uploads
- Add caching with DI

**Implementation Tips:**
- Implement image upload service
- Add product search with filters
- Implement caching strategy
- Add category management

## Month 5: Advanced Dependency Injection & SQL Server

### Week 1-2: Advanced DI Patterns & SQL Server

#### Exercise 25: File Download Manager with DI and Database Logging
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Background services, queue management, database logging

**Requirements:**
- Download multiple files concurrently
- Progress reporting
- Error handling and retry logic
- Download queue management
- File validation
- Log all operations to SQL Server
- Create `IDownloadService`, `IFileValidator` interfaces
- Use background services with DI
- Implement queue pattern

**Implementation Tips:**
- Implement background service for downloads
- Add progress reporting
- Use queue for download management
- Implement retry logic

#### Exercise 26: API Client with Error Handling and DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: HTTP client patterns, error handling, circuit breaker

**Requirements:**
- HTTP client with retry logic
- Comprehensive error handling
- Rate limiting
- Response caching
- Logging and monitoring
- Store API call logs in SQL Server
- Create `IHttpClientService`, `ICacheService` interfaces
- Use HttpClientFactory with DI
- Implement circuit breaker pattern

**Implementation Tips:**
- Use HttpClientFactory for HTTP clients
- Implement circuit breaker pattern
- Add rate limiting
- Implement response caching

#### Exercise 27: Background Task Processor with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Background services, task processing, health monitoring

**Requirements:**
- Task queue management
- Background service implementation
- Task status tracking
- Error recovery
- Performance monitoring
- Store task data in SQL Server
- Create `ITaskProcessor`, `ITaskQueue` interfaces
- Use hosted services with DI
- Implement health checks

**Implementation Tips:**
- Implement background hosted service
- Add task queue management
- Implement health checks
- Add performance monitoring

### Week 3-4: Design Patterns & Best Practices with DI

#### Exercise 28: Repository Pattern Implementation with SQL Server
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Generic repository, unit of work, database transactions

**Requirements:**
- Generic repository interface
- Entity-specific repositories
- Unit of work pattern
- Dependency injection
- Unit testing
- Use Entity Framework Core
- Create `IGenericRepository<T>` interface
- Implement unit of work with DI
- Add database transaction management

**Implementation Tips:**
- Create generic repository interface
- Implement unit of work pattern
- Add database transaction management
- Write unit tests with mocked repositories

#### Exercise 29: Factory Pattern Application with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Factory patterns, configuration-driven creation, object pooling

**Requirements:**
- Abstract factory for different product types
- Factory method for object creation
- Configuration-driven factories
- Object pooling
- Performance optimization
- Create `IProductFactory`, `IObjectPool` interfaces
- Use Microsoft.Extensions.DependencyInjection
- Implement configuration-based factories

**Implementation Tips:**
- Implement abstract factory pattern
- Add configuration-driven factory creation
- Implement object pooling
- Optimize for performance

#### Exercise 30: Observer Pattern System with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Event-driven architecture, observer pattern, event sourcing

**Requirements:**
- Event publishing and subscription
- Multiple observer types
- Event filtering
- Asynchronous event processing
- Event logging to SQL Server
- Create `IEventPublisher`, `IEventSubscriber` interfaces
- Use MediatR library with DI
- Implement event sourcing pattern

**Implementation Tips:**
- Use MediatR for event handling
- Implement event sourcing
- Add event filtering
- Store events in SQL Server

## Month 6: Real-World Projects with Full DI & SQL Server Stack

### Week 1-2: Building a Full-Stack Application with DI

#### Exercise 31: Task Management Application with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Full-stack application, complex business logic, real-time features

**Requirements:**
- User authentication with JWT
- Task CRUD operations
- Task categories and priorities
- Due date reminders
- Team collaboration features
- Use Entity Framework Core
- Create comprehensive service interfaces
- Implement CQRS pattern with MediatR
- Add real-time notifications with SignalR

**Implementation Tips:**
- Implement CQRS pattern
- Add real-time notifications
- Implement team collaboration
- Add due date reminders

#### Exercise 32: Personal Finance Tracker with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Financial calculations, reporting, data visualization

**Requirements:**
- Income and expense tracking
- Budget management
- Financial reports and charts
- Category-based analysis
- Export functionality
- Use Entity Framework Core
- Create `IFinanceService`, `IBudgetService` interfaces
- Implement reporting service with DI
- Add data visualization with charts

**Implementation Tips:**
- Implement financial calculations
- Add budget management
- Create financial reports
- Add data visualization

#### Exercise 33: Social Media Clone with DI
**Difficulty**: ⭐⭐⭐⭐⭐  
**Focus**: Social features, real-time updates, complex relationships

**Requirements:**
- User profiles and posts
- Friend/follow system
- News feed
- Like and comment functionality
- Real-time notifications
- Use Entity Framework Core
- Create `IUserService`, `IPostService` interfaces
- Implement feed algorithm with DI
- Add SignalR for real-time features

**Implementation Tips:**
- Implement feed algorithm
- Add real-time notifications
- Implement friend/follow system
- Add like and comment functionality

### Week 3-4: Deployment & DevOps Basics with DI

#### Exercise 34: Portfolio Website Deployment with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Cloud deployment, environment configuration, monitoring

**Requirements:**
- Azure/AWS deployment
- CI/CD pipeline setup
- Environment configuration
- Monitoring and logging
- SSL certificate setup
- Use SQL Server in the cloud
- Configure DI for different environments
- Implement health checks
- Add application insights

**Implementation Tips:**
- Set up CI/CD pipeline
- Configure environment-specific settings
- Add health checks
- Implement monitoring

#### Exercise 35: Automated Testing Suite with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Unit testing, integration testing, test data management

**Requirements:**
- Unit tests for all business logic
- Integration tests for APIs
- End-to-end testing
- Test coverage reporting
- Automated test execution
- Use in-memory database for testing
- Mock services with DI
- Implement test data builders
- Add performance testing

**Implementation Tips:**
- Write comprehensive unit tests
- Use in-memory database for testing
- Mock dependencies properly
- Add performance tests

#### Exercise 36: Docker Containerization with DI
**Difficulty**: ⭐⭐⭐⭐  
**Focus**: Containerization, orchestration, configuration management

**Requirements:**
- Multi-stage Docker builds
- Docker Compose for multi-service apps
- Environment-specific configurations
- Health checks
- Container orchestration basics
- Use SQL Server in containers
- Configure DI for containerized apps
- Implement configuration management
- Add container monitoring

**Implementation Tips:**
- Create multi-stage Docker builds
- Set up Docker Compose
- Configure health checks
- Add container monitoring

## 📊 Progress Tracking

Keep track of your progress with this checklist:

### Month 1
- [ ] Exercise 1: Personal Information Display with DI
- [ ] Exercise 2: Temperature Converter with Service Pattern
- [ ] Exercise 3: Simple Calculator with DI Container
- [ ] Exercise 4: Number Guessing Game with Services
- [ ] Exercise 5: Grade Calculator with Repository Pattern
- [ ] Exercise 6: Menu System with Service Layer

### Month 2
- [ ] Exercise 7: Library Book Management with DI
- [ ] Exercise 8: Bank Account System with DI Container
- [ ] Exercise 9: Student Grade Management with Services
- [ ] Exercise 10: Shape Hierarchy with DI
- [ ] Exercise 11: Animal Classification System with DI
- [ ] Exercise 12: Employee Management System with DI

### Month 3
- [ ] Exercise 13: Contact List Manager with SQL Server
- [ ] Exercise 14: Inventory Tracking System with Database
- [ ] Exercise 15: Word Frequency Counter with Database Storage
- [ ] Exercise 16: Data Analysis Tool with Database
- [ ] Exercise 17: File Processing Utility with Database Logging
- [ ] Exercise 18: Report Generator with Database Backend

### Month 4
- [ ] Exercise 19: Weather Service API with DI
- [ ] Exercise 20: Blog API with DI and SQL Server
- [ ] Exercise 21: E-commerce API with DI
- [ ] Exercise 22: Book Management System with SQL Server and DI
- [ ] Exercise 23: User Registration and Authentication with DI
- [ ] Exercise 24: E-commerce Product Catalog with DI

### Month 5
- [ ] Exercise 25: File Download Manager with DI and Database Logging
- [ ] Exercise 26: API Client with Error Handling and DI
- [ ] Exercise 27: Background Task Processor with DI
- [ ] Exercise 28: Repository Pattern Implementation with SQL Server
- [ ] Exercise 29: Factory Pattern Application with DI
- [ ] Exercise 30: Observer Pattern System with DI

### Month 6
- [ ] Exercise 31: Task Management Application with DI
- [ ] Exercise 32: Personal Finance Tracker with DI
- [ ] Exercise 33: Social Media Clone with DI
- [ ] Exercise 34: Portfolio Website Deployment with DI
- [ ] Exercise 35: Automated Testing Suite with DI
- [ ] Exercise 36: Docker Containerization with DI

## 🏆 Challenge Yourself

Once you've completed the basic exercises, try these advanced challenges:

1. **Advanced DI Patterns** - Implement complex DI scenarios with multiple service lifetimes
2. **SQL Server Performance** - Optimize database queries and implement caching strategies
3. **API Security** - Add comprehensive authentication and authorization to your APIs
4. **Microservices Architecture** - Break down applications into microservices with DI
5. **Real-time Applications** - Add SignalR with DI for real-time features
6. **Event Sourcing** - Implement event sourcing patterns with SQL Server
7. **CQRS Implementation** - Use MediatR and DI for CQRS pattern
8. **API Gateway** - Create an API gateway with DI and routing

## 🔧 Key Technologies to Master

### Dependency Injection
- Microsoft.Extensions.DependencyInjection
- Service lifetimes (Singleton, Scoped, Transient)
- Constructor injection vs property injection
- Factory patterns with DI
- Service provider and service locator patterns

### SQL Server & Entity Framework Core
- Entity Framework Core setup and configuration
- Code-first and database-first approaches
- LINQ to Entities
- Database migrations
- Connection string management
- Stored procedures and raw SQL
- Database transactions and concurrency

### REST API Development
- ASP.NET Core Web API
- Controller design and routing
- Model binding and validation
- API versioning
- Swagger/OpenAPI documentation
- Middleware implementation
- Authentication and authorization
- API response formatting

## 📝 Exercise Submission Guidelines

### **Code Quality Standards:**
- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Implement proper error handling
- Write unit tests for business logic

### **DI Implementation:**
- Use constructor injection
- Register services with appropriate lifetimes
- Create interfaces for all services
- Use dependency injection container properly
- Avoid service locator pattern

### **Database Design:**
- Use proper data annotations
- Implement repository pattern
- Use async/await for database operations
- Add proper indexes for performance
- Implement data validation

### **API Design:**
- Use proper HTTP status codes
- Implement comprehensive error handling
- Add request/response logging
- Use Swagger for documentation
- Implement proper validation

## 🎯 Success Metrics

### **By Month 3:**
- ✅ Comfortable with basic DI patterns
- ✅ Can create simple SQL Server databases
- ✅ Understand Entity Framework Core basics

### **By Month 4:**
- ✅ Can build REST APIs with proper DI
- ✅ Understand HTTP status codes and error handling
- ✅ Can implement basic authentication

### **By Month 6:**
- ✅ Can build full-stack applications
- ✅ Understand advanced DI patterns
- ✅ Can deploy applications to cloud platforms
- ✅ Can write comprehensive tests

Remember: The goal is to become proficient in building robust, scalable applications using modern C# development practices with dependency injection, SQL Server, and REST APIs! 