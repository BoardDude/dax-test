# 6-Month C# Software Development Curriculum

A comprehensive curriculum focused on **Dependency Injection**, **SQL Server**, and **REST API** development using modern C# practices.

## 🎯 Curriculum Overview

This curriculum is designed to take you from C# basics to building full-stack applications with enterprise-level patterns. Each month builds upon the previous, ensuring a solid foundation in modern C# development practices.

### **Core Focus Areas**
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection and advanced DI patterns
- **SQL Server**: Entity Framework Core, LINQ, and database design
- **REST API**: ASP.NET Core Web APIs with proper HTTP practices

## 📚 Monthly Breakdown

### **Month 1: Foundations & Dependency Injection Basics**
**Focus**: Introduction to DI patterns and service-oriented architecture

#### Week 1-2: Introduction to Programming & C# Basics
- **Exercise 1**: Personal Information Display with DI
- **Exercise 2**: Temperature Converter with Service Pattern  
- **Exercise 3**: Simple Calculator with DI Container

#### Week 3-4: Control Structures & Functions with DI
- **Exercise 4**: Number Guessing Game with Services
- **Exercise 5**: Grade Calculator with Repository Pattern
- **Exercise 6**: Menu System with Service Layer

**Key Concepts**: Constructor injection, service interfaces, Microsoft.Extensions.DependencyInjection

### **Month 2: Object-Oriented Programming & Advanced DI**
**Focus**: Advanced DI patterns and object-oriented design

#### Week 1-2: Classes, Objects, and Methods with DI
- **Exercise 7**: Library Book Management with DI
- **Exercise 8**: Bank Account System with DI Container
- **Exercise 9**: Student Grade Management with Services

#### Week 3-4: Inheritance, Polymorphism, and Interfaces with DI
- **Exercise 10**: Shape Hierarchy with DI
- **Exercise 11**: Animal Classification System with DI
- **Exercise 12**: Employee Management System with DI

**Key Concepts**: Service lifetimes, factory patterns, strategy patterns, repository pattern

### **Month 3: Data & Collections with SQL Server Integration**
**Focus**: Database integration with Entity Framework Core

#### Week 1-2: Arrays, Lists, and Collections with Database
- **Exercise 13**: Contact List Manager with SQL Server
- **Exercise 14**: Inventory Tracking System with Database
- **Exercise 15**: Word Frequency Counter with Database Storage

#### Week 3-4: LINQ and Data Processing with SQL Server
- **Exercise 16**: Data Analysis Tool with Database
- **Exercise 17**: File Processing Utility with Database Logging
- **Exercise 18**: Report Generator with Database Backend

**Key Concepts**: Entity Framework Core, LINQ to Entities, async/await, data annotations

### **Month 4: REST API Development with Dependency Injection**
**Focus**: Building RESTful APIs with proper DI patterns

#### Week 1-2: ASP.NET Core Web APIs with DI
- **Exercise 19**: Weather Service API with DI
- **Exercise 20**: Blog API with DI and SQL Server
- **Exercise 21**: E-commerce API with DI

#### Week 3-4: Database Integration with Entity Framework and DI
- **Exercise 22**: Book Management System with SQL Server and DI
- **Exercise 23**: User Registration and Authentication with DI
- **Exercise 24**: E-commerce Product Catalog with DI

**Key Concepts**: Controller design, HTTP status codes, Swagger documentation, middleware

### **Month 5: Advanced Dependency Injection & SQL Server**
**Focus**: Advanced patterns and enterprise-level development

#### Week 1-2: Advanced DI Patterns & SQL Server
- **Exercise 25**: File Download Manager with DI and Database Logging
- **Exercise 26**: API Client with Error Handling and DI
- **Exercise 27**: Background Task Processor with DI

#### Week 3-4: Design Patterns & Best Practices with DI
- **Exercise 28**: Repository Pattern Implementation with SQL Server
- **Exercise 29**: Factory Pattern Application with DI
- **Exercise 30**: Observer Pattern System with DI

**Key Concepts**: Background services, queue patterns, advanced service lifetimes, error handling

### **Month 6: Real-World Projects with Full DI & SQL Server Stack**
**Focus**: Full-stack applications with comprehensive architecture

#### Week 1-2: Building a Full-Stack Application with DI
- **Exercise 31**: Task Management Application with DI
- **Exercise 32**: Personal Finance Tracker with DI
- **Exercise 33**: Social Media Clone with DI

#### Week 3-4: Deployment & DevOps Basics with DI
- **Exercise 34**: Portfolio Website Deployment with DI
- **Exercise 35**: Automated Testing Suite with DI
- **Exercise 36**: Docker Containerization with DI

**Key Concepts**: Microservices, CQRS, event sourcing, containerization, CI/CD

## 🛠️ Example Code Projects

### **Month 1 Examples**
- **PersonalInfoApp**: Basic DI with user input and display services
- **TemperatureConverter**: Service pattern with temperature conversion strategies
- **Calculator**: DI container with arithmetic operations

