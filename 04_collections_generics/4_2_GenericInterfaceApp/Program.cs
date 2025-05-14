using System;

namespace GenericInterfacesDemo
{
    // Generic interface with constraint
    public interface IBasic<T> where T : struct
    {
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
    }
    
    // Implementation for int
    public class IntCalculator : IBasic<int>
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
        public int Divide(int a, int b) => b != 0 ? a / b : throw new DivideByZeroException();
    }
    
    // Implementation for double
    public class DoubleCalculator : IBasic<double>
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => b != 0 ? a / b : throw new DivideByZeroException();
    }
    
    // Interface with multiple type parameters
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput input);
    }
    
    // String to int converter
    public class StringToIntConverter : IConverter<string, int>
    {
        public int Convert(string input)
        {
            if (int.TryParse(input, out int result))
                return result;
            return 0;
        }
    }
    
    // Regular interface without default methods
    public interface ILogger<T>
    {
        void Log(T data);
    }
    
    // Extension method as alternative to default interface method
    public static class LoggerExtensions
    {
        public static void LogWithTimestamp<T>(this ILogger<T> logger, T data)
        {
            Console.WriteLine($"[{DateTime.Now}] {data}");
        }
    }
    
    public class ConsoleLogger<T> : ILogger<T>
    {
        public void Log(T data)
        {
            Console.WriteLine($"LOG: {data}");
        }
    }
    
    // Main demo class
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Interfaces Demo ===");
            
            // Using IBasic<T> with int
            IntCalculator intCalc = new IntCalculator();
            int resultInt = intCalc.Add(10, 20);
            Console.WriteLine($"Int addition: 10 + 20 = {resultInt}");
            Console.WriteLine($"Int multiplication: 10 * 20 = {intCalc.Multiply(10, 20)}");
            
            // Using IBasic<T> with double
            DoubleCalculator doubleCalc = new DoubleCalculator();
            double resultDouble = doubleCalc.Add(10.5, 20.5);
            Console.WriteLine($"Double addition: 10.5 + 20.5 = {resultDouble}");
            Console.WriteLine($"Double division: 10.5 / 20.5 = {doubleCalc.Divide(10.5, 20.5)}");
            
            // Using IConverter<TInput, TOutput>
            StringToIntConverter converter = new StringToIntConverter();
            int converted = converter.Convert("123");
            Console.WriteLine($"Converted string \"123\" to int: {converted}");
            
            // Using ILogger<T> with extension method
            ConsoleLogger<string> logger = new ConsoleLogger<string>();
            logger.Log("This is a regular log message");
            
            // Using extension method instead of default interface method
            logger.LogWithTimestamp("This log includes a timestamp");
            
            // Default values in generics
            Console.WriteLine("\n=== Default Values in Generics ===");
            Console.WriteLine($"Default value for int: {GetDefault<int>()}");
            Console.WriteLine($"Default value for double: {GetDefault<double>()}");
            Console.WriteLine($"Default value for bool: {GetDefault<bool>()}");
            Console.WriteLine($"Default value for string: {GetDefault<string>()}");
            Console.WriteLine($"Default value for Person: {GetDefault<Person>()}");
            
            Console.ReadLine();
        }
        
        // Method to demonstrate default values in generics
        static T GetDefault<T>()
        {
            return default(T);
        }
    }
    
    public class Person
    {
        public string Name { get; set; }
        
        public override string ToString()
        {
            return Name ?? "null";
        }
    }
}