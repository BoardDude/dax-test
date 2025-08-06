# Development Environment Setup Guide

This guide will help you set up your development environment for the 6-month C# software development curriculum.

## 🛠️ Required Software

### 1. .NET 8.0 SDK
- **Download**: [Microsoft .NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Installation**: Follow the installer instructions for your operating system
- **Verification**: Open terminal/command prompt and run:
  ```bash
  dotnet --version
  ```
  You should see version 8.0.x

### 2. Visual Studio Code
- **Download**: [Visual Studio Code](https://code.visualstudio.com/)
- **Installation**: Download and install for your operating system
- **Extensions**: Install these extensions in VS Code:
  - C# (by Microsoft)
  - C# Extensions
  - .NET Core Tools
  - GitLens (optional but recommended)

### 3. Git
- **Download**: [Git for Windows/Mac/Linux](https://git-scm.com/)
- **Installation**: Follow the installer instructions
- **Configuration**: Set up your identity:
  ```bash
  git config --global user.name "Your Name"
  git config --global user.email "your.email@example.com"
  ```

## 🚀 First Steps

### 1. Create Your First Project
```bash
# Create a new directory for your learning
mkdir csharp-learning
cd csharp-learning

# Create your first console application
dotnet new console -n HelloWorld
cd HelloWorld

# Run your first program
dotnet run
```

### 2. Verify Your Setup
Create a test file to verify everything is working:

```csharp
// Test.cs
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello from C#!");
        Console.WriteLine($"Current time: {DateTime.Now}");
        Console.WriteLine("Your development environment is ready!");
    }
}
```

Run it with:
```bash
dotnet run
```

## 📁 Project Structure

Create this folder structure for your learning journey:

```
csharp-learning/
├── month1/
│   ├── week1/
│   ├── week2/
│   ├── week3/
│   └── week4/
├── month2/
│   ├── week1/
│   ├── week2/
│   ├── week3/
│   └── week4/
├── month3/
│   ├── week1/
│   ├── week2/
│   ├── week3/
│   └── week4/
├── month4/
│   ├── week1/
│   ├── week2/
│   ├── week3/
│   └── week4/
├── month5/
│   ├── week1/
│   ├── week2/
│   ├── week3/
│   └── week4/
└── month6/
    ├── week1/
    ├── week2/
    ├── week3/
    └── week4/
```

## 🔧 Common Commands

### Creating New Projects
```bash
# Console application
dotnet new console -n ProjectName

# Web API
dotnet new webapi -n ProjectName

# Class library
dotnet new classlib -n ProjectName

# Unit test project
dotnet new xunit -n ProjectName.Tests
```

### Building and Running
```bash
# Build project
dotnet build

# Run project
dotnet run

# Run tests
dotnet test

# Clean build artifacts
dotnet clean
```

### Package Management
```bash
# Add a package
dotnet add package PackageName

# Remove a package
dotnet remove package PackageName

# List packages
dotnet list package
```

## 🌐 Useful Resources

### Documentation
- [Microsoft C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)

### Learning Platforms
- [Microsoft Learn](https://docs.microsoft.com/en-us/learn/)
- [Pluralsight](https://www.pluralsight.com/) (subscription required)
- [Udemy](https://www.udemy.com/) (various courses)

### Practice Sites
- [LeetCode](https://leetcode.com/) - Algorithm problems
- [HackerRank](https://www.hackerrank.com/) - Programming challenges
- [CodeWars](https://www.codewars.com/) - Coding katas

## 🐛 Troubleshooting

### Common Issues

1. **"dotnet command not found"**
   - Make sure .NET SDK is installed
   - Restart your terminal/command prompt
   - Add .NET to your PATH environment variable

2. **"Cannot find project file"**
   - Make sure you're in the correct directory
   - Run `dotnet new console` to create a new project

3. **Build errors**
   - Run `dotnet clean` then `dotnet build`
   - Check that you're using the correct .NET version

4. **VS Code C# extension not working**
   - Install the C# extension
   - Restart VS Code
   - Make sure you have the .NET SDK installed

### Getting Help
- [Stack Overflow](https://stackoverflow.com/) - Programming Q&A
- [Microsoft Q&A](https://docs.microsoft.com/en-us/answers/) - Microsoft-specific questions
- [GitHub Issues](https://github.com/dotnet/core/issues) - .NET Core issues

## 📝 Next Steps

1. **Complete the setup** - Make sure all tools are installed and working
2. **Start with Month 1** - Begin with the basic C# concepts
3. **Practice daily** - Even 30 minutes of coding daily is better than 4 hours once a week
4. **Build projects** - Apply what you learn by building real applications
5. **Join communities** - Participate in C# and .NET communities

## 🎯 Success Checklist

- [ ] .NET 8.0 SDK installed and working
- [ ] Visual Studio Code installed with C# extension
- [ ] Git installed and configured
- [ ] First "Hello World" program runs successfully
- [ ] Project folder structure created
- [ ] Ready to start Month 1 curriculum

Once you've completed this checklist, you're ready to begin your 6-month C# software development journey! 