### **Month 2 Examples**
- **LibraryManagement**: Repository pattern with book management system
- Demonstrates scoped and singleton services
- Factory patterns for object creation

### **Month 3 Examples**
- **ContactManager**: Complete Entity Framework Core setup with SQL Server
- Models with data annotations and validation
- Repository pattern with async/await operations
- Database logging and statistics

### **Month 4 Examples**
- **WeatherApi**: Comprehensive REST API with dependency injection
- Multiple service interfaces for different concerns
- Proper HTTP status codes and error handling
- Swagger documentation and middleware configuration

### **Month 5 Examples**
- **FileDownloadManager**: Advanced DI with multiple service lifetimes
- Background services and queue management
- Comprehensive logging and error handling
- Service interfaces for validation and progress reporting

### **Month 6 Examples**
- **TaskManagementApp**: Full-stack application with comprehensive architecture
- Multiple service interfaces for different concerns
- Advanced search and notification patterns
- File upload handling and validation

## 🔧 Key Technologies Covered

### **Dependency Injection**
- Microsoft.Extensions.DependencyInjection
- Service lifetimes (Singleton, Scoped, Transient)
- Constructor injection vs property injection
- Factory patterns with DI
- Service provider and service locator patterns

### **SQL Server & Entity Framework Core**
- Entity Framework Core setup and configuration
- Code-first and database-first approaches
- LINQ to Entities
- Database migrations
- Connection string management
- Stored procedures and raw SQL
- Database transactions and concurrency

### **REST API Development**
- ASP.NET Core Web API
- Controller design and routing
- Model binding and validation
- API versioning
- Swagger/OpenAPI documentation
- Middleware implementation
- Authentication and authorization
- API response formatting

## 📁 Project Structure

```
dax-test/
├── month1/
│   ├── week1/
│   │   ├── PersonalInfoApp/
│   │   ├── TemperatureConverter/
│   │   └── Calculator/
│   └── week2/
├── month2/
│   ├── week1/
│   │   └── LibraryManagement/
│   └── week2/
├── month3/
│   ├── week1/
│   │   └── ContactManager/
│   │       ├── Models/
│   │       ├── Data/
│   │       └── Services/
│   └── week2/
├── month4/
│   ├── week1/
│   │   └── WeatherApi/
│   │       ├── Models/
│   │       ├── Services/
│   │       └── Controllers/
│   └── week2/
├── month5/
│   ├── week1/
│   │   └── FileDownloadManager/
│   │       ├── Models/
│   │       └── Services/
│   └── week2/
├── month6/
│   ├── week1/
│   │   └── TaskManagementApp/
│   │       ├── Models/
│   │       ├── Services/
│   │       └── Controllers/
│   └── week2/
├── practice-exercises.md
├── setup-guide.md
└── README.md
```

## 🚀 Getting Started

### **Prerequisites**
- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or Developer Edition)
- Visual Studio 2022 or VS Code
- Git for version control

### **Setup Instructions**
1. Clone this repository
2. Install .NET 8.0 SDK
3. Install SQL Server (LocalDB recommended for development)
4. Open the project in your preferred IDE
5. Follow the setup guide in `setup-guide.md`

### **Running Examples**
Each example project can be run independently:

```bash
# Navigate to any example project
cd month1/week1/PersonalInfoApp

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## 📊 Progress Tracking

Use the checklist in `practice-exercises.md` to track your progress through each month's exercises. Each exercise builds upon previous concepts, ensuring a solid foundation in modern C# development.

## 🏆 Advanced Challenges

Once you've completed the basic exercises, try these advanced challenges:

1. **Advanced DI Patterns**: Implement complex DI scenarios with multiple service lifetimes
2. **SQL Server Performance**: Optimize database queries and implement caching strategies
3. **API Security**: Add comprehensive authentication and authorization to your APIs
4. **Microservices Architecture**: Break down applications into microservices with DI
5. **Real-time Applications**: Add SignalR with DI for real-time features
6. **Event Sourcing**: Implement event sourcing patterns with SQL Server
7. **CQRS Implementation**: Use MediatR and DI for CQRS pattern
8. **API Gateway**: Create an API gateway with DI and routing

## 🤝 Contributing

This curriculum is designed to be collaborative and community-driven. Feel free to:
- Suggest improvements to exercises
- Add new examples or patterns
- Report issues or bugs
- Share your solutions and implementations

## 📝 License

This curriculum is provided as-is for educational purposes. Feel free to use, modify, and distribute as needed.

## 🎯 Learning Goals

By the end of this curriculum, you will be proficient in:
- Building robust applications with dependency injection
- Designing and implementing SQL Server databases with Entity Framework Core
- Creating RESTful APIs with proper HTTP practices
- Implementing enterprise-level patterns and best practices
- Deploying applications to cloud platforms
- Writing comprehensive tests for your applications

**Remember**: The goal is to become proficient in building robust, scalable applications using modern C# development practices with dependency injection, SQL Server, and REST APIs! 