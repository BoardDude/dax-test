-- =============================================
-- SQL Server Database Setup for C# Curriculum
-- Focus: Dependency Injection, REST APIs, Entity Framework Core
-- =============================================

USE master;
GO

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CSharpCurriculumDB')
BEGIN
    CREATE DATABASE CSharpCurriculumDB;
END
GO

USE CSharpCurriculumDB;
GO

-- =============================================
-- CREATE TABLES
-- =============================================

-- Users table for authentication and authorization
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2 NULL
);

-- User Roles for authorization
CREATE TABLE UserRoles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- User-Role relationship (many-to-many)
CREATE TABLE UserRoleMappings (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES UserRoles(Id) ON DELETE CASCADE
);

-- Contacts table (from our Contact Manager example)
CREATE TABLE Contacts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NULL,
    City NVARCHAR(100) NULL,
    Category NVARCHAR(20) NOT NULL,
    Birthday DATE NULL,
    Notes NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy INT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

-- Products table (from Inventory Management example)
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    SKU NVARCHAR(50) NOT NULL UNIQUE,
    Category NVARCHAR(50) NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    CostPrice DECIMAL(10,2) NOT NULL,
    CurrentStock INT NOT NULL DEFAULT 0,
    MinStockLevel INT NOT NULL DEFAULT 10,
    MaxStockLevel INT NOT NULL DEFAULT 1000,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy INT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

-- Inventory Transactions for tracking stock movements
CREATE TABLE InventoryTransactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    TransactionType NVARCHAR(20) NOT NULL, -- 'IN', 'OUT', 'ADJUSTMENT'
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NULL,
    Reference NVARCHAR(100) NULL, -- PO number, invoice number, etc.
    Notes NVARCHAR(500) NULL,
    TransactionDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy INT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

-- Weather Forecasts table (from Weather API example)
CREATE TABLE WeatherForecasts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    City NVARCHAR(100) NOT NULL,
    Date DATE NOT NULL,
    TemperatureC DECIMAL(5,2) NOT NULL,
    TemperatureF DECIMAL(5,2) NOT NULL,
    Description NVARCHAR(100) NOT NULL,
    Humidity INT NULL,
    WindSpeed DECIMAL(5,2) NULL,
    PrecipitationChance DECIMAL(5,2) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UNIQUE (City, Date)
);

-- Weather Alerts table
CREATE TABLE WeatherAlerts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    City NVARCHAR(100) NOT NULL,
    AlertType NVARCHAR(50) NOT NULL,
    Severity NVARCHAR(20) NOT NULL, -- 'LOW', 'MEDIUM', 'HIGH', 'CRITICAL'
    Description NVARCHAR(500) NOT NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tasks table (from Task Management example)
CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'TODO', -- 'TODO', 'IN_PROGRESS', 'REVIEW', 'DONE', 'CANCELLED'
    Priority NVARCHAR(20) NOT NULL DEFAULT 'MEDIUM', -- 'LOW', 'MEDIUM', 'HIGH', 'CRITICAL'
    Category NVARCHAR(50) NOT NULL,
    AssignedTo INT NULL,
    CreatedBy INT NOT NULL,
    DueDate DATETIME2 NULL,
    CompletedAt DATETIME2 NULL,
    EstimatedHours DECIMAL(5,2) NULL,
    ActualHours DECIMAL(5,2) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (AssignedTo) REFERENCES Users(Id),
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);

-- Task Comments
CREATE TABLE TaskComments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    UserId INT NOT NULL,
    Comment NVARCHAR(1000) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Task Attachments
CREATE TABLE TaskAttachments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL,
    ContentType NVARCHAR(100) NOT NULL,
    UploadedBy INT NOT NULL,
    UploadedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    FOREIGN KEY (UploadedBy) REFERENCES Users(Id)
);

-- API Logs for monitoring and debugging
CREATE TABLE ApiLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Endpoint NVARCHAR(200) NOT NULL,
    HttpMethod NVARCHAR(10) NOT NULL,
    StatusCode INT NOT NULL,
    RequestBody NVARCHAR(MAX) NULL,
    ResponseBody NVARCHAR(MAX) NULL,
    UserId INT NULL,
    IpAddress NVARCHAR(45) NULL,
    UserAgent NVARCHAR(500) NULL,
    RequestTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ResponseTime DATETIME2 NOT NULL,
    DurationMs INT NOT NULL,
    ErrorMessage NVARCHAR(1000) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- =============================================
