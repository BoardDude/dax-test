using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TemperatureConverter
{
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

    public class TemperatureConverterService : ITemperatureConverter
    {
        public double Convert(double value, string fromScale, string toScale)
        {
            // First convert to Celsius
            double celsius = fromScale.ToUpper() switch
            {
                "C" => value,
                "F" => (value - 32) * 5.0 / 9.0,
                "K" => value - 273.15,
                _ => throw new ArgumentException($"Invalid scale: {fromScale}")
            };

            // Then convert from Celsius to target scale
            return toScale.ToUpper() switch
            {
                "C" => celsius,
                "F" => celsius * 9.0 / 5.0 + 32,
                "K" => celsius + 273.15,
                _ => throw new ArgumentException($"Invalid scale: {toScale}")
            };
        }

        public bool IsValidScale(string scale)
        {
            return scale.ToUpper() is "C" or "F" or "K";
        }
    }

    public class TemperatureDisplayService : ITemperatureDisplayService
    {
        public void DisplayConversion(double originalValue, string fromScale, double convertedValue, string toScale)
        {
            Console.WriteLine($"{originalValue:F2}°{fromScale.ToUpper()} = {convertedValue:F2}°{toScale.ToUpper()}");
        }

        public void DisplayError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void DisplayAvailableScales()
        {
            Console.WriteLine("Available scales: C (Celsius), F (Fahrenheit), K (Kelvin)");
        }
    }

    public class TemperatureConverterApp
    {
        private readonly ITemperatureConverter _converter;
        private readonly ITemperatureDisplayService _displayService;

        public TemperatureConverterApp(ITemperatureConverter converter, ITemperatureDisplayService displayService)
        {
            _converter = converter;
            _displayService = displayService;
        }

        public void Run()
        {
            Console.WriteLine("=== Temperature Converter ===");
            _displayService.DisplayAvailableScales();
            Console.WriteLine();

            while (true)
            {
                try
                {
                    Console.Write("Enter temperature value (or 'quit' to exit): ");
                    string input = Console.ReadLine() ?? string.Empty;

                    if (input.ToLower() == "quit")
                        break;

                    if (!double.TryParse(input, out double value))
                    {
                        _displayService.DisplayError("Please enter a valid number.");
                        continue;
                    }

                    Console.Write("Enter source scale (C/F/K): ");
                    string fromScale = Console.ReadLine() ?? string.Empty;

                    if (!_converter.IsValidScale(fromScale))
                    {
                        _displayService.DisplayError("Invalid source scale.");
                        continue;
                    }

                    Console.Write("Enter target scale (C/F/K): ");
                    string toScale = Console.ReadLine() ?? string.Empty;

                    if (!_converter.IsValidScale(toScale))
                    {
                        _displayService.DisplayError("Invalid target scale.");
                        continue;
                    }

                    double convertedValue = _converter.Convert(value, fromScale, toScale);
                    _displayService.DisplayConversion(value, fromScale, convertedValue, toScale);
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
                    services.AddScoped<ITemperatureConverter, TemperatureConverterService>();
                    services.AddScoped<ITemperatureDisplayService, TemperatureDisplayService>();
                    services.AddScoped<TemperatureConverterApp>();
                })
                .Build();

            // Get the service and run the application
            var app = host.Services.GetRequiredService<TemperatureConverterApp>();
            app.Run();
        }
    }
} 