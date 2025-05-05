// A simple calculator app that takes commands from the command line
using System;

namespace CommandLineCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to .NET Core Calculator!");
            Console.WriteLine("Available operations: add, subtract, multiply, divide");
            Console.WriteLine("Example usage: add 5 3");
            Console.WriteLine("Type 'exit' to quit");
            
            bool running = true;
            while (running)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                    
                string[] parts = input.Trim().Split(' ');
                
                if (parts[0].ToLower() == "exit")
                {
                    running = false;
                    continue;
                }
                
                if (parts.Length != 3)
                {
                    Console.WriteLine("Invalid input format. Please use: operation number1 number2");
                    continue;
                }
                
                // TODO: Implement parsing numbers and performing calculations
                // This is where you will add your code
                
                // Example implementation for addition:
                if (parts[0].ToLower() == "add")
                {
                    if (double.TryParse(parts[1], out double num1) && double.TryParse(parts[2], out double num2))
                    {
                        Console.WriteLine($"Result: {num1 + num2}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid numbers provided");
                    }
                }
                // TODO: Implement subtract, multiply, divide operations
            }
        }
    }
}