-- CREATE INDEXES FOR PERFORMANCE
-- =============================================

-- Users table indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);

-- Contacts table indexes
CREATE INDEX IX_Contacts_Email ON Contacts(Email);
CREATE INDEX IX_Contacts_Category ON Contacts(Category);
CREATE INDEX IX_Contacts_City ON Contacts(City);
CREATE INDEX IX_Contacts_CreatedBy ON Contacts(CreatedBy);
CREATE INDEX IX_Contacts_CreatedAt ON Contacts(CreatedAt);

-- Products table indexes
CREATE INDEX IX_Products_SKU ON Products(SKU);
CREATE INDEX IX_Products_Category ON Products(Category);
CREATE INDEX IX_Products_IsActive ON Products(IsActive);
CREATE INDEX IX_Products_CurrentStock ON Products(CurrentStock);
CREATE INDEX IX_Products_CreatedBy ON Products(CreatedBy);

-- Inventory Transactions indexes
CREATE INDEX IX_InventoryTransactions_ProductId ON InventoryTransactions(ProductId);
CREATE INDEX IX_InventoryTransactions_TransactionType ON InventoryTransactions(TransactionType);
CREATE INDEX IX_InventoryTransactions_TransactionDate ON InventoryTransactions(TransactionDate);
CREATE INDEX IX_InventoryTransactions_CreatedBy ON InventoryTransactions(CreatedBy);

-- Weather Forecasts indexes
CREATE INDEX IX_WeatherForecasts_City ON WeatherForecasts(City);
CREATE INDEX IX_WeatherForecasts_Date ON WeatherForecasts(Date);
CREATE INDEX IX_WeatherForecasts_CityDate ON WeatherForecasts(City, Date);

-- Weather Alerts indexes
CREATE INDEX IX_WeatherAlerts_City ON WeatherAlerts(City);
CREATE INDEX IX_WeatherAlerts_AlertType ON WeatherAlerts(AlertType);
CREATE INDEX IX_WeatherAlerts_Severity ON WeatherAlerts(Severity);
CREATE INDEX IX_WeatherAlerts_IsActive ON WeatherAlerts(IsActive);
CREATE INDEX IX_WeatherAlerts_StartTime ON WeatherAlerts(StartTime);

-- Tasks table indexes
CREATE INDEX IX_Tasks_Status ON Tasks(Status);
CREATE INDEX IX_Tasks_Priority ON Tasks(Priority);
CREATE INDEX IX_Tasks_Category ON Tasks(Category);
CREATE INDEX IX_Tasks_AssignedTo ON Tasks(AssignedTo);
CREATE INDEX IX_Tasks_CreatedBy ON Tasks(CreatedBy);
CREATE INDEX IX_Tasks_DueDate ON Tasks(DueDate);
CREATE INDEX IX_Tasks_StatusPriority ON Tasks(Status, Priority);

-- Task Comments indexes
CREATE INDEX IX_TaskComments_TaskId ON TaskComments(TaskId);
CREATE INDEX IX_TaskComments_UserId ON TaskComments(UserId);
CREATE INDEX IX_TaskComments_CreatedAt ON TaskComments(CreatedAt);

-- Task Attachments indexes
CREATE INDEX IX_TaskAttachments_TaskId ON TaskAttachments(TaskId);
CREATE INDEX IX_TaskAttachments_UploadedBy ON TaskAttachments(UploadedBy);

-- API Logs indexes
CREATE INDEX IX_ApiLogs_Endpoint ON ApiLogs(Endpoint);
CREATE INDEX IX_ApiLogs_StatusCode ON ApiLogs(StatusCode);
CREATE INDEX IX_ApiLogs_UserId ON ApiLogs(UserId);
CREATE INDEX IX_ApiLogs_RequestTime ON ApiLogs(RequestTime);
CREATE INDEX IX_ApiLogs_DurationMs ON ApiLogs(DurationMs);

-- =============================================
-- CREATE STORED PROCEDURES
-- =============================================

