using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PersonalInfoApp
{
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

    public class UserInputService : IUserInputService
    {
        public string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        public int GetIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                Console.WriteLine("Please enter a valid number.");
            }
        }

        public bool ValidateAge(int age)
        {
            return age >= 0 && age <= 120;
        }
    }

    public class DisplayService : IDisplayService
    {
        public void DisplayWelcome()
        {
            Console.WriteLine("=== Personal Information Collector ===");
            Console.WriteLine();
        }

        public void DisplayPersonalInfo(string name, int age, string city, string language, int experience)
        {
            Console.WriteLine("\n=== Your Information ===");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"City: {city}");
            Console.WriteLine($"Favorite Programming Language: {language}");
            Console.WriteLine($"Years of Experience: {experience}");
        }

        public void DisplayError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }
    }

    public class PersonalInfoCollector
    {
        private readonly IUserInputService _inputService;
        private readonly IDisplayService _displayService;

        public PersonalInfoCollector(IUserInputService inputService, IDisplayService displayService)
        {
            _inputService = inputService;
            _displayService = displayService;
        }

        public void CollectAndDisplayInfo()
        {
            _displayService.DisplayWelcome();

            string name = _inputService.GetStringInput("Enter your name: ");
            
            int age;
            do
            {
                age = _inputService.GetIntInput("Enter your age: ");
                if (!_inputService.ValidateAge(age))
                {
                    _displayService.DisplayError("Age must be between 0 and 120.");
                }
            } while (!_inputService.ValidateAge(age));

            string city = _inputService.GetStringInput("Enter your city: ");
            string language = _inputService.GetStringInput("Enter your favorite programming language: ");
            int experience = _inputService.GetIntInput("Enter your years of programming experience: ");

            _displayService.DisplayPersonalInfo(name, age, city, language, experience);
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
                    services.AddScoped<IUserInputService, UserInputService>();
                    services.AddScoped<IDisplayService, DisplayService>();
                    services.AddScoped<PersonalInfoCollector>();
                })
                .Build();

            // Get the service and run the application
            var collector = host.Services.GetRequiredService<PersonalInfoCollector>();
            collector.CollectAndDisplayInfo();
        }
    }
} 