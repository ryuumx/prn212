using System;
using System.Collections.Generic;
using System.Linq;

namespace AnonymousGenericDelegate
{
    // Custom delegate for comparison
    public delegate int Calculator(int x, int y);
    public delegate bool NumberPredicate(int number);

    public class AnonymousMethodsAndGenericDelegates
    {
        // Named method for comparison
        public static int AddNumbers(int a, int b)
        {
            return a + b;
        }

        public static bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("=== ANONYMOUS METHODS DEMONSTRATION ===");

            // 1. Traditional approach with named methods
            Console.WriteLine("1. Traditional approach with named methods:");
            Calculator namedCalculator = AddNumbers;
            int result1 = namedCalculator(10, 5);
            Console.WriteLine($"Named method result: {result1}");
            Console.WriteLine();

            // 2. Anonymous methods using delegate keyword
            Console.WriteLine("2. Anonymous methods using delegate keyword:");
            
            Calculator anonymousCalculator = delegate(int x, int y)
            {
                Console.WriteLine($"Anonymous method calculating: {x} + {y}");
                return x + y;
            };
            
            int result2 = anonymousCalculator(10, 5);
            Console.WriteLine($"Anonymous method result: {result2}");
            Console.WriteLine();

            // 3. Multiple anonymous methods for different operations
            Console.WriteLine("3. Different anonymous method operations:");
            
            Calculator multiply = delegate(int x, int y) { return x * y; };
            Calculator subtract = delegate(int x, int y) { return x - y; };
            Calculator divide = delegate(int x, int y) 
            { 
                if (y != 0) return x / y;
                Console.WriteLine("Division by zero!");
                return 0;
            };

            Console.WriteLine($"Multiply: {multiply(8, 3)}");
            Console.WriteLine($"Subtract: {subtract(8, 3)}");
            Console.WriteLine($"Divide: {divide(8, 3)}");
            Console.WriteLine();

            Console.WriteLine("=== GENERIC DELEGATES: FUNC AND ACTION ===");

            // 4. Func delegate examples
            Console.WriteLine("4. Func delegate examples:");
            
            // Func with one parameter and return value
            Func<int, int> square = delegate(int x) { return x * x; };
            Console.WriteLine($"Square of 5: {square(5)}");

            // Func with two parameters and return value
            Func<int, int, int> add = delegate(int x, int y) { return x + y; };
            Console.WriteLine($"Add 7 + 3: {add(7, 3)}");

            // Func with string parameter and bool return
            Func<string, bool> isLongString = delegate(string str) 
            { 
                return str.Length > 5; 
            };
            Console.WriteLine($"Is 'Hello' long? {isLongString("Hello")}");
            Console.WriteLine($"Is 'Programming' long? {isLongString("Programming")}");
            Console.WriteLine();

            // 5. Action delegate examples
            Console.WriteLine("5. Action delegate examples:");
            
            // Action with one parameter (no return value)
            Action<string> printMessage = delegate(string msg)
            {
                Console.WriteLine($"📢 Message: {msg}");
            };
            printMessage("Hello from Action!");

            // Action with multiple parameters
            Action<string, int> printPersonInfo = delegate(string name, int age)
            {
                Console.WriteLine($"👤 Person: {name}, Age: {age}");
            };
            printPersonInfo("Alice", 25);

            // Action with no parameters
            Action greet = delegate()
            {
                Console.WriteLine("👋 Hello World from parameterless Action!");
            };
            greet();
            Console.WriteLine();

            // 6. Using Func and Action in collections
            Console.WriteLine("6. Using Func and Action with collections:");
            
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Using Func with Where (filtering)
            Func<int, bool> isEvenFunc = delegate(int n) { return n % 2 == 0; };
            var evenNumbers = numbers.Where(isEvenFunc);
            
            Console.WriteLine("Even numbers:");
            foreach (int num in evenNumbers)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();

            // Using Action with ForEach
            Action<int> printSquare = delegate(int n)
            {
                Console.WriteLine($"{n}² = {n * n}");
            };
            
            Console.WriteLine("Squares of first 5 numbers:");
            numbers.Take(5).ToList().ForEach(printSquare);
            Console.WriteLine();

            // 7. Predicate delegate (specialized Func<T, bool>)
            Console.WriteLine("7. Predicate delegate:");
            
            Predicate<int> isPositive = delegate(int n) { return n > 0; };
            List<int> mixedNumbers = new List<int> { -3, -1, 0, 2, 5, -7, 8 };
            
            List<int> positiveNumbers = mixedNumbers.FindAll(isPositive);
            Console.WriteLine($"Positive numbers: {string.Join(", ", positiveNumbers)}");
            Console.WriteLine();

            // 8. Comparison between custom delegate and Func/Action
            Console.WriteLine("8. Custom delegate vs Func/Action comparison:");
            
            // Custom delegate
            NumberPredicate customPredicate = delegate(int n) { return n > 5; };
            
            // Equivalent Func
            Func<int, bool> funcPredicate = delegate(int n) { return n > 5; };
            
            int testNumber = 7;
            Console.WriteLine($"Custom delegate result for {testNumber}: {customPredicate(testNumber)}");
            Console.WriteLine($"Func delegate result for {testNumber}: {funcPredicate(testNumber)}");
            Console.WriteLine();

            // 9. Multicast with Action (void return type)
            Console.WriteLine("9. Multicast Action delegates:");
            
            Action<string> multiAction = delegate(string msg) 
            { 
                Console.WriteLine($"Handler 1: {msg}"); 
            };
            
            multiAction += delegate(string msg) 
            { 
                Console.WriteLine($"Handler 2: {msg}"); 
            };
            
            multiAction += delegate(string msg) 
            { 
                Console.WriteLine($"Handler 3: {msg}"); 
            };
            
            multiAction("Testing multicast Action");

            Console.ReadKey();
        }
    }
}