-- Stored Procedure: Get Contact Statistics
CREATE PROCEDURE sp_GetContactStatistics
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalContacts,
        COUNT(CASE WHEN Category = 'Family' THEN 1 END) AS FamilyContacts,
        COUNT(CASE WHEN Category = 'Work' THEN 1 END) AS WorkContacts,
        COUNT(CASE WHEN Category = 'Friends' THEN 1 END) AS FriendsContacts,
        COUNT(CASE WHEN Category = 'Other' THEN 1 END) AS OtherContacts,
        COUNT(CASE WHEN Birthday IS NOT NULL THEN 1 END) AS ContactsWithBirthday,
        COUNT(CASE WHEN CreatedAt >= DATEADD(day, -30, GETUTCDATE()) THEN 1 END) AS RecentContacts
    FROM Contacts
    WHERE (@UserId IS NULL OR CreatedBy = @UserId);
END
GO

-- Stored Procedure: Get Low Stock Products
CREATE PROCEDURE sp_GetLowStockProducts
    @Threshold INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Name,
        p.SKU,
        p.Category,
        p.CurrentStock,
        p.MinStockLevel,
        p.UnitPrice,
        p.CostPrice,
        CASE 
            WHEN p.CurrentStock = 0 THEN 'OUT_OF_STOCK'
            WHEN p.CurrentStock <= p.MinStockLevel THEN 'LOW_STOCK'
            ELSE 'NORMAL'
        END AS StockStatus
    FROM Products p
    WHERE p.IsActive = 1 
        AND p.CurrentStock <= @Threshold
    ORDER BY p.CurrentStock ASC, p.Name ASC;
END
GO

-- Stored Procedure: Get Product Stock History
CREATE PROCEDURE sp_GetProductStockHistory
    @ProductId INT,
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @StartDate IS NULL SET @StartDate = DATEADD(month, -1, GETUTCDATE());
    IF @EndDate IS NULL SET @EndDate = GETUTCDATE();
    
    SELECT 
        it.Id,
        it.TransactionType,
        it.Quantity,
        it.UnitPrice,
        it.Reference,
        it.Notes,
        it.TransactionDate,
        u.FirstName + ' ' + u.LastName AS CreatedByUser
    FROM InventoryTransactions it
    LEFT JOIN Users u ON it.CreatedBy = u.Id
    WHERE it.ProductId = @ProductId
        AND it.TransactionDate BETWEEN @StartDate AND @EndDate
    ORDER BY it.TransactionDate DESC;
END
GO

-- Stored Procedure: Get Weather Forecasts by City
CREATE PROCEDURE sp_GetWeatherForecastsByCity
    @City NVARCHAR(100),
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @StartDate IS NULL SET @StartDate = CAST(GETUTCDATE() AS DATE);
    IF @EndDate IS NULL SET @EndDate = DATEADD(day, 7, @StartDate);
    
    SELECT 
        Id,
        City,
        Date,
        TemperatureC,
        TemperatureF,
        Description,
        Humidity,
        WindSpeed,
        PrecipitationChance,
        CreatedAt,
        UpdatedAt
    FROM WeatherForecasts
    WHERE City = @City
        AND Date BETWEEN @StartDate AND @EndDate
    ORDER BY Date ASC;
END
GO

-- Stored Procedure: Get Active Weather Alerts
CREATE PROCEDURE sp_GetActiveWeatherAlerts
    @City NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        City,
        AlertType,
        Severity,
        Description,
        StartTime,
        EndTime,
        IsActive,
        CreatedAt
    FROM WeatherAlerts
    WHERE IsActive = 1
        AND (@City IS NULL OR City = @City)
        AND GETUTCDATE() BETWEEN StartTime AND EndTime
    ORDER BY Severity DESC, StartTime ASC;
END
GO

