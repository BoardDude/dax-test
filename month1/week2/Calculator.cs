using System;

namespace SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Simple Calculator ===");
            
            while (true)
            {
                Console.WriteLine("\nChoose an operation:");
                Console.WriteLine("1. Addition (+)");
                Console.WriteLine("2. Subtraction (-)");
                Console.WriteLine("3. Multiplication (*)");
                Console.WriteLine("4. Division (/)");
                Console.WriteLine("5. Exit");
                
                Console.Write("Enter your choice (1-5): ");
                string choice = Console.ReadLine();
                
                if (choice == "5")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                
                if (int.TryParse(choice, out int operation) && operation >= 1 && operation <= 4)
                {
                    Console.Write("Enter first number: ");
                    if (double.TryParse(Console.ReadLine(), out double num1))
                    {
                        Console.Write("Enter second number: ");
                        if (double.TryParse(Console.ReadLine(), out double num2))
                        {
                            double result = 0;
                            string operationSymbol = "";
                            
                            switch (operation)
                            {
                                case 1:
                                    result = num1 + num2;
                                    operationSymbol = "+";
                                    break;
                                case 2:
                                    result = num1 - num2;
                                    operationSymbol = "-";
                                    break;
                                case 3:
                                    result = num1 * num2;
                                    operationSymbol = "*";
                                    break;
                                case 4:
                                    if (num2 != 0)
                                    {
                                        result = num1 / num2;
                                        operationSymbol = "/";
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error: Cannot divide by zero!");
                                        continue;
                                    }
                                    break;
                            }
                            
                            Console.WriteLine($"Result: {num1} {operationSymbol} {num2} = {result}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid second number!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid first number!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice! Please enter 1-5.");
                }
            }
        }
    }
} 