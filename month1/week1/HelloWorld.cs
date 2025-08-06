using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is your first C# program!
            Console.WriteLine("Hello, World!");
            
            // Let's add some basic interaction
            Console.Write("What's your name? ");
            string name = Console.ReadLine();
            
            Console.WriteLine($"Nice to meet you, {name}!");
            
            // Basic arithmetic
            Console.Write("Enter a number: ");
            int number = Convert.ToInt32(Console.ReadLine());
            
            Console.WriteLine($"Your number doubled is: {number * 2}");
            
            // Keep console window open
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
} 