-- Stored Procedure: Get Tasks by User
CREATE PROCEDURE sp_GetTasksByUser
    @UserId INT,
    @Status NVARCHAR(20) = NULL,
    @Priority NVARCHAR(20) = NULL,
    @Category NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.Priority,
        t.Category,
        t.DueDate,
        t.EstimatedHours,
        t.ActualHours,
        t.CreatedAt,
        t.UpdatedAt,
        creator.FirstName + ' ' + creator.LastName AS CreatedBy,
        assignee.FirstName + ' ' + assignee.LastName AS AssignedTo
    FROM Tasks t
    INNER JOIN Users creator ON t.CreatedBy = creator.Id
    LEFT JOIN Users assignee ON t.AssignedTo = assignee.Id
    WHERE (t.AssignedTo = @UserId OR t.CreatedBy = @UserId)
        AND (@Status IS NULL OR t.Status = @Status)
        AND (@Priority IS NULL OR t.Priority = @Priority)
        AND (@Category IS NULL OR t.Category = @Category)
    ORDER BY 
        CASE t.Priority 
            WHEN 'CRITICAL' THEN 1 
            WHEN 'HIGH' THEN 2 
            WHEN 'MEDIUM' THEN 3 
            WHEN 'LOW' THEN 4 
        END,
        t.DueDate ASC;
END
GO

-- Stored Procedure: Get Task Statistics
CREATE PROCEDURE sp_GetTaskStatistics
    @UserId INT = NULL,
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @StartDate IS NULL SET @StartDate = DATEADD(month, -1, GETUTCDATE());
    IF @EndDate IS NULL SET @EndDate = GETUTCDATE();
    
    SELECT 
        COUNT(*) AS TotalTasks,
        COUNT(CASE WHEN Status = 'TODO' THEN 1 END) AS TodoTasks,
        COUNT(CASE WHEN Status = 'IN_PROGRESS' THEN 1 END) AS InProgressTasks,
        COUNT(CASE WHEN Status = 'REVIEW' THEN 1 END) AS ReviewTasks,
        COUNT(CASE WHEN Status = 'DONE' THEN 1 END) AS DoneTasks,
        COUNT(CASE WHEN Status = 'CANCELLED' THEN 1 END) AS CancelledTasks,
        COUNT(CASE WHEN Priority = 'CRITICAL' THEN 1 END) AS CriticalTasks,
        COUNT(CASE WHEN Priority = 'HIGH' THEN 1 END) AS HighPriorityTasks,
        COUNT(CASE WHEN DueDate < GETUTCDATE() AND Status NOT IN ('DONE', 'CANCELLED') THEN 1 END) AS OverdueTasks,
        AVG(CAST(ActualHours AS FLOAT)) AS AverageActualHours,
        SUM(CAST(ActualHours AS FLOAT)) AS TotalActualHours
    FROM Tasks
    WHERE CreatedAt BETWEEN @StartDate AND @EndDate
        AND (@UserId IS NULL OR CreatedBy = @UserId OR AssignedTo = @UserId);
END
GO

-- Stored Procedure: Get API Performance Statistics
CREATE PROCEDURE sp_GetApiPerformanceStatistics
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL,
    @Endpoint NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @StartDate IS NULL SET @StartDate = DATEADD(hour, -24, GETUTCDATE());
    IF @EndDate IS NULL SET @EndDate = GETUTCDATE();
    
    SELECT 
        Endpoint,
        HttpMethod,
        COUNT(*) AS RequestCount,
        AVG(CAST(DurationMs AS FLOAT)) AS AverageDurationMs,
        MIN(DurationMs) AS MinDurationMs,
        MAX(DurationMs) AS MaxDurationMs,
        COUNT(CASE WHEN StatusCode >= 400 THEN 1 END) AS ErrorCount,
        COUNT(CASE WHEN StatusCode = 200 THEN 1 END) AS SuccessCount,
        COUNT(CASE WHEN StatusCode = 404 THEN 1 END) AS NotFoundCount,
        COUNT(CASE WHEN StatusCode = 500 THEN 1 END) AS ServerErrorCount
    FROM ApiLogs
    WHERE RequestTime BETWEEN @StartDate AND @EndDate
        AND (@Endpoint IS NULL OR Endpoint = @Endpoint)
    GROUP BY Endpoint, HttpMethod
    ORDER BY RequestCount DESC;
END
GO

-- =============================================
-- CREATE FUNCTIONS
-- =============================================

-- Function: Calculate Age from Birthday
CREATE FUNCTION fn_CalculateAge(@Birthday DATE)
RETURNS INT
AS
BEGIN
    DECLARE @Age INT;
    SET @Age = YEAR(GETUTCDATE()) - YEAR(@Birthday);
    
    IF MONTH(GETUTCDATE()) < MONTH(@Birthday) 
        OR (MONTH(GETUTCDATE()) = MONTH(@Birthday) AND DAY(GETUTCDATE()) < DAY(@Birthday))
    BEGIN
        SET @Age = @Age - 1;
    END
    
    RETURN @Age;
