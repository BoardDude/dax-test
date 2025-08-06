using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calculator
{
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

    public class CalculatorService : ICalculatorService
    {
        public double Calculate(double a, double b, string operation)
        {
            return operation.ToLower() switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b != 0 ? a / b : throw new DivideByZeroException("Cannot divide by zero."),
                _ => throw new ArgumentException($"Unknown operation: {operation}")
            };
        }

        public bool IsValidOperation(string operation)
        {
            return operation is "+" or "-" or "*" or "/";
        }

        public string[] GetAvailableOperations()
        {
            return new[] { "+", "-", "*", "/" };
        }
    }

    public class CalculatorDisplayService : ICalculatorDisplayService
    {
        public void DisplayWelcome()
        {
            Console.WriteLine("=== Simple Calculator ===");
        }

        public void DisplayResult(double a, double b, string operation, double result)
        {
            Console.WriteLine($"{a} {operation} {b} = {result:F2}");
        }

        public void DisplayError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void DisplayAvailableOperations(string[] operations)
        {
            Console.WriteLine($"Available operations: {string.Join(", ", operations)}");
        }
    }

    public class CalculatorApp
    {
        private readonly ICalculatorService _calculator;
        private readonly ICalculatorDisplayService _displayService;

        public CalculatorApp(ICalculatorService calculator, ICalculatorDisplayService displayService)
        {
            _calculator = calculator;
            _displayService = displayService;
        }

        public void Run()
        {
            _displayService.DisplayWelcome();
            _displayService.DisplayAvailableOperations(_calculator.GetAvailableOperations());
            Console.WriteLine();

            while (true)
            {
                try
                {
                    Console.Write("Enter first number (or 'quit' to exit): ");
                    string input = Console.ReadLine() ?? string.Empty;

                    if (input.ToLower() == "quit")
                        break;

                    if (!double.TryParse(input, out double a))
                    {
                        _displayService.DisplayError("Please enter a valid number.");
                        continue;
                    }

                    Console.Write("Enter operation (+/-/*//): ");
                    string operation = Console.ReadLine() ?? string.Empty;

                    if (!_calculator.IsValidOperation(operation))
                    {
                        _displayService.DisplayError("Invalid operation.");
                        continue;
                    }

                    Console.Write("Enter second number: ");
                    if (!double.TryParse(Console.ReadLine(), out double b))
                    {
                        _displayService.DisplayError("Please enter a valid number.");
                        continue;
                    }

                    double result = _calculator.Calculate(a, b, operation);
                    _displayService.DisplayResult(a, b, operation, result);
                }
                catch (Exception ex)
                {
                    _displayService.DisplayError(ex.Message);
                }

                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependency injection
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddScoped<ICalculatorService, CalculatorService>();
                    services.AddScoped<ICalculatorDisplayService, CalculatorDisplayService>();
                    services.AddScoped<CalculatorApp>();
                })
                .Build();

            // Get the service and run the application
            var app = host.Services.GetRequiredService<CalculatorApp>();
            app.Run();
        }
    }
} 