END
GO

-- Function: Get Contact Full Name
CREATE FUNCTION fn_GetContactFullName(@ContactId INT)
RETURNS NVARCHAR(200)
AS
BEGIN
    DECLARE @FullName NVARCHAR(200);
    
    SELECT @FullName = Name
    FROM Contacts
    WHERE Id = @ContactId;
    
    RETURN @FullName;
END
GO

-- Function: Get Product Stock Status
CREATE FUNCTION fn_GetProductStockStatus(@ProductId INT)
RETURNS NVARCHAR(20)
AS
BEGIN
    DECLARE @StockStatus NVARCHAR(20);
    DECLARE @CurrentStock INT;
    DECLARE @MinStockLevel INT;
    
    SELECT @CurrentStock = CurrentStock, @MinStockLevel = MinStockLevel
    FROM Products
    WHERE Id = @ProductId;
    
    SET @StockStatus = CASE 
        WHEN @CurrentStock = 0 THEN 'OUT_OF_STOCK'
        WHEN @CurrentStock <= @MinStockLevel THEN 'LOW_STOCK'
        ELSE 'NORMAL'
    END;
    
    RETURN @StockStatus;
END
GO

-- =============================================
-- CREATE TRIGGERS
-- =============================================

-- Trigger: Update UpdatedAt timestamp
CREATE TRIGGER tr_Contacts_UpdateTimestamp
ON Contacts
AFTER UPDATE
AS
BEGIN
    UPDATE Contacts
    SET UpdatedAt = GETUTCDATE()
    FROM Contacts c
    INNER JOIN inserted i ON c.Id = i.Id;
END
GO

CREATE TRIGGER tr_Products_UpdateTimestamp
ON Products
AFTER UPDATE
AS
BEGIN
    UPDATE Products
    SET UpdatedAt = GETUTCDATE()
    FROM Products p
    INNER JOIN inserted i ON p.Id = i.Id;
END
GO

CREATE TRIGGER tr_Tasks_UpdateTimestamp
ON Tasks
AFTER UPDATE
AS
BEGIN
    UPDATE Tasks
    SET UpdatedAt = GETUTCDATE()
    FROM Tasks t
    INNER JOIN inserted i ON t.Id = i.Id;
END
GO

-- Trigger: Update product stock on inventory transaction
CREATE TRIGGER tr_InventoryTransactions_UpdateStock
ON InventoryTransactions
AFTER INSERT
AS
BEGIN
    UPDATE Products
    SET CurrentStock = CurrentStock + 
        CASE 
            WHEN i.TransactionType = 'IN' THEN i.Quantity
            WHEN i.TransactionType = 'OUT' THEN -i.Quantity
            WHEN i.TransactionType = 'ADJUSTMENT' THEN i.Quantity
            ELSE 0
        END
    FROM Products p
    INNER JOIN inserted i ON p.Id = i.ProductId;
END
GO

-- =============================================
-- INSERT SAMPLE DATA
-- =============================================

-- Insert User Roles
INSERT INTO UserRoles (Name, Description) VALUES
('Admin', 'System Administrator'),
('Manager', 'Project Manager'),
('Developer', 'Software Developer'),
('Tester', 'Quality Assurance'),
('User', 'Regular User');

-- Insert Sample Users
INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName) VALUES
('admin', 'admin@company.com', 'hashed_password_here', 'John', 'Admin'),
('jane.doe', 'jane.doe@company.com', 'hashed_password_here', 'Jane', 'Doe'),
('bob.smith', 'bob.smith@company.com', 'hashed_password_here', 'Bob', 'Smith'),
('alice.johnson', 'alice.johnson@company.com', 'hashed_password_here', 'Alice', 'Johnson'),
('mike.wilson', 'mike.wilson@company.com', 'hashed_password_here', 'Mike', 'Wilson');

-- Assign Roles to Users
INSERT INTO UserRoleMappings (UserId, RoleId) VALUES
(1, 1), -- Admin user gets Admin role
(2, 2), -- Jane gets Manager role
(3, 3), -- Bob gets Developer role
(4, 4), -- Alice gets Tester role
(5, 5); -- Mike gets User role

-- Insert Sample Contacts
INSERT INTO Contacts (Name, Email, Phone, City, Category, Birthday, CreatedBy) VALUES
('John Smith', 'john.smith@email.com', '555-0101', 'New York', 'Work', '1985-03-15', 1),
('Sarah Johnson', 'sarah.johnson@email.com', '555-0102', 'Los Angeles', 'Family', '1990-07-22', 1),
('Mike Davis', 'mike.davis@email.com', '555-0103', 'Chicago', 'Friends', '1988-11-08', 2),
('Emily Wilson', 'emily.wilson@email.com', '555-0104', 'Houston', 'Work', '1992-04-12', 2),
('David Brown', 'david.brown@email.com', '555-0105', 'Phoenix', 'Other', '1987-09-30', 3);

-- Insert Sample Products
INSERT INTO Products (Name, Description, SKU, Category, UnitPrice, CostPrice, CurrentStock, MinStockLevel, CreatedBy) VALUES
('Laptop Dell XPS 13', '13-inch premium laptop with Intel i7', 'LAP-DELL-XPS13', 'Electronics', 1299.99, 899.99, 25, 5, 1),
('iPhone 15 Pro', 'Latest iPhone with A17 Pro chip', 'PHONE-IPHONE15PRO', 'Electronics', 999.99, 699.99, 15, 10, 1),
('Office Chair Ergonomic', 'Comfortable office chair with lumbar support', 'FURN-CHAIR-ERG', 'Furniture', 299.99, 199.99, 8, 3, 2),
('Coffee Maker B2000', 'Programmable coffee maker with thermal carafe', 'KITCH-COFFEE-B2000', 'Kitchen', 89.99, 59.99, 12, 5, 2),
('Wireless Headphones', 'Noise-cancelling wireless headphones', 'AUDIO-WIRELESS-HP', 'Electronics', 199.99, 149.99, 30, 8, 3);

-- Insert Sample Inventory Transactions
INSERT INTO InventoryTransactions (ProductId, TransactionType, Quantity, UnitPrice, Reference, Notes, CreatedBy) VALUES
(1, 'IN', 30, 899.99, 'PO-2024-001', 'Initial stock purchase', 1),
(1, 'OUT', 5, 1299.99, 'INV-2024-001', 'Customer order', 2),
(2, 'IN', 20, 699.99, 'PO-2024-002', 'iPhone stock purchase', 1),
(2, 'OUT', 5, 999.99, 'INV-2024-002', 'Customer order', 2),
(3, 'IN', 10, 199.99, 'PO-2024-003', 'Furniture purchase', 2),
(3, 'OUT', 2, 299.99, 'INV-2024-003', 'Office supply order', 3);

-- Insert Sample Weather Forecasts
INSERT INTO WeatherForecasts (City, Date, TemperatureC, TemperatureF, Description, Humidity, WindSpeed, PrecipitationChance) VALUES
('New York', CAST(GETUTCDATE() AS DATE), 22.5, 72.5, 'Partly Cloudy', 65, 12.5, 20.0),
('New York', DATEADD(day, 1, CAST(GETUTCDATE() AS DATE)), 25.0, 77.0, 'Sunny', 55, 8.0, 10.0),
('New York', DATEADD(day, 2, CAST(GETUTCDATE() AS DATE)), 18.0, 64.4, 'Rainy', 85, 25.0, 80.0),
('Los Angeles', CAST(GETUTCDATE() AS DATE), 28.0, 82.4, 'Sunny', 45, 5.0, 5.0),
('Los Angeles', DATEADD(day, 1, CAST(GETUTCDATE() AS DATE)), 30.0, 86.0, 'Clear', 40, 3.0, 0.0),
('Chicago', CAST(GETUTCDATE() AS DATE), 15.0, 59.0, 'Cloudy', 70, 15.0, 30.0);

-- Insert Sample Weather Alerts
INSERT INTO WeatherAlerts (City, AlertType, Severity, Description, StartTime, EndTime) VALUES
('New York', 'Severe Thunderstorm', 'HIGH', 'Severe thunderstorm warning in effect', DATEADD(hour, -2, GETUTCDATE()), DATEADD(hour, 2, GETUTCDATE())),
('Chicago', 'Winter Storm', 'CRITICAL', 'Heavy snowfall expected', DATEADD(hour, -1, GETUTCDATE()), DATEADD(hour, 6, GETUTCDATE())),
('Los Angeles', 'Heat Advisory', 'MEDIUM', 'High temperatures expected', DATEADD(hour, 1, GETUTCDATE()), DATEADD(hour, 8, GETUTCDATE()));

-- Insert Sample Tasks
INSERT INTO Tasks (Title, Description, Status, Priority, Category, AssignedTo, CreatedBy, DueDate, EstimatedHours) VALUES
('Implement User Authentication', 'Add JWT token authentication to the API', 'IN_PROGRESS', 'HIGH', 'Development', 3, 2, DATEADD(day, 3, GETUTCDATE()), 16.0),
('Design Database Schema', 'Create ERD for the new contact management system', 'DONE', 'MEDIUM', 'Design', 4, 2, DATEADD(day, -2, GETUTCDATE()), 8.0),
('Write Unit Tests', 'Create comprehensive unit tests for ContactService', 'TODO', 'HIGH', 'Testing', 4, 3, DATEADD(day, 5, GETUTCDATE()), 12.0),
('Deploy to Production', 'Deploy the new version to production environment', 'TODO', 'CRITICAL', 'DevOps', 1, 2, DATEADD(day, 1, GETUTCDATE()), 4.0),
('Code Review', 'Review the authentication implementation', 'REVIEW', 'MEDIUM', 'Development', 2, 3, DATEADD(day, 2, GETUTCDATE()), 2.0);

-- Insert Sample Task Comments
INSERT INTO TaskComments (TaskId, UserId, Comment) VALUES
(1, 3, 'Started implementing JWT token generation'),
(1, 2, 'Make sure to include refresh token functionality'),
(2, 4, 'Database schema looks good, approved'),
(3, 4, 'Will start with ContactService tests tomorrow'),
(4, 1, 'Production deployment checklist completed');

-- Insert Sample API Logs
INSERT INTO ApiLogs (Endpoint, HttpMethod, StatusCode, UserId, IpAddress, UserAgent, ResponseTime, DurationMs) VALUES
('/api/contacts', 'GET', 200, 1, '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)', DATEADD(minute, -30, GETUTCDATE()), 45),
('/api/contacts', 'POST', 201, 2, '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X)', DATEADD(minute, -25, GETUTCDATE()), 120),
('/api/contacts/1', 'PUT', 200, 1, '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)', DATEADD(minute, -20, GETUTCDATE()), 78),
('/api/weather/new-york', 'GET', 200, 3, '192.168.1.102', 'PostmanRuntime/7.0', DATEADD(minute, -15, GETUTCDATE()), 156),
('/api/products', 'GET', 200, 2, '192.168.1.101', 'Mozilla/5.0 (Macintosh; Intel Mac OS X)', DATEADD(minute, -10, GETUTCDATE()), 89),
('/api/nonexistent', 'GET', 404, 1, '192.168.1.100', 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)', DATEADD(minute, -5, GETUTCDATE()), 12);

-- =============================================
-- CREATE VIEWS FOR COMMON QUERIES
-- =============================================

-- View: Contact Details with User Information
CREATE VIEW vw_ContactDetails AS
SELECT 
    c.Id,
    c.Name,
    c.Email,
    c.Phone,
    c.City,
    c.Category,
    c.Birthday,
    dbo.fn_CalculateAge(c.Birthday) AS Age,
    c.Notes,
    c.CreatedAt,
    c.UpdatedAt,
    u.FirstName + ' ' + u.LastName AS CreatedByUser
FROM Contacts c
LEFT JOIN Users u ON c.CreatedBy = u.Id;

-- View: Product Stock Status
CREATE VIEW vw_ProductStockStatus AS
SELECT 
    p.Id,
    p.Name,
    p.SKU,
    p.Category,
    p.UnitPrice,
    p.CostPrice,
    p.CurrentStock,
    p.MinStockLevel,
    p.MaxStockLevel,
    dbo.fn_GetProductStockStatus(p.Id) AS StockStatus,
    p.IsActive,
    u.FirstName + ' ' + u.LastName AS CreatedByUser,
    p.CreatedAt,
    p.UpdatedAt
FROM Products p
LEFT JOIN Users u ON p.CreatedBy = u.Id;

-- View: Task Details with User Information
CREATE VIEW vw_TaskDetails AS
SELECT 
    t.Id,
    t.Title,
    t.Description,
    t.Status,
    t.Priority,
    t.Category,
    t.DueDate,
    t.EstimatedHours,
    t.ActualHours,
    t.CompletedAt,
    t.CreatedAt,
    t.UpdatedAt,
    creator.FirstName + ' ' + creator.LastName AS CreatedBy,
    assignee.FirstName + ' ' + assignee.LastName AS AssignedTo,
    CASE 
        WHEN t.DueDate < GETUTCDATE() AND t.Status NOT IN ('DONE', 'CANCELLED') THEN 1
        ELSE 0
    END AS IsOverdue
FROM Tasks t
INNER JOIN Users creator ON t.CreatedBy = creator.Id
LEFT JOIN Users assignee ON t.AssignedTo = assignee.Id;

-- View: API Performance Summary
CREATE VIEW vw_ApiPerformanceSummary AS
SELECT 
    Endpoint,
    HttpMethod,
    COUNT(*) AS RequestCount,
    AVG(CAST(DurationMs AS FLOAT)) AS AverageDurationMs,
    COUNT(CASE WHEN StatusCode >= 400 THEN 1 END) AS ErrorCount,
    COUNT(CASE WHEN StatusCode = 200 THEN 1 END) AS SuccessCount,
    CAST(COUNT(CASE WHEN StatusCode >= 400 THEN 1 END) AS FLOAT) / COUNT(*) * 100 AS ErrorRate
FROM ApiLogs
WHERE RequestTime >= DATEADD(hour, -24, GETUTCDATE())
GROUP BY Endpoint, HttpMethod;

-- =============================================
-- CREATE SECURITY ROLES AND PERMISSIONS
-- =============================================

-- Create database roles
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'AppUser')
BEGIN
    CREATE ROLE AppUser;
END

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'AppAdmin')
BEGIN
    CREATE ROLE AppAdmin;
END

-- Grant permissions to AppUser role
GRANT SELECT ON Contacts TO AppUser;
GRANT INSERT, UPDATE, DELETE ON Contacts TO AppUser;
GRANT SELECT ON Products TO AppUser;
GRANT SELECT ON Tasks TO AppUser;
GRANT EXECUTE ON sp_GetContactStatistics TO AppUser;
GRANT EXECUTE ON sp_GetLowStockProducts TO AppUser;

-- Grant permissions to AppAdmin role
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES TO AppAdmin;
GRANT EXECUTE ON ALL PROCEDURES TO AppAdmin;
GRANT EXECUTE ON ALL FUNCTIONS TO AppAdmin;

-- =============================================
-- VERIFICATION QUERIES
-- =============================================

-- Verify table creation
SELECT 
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Verify indexes
SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    c.name AS ColumnName
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.is_hypothetical = 0
ORDER BY t.name, i.name;

-- Verify stored procedures
SELECT 
    ROUTINE_NAME,
    ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME;

-- Verify functions
SELECT 
    ROUTINE_NAME,
    ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'FUNCTION'
ORDER BY ROUTINE_NAME;

-- Verify triggers
SELECT 
    t.name AS TableName,
    tr.name AS TriggerName,
    tr.type_desc AS TriggerType
FROM sys.triggers tr
INNER JOIN sys.tables t ON tr.parent_id = t.object_id
ORDER BY t.name, tr.name;

-- Sample data verification
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL
SELECT 'Contacts', COUNT(*) FROM Contacts
UNION ALL
SELECT 'Products', COUNT(*) FROM Products
UNION ALL
SELECT 'Tasks', COUNT(*) FROM Tasks
UNION ALL
SELECT 'WeatherForecasts', COUNT(*) FROM WeatherForecasts;

PRINT 'Database setup completed successfully!';
PRINT 'Tables, indexes, stored procedures, functions, triggers, and sample data have been created.';
PRINT 'You can now use this database with your C# applications using Entity Framework Core.'; 