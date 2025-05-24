using System;

namespace DelegateAndPassing
{
    // 1. Define delegate type
    public delegate int MathOperation(int x, int y);
    public delegate void PrintDelegate(string message);

    public class DelegateBasicsAndPassing
    {
        // Basic math operations
        public static int Add(int a, int b)
        {
            Console.WriteLine($"Adding {a} + {b}");
            return a + b;
        }

        public static int Subtract(int a, int b)
        {
            Console.WriteLine($"Subtracting {a} - {b}");
            return a - b;
        }

        public static int Multiply(int a, int b)
        {
            Console.WriteLine($"Multiplying {a} * {b}");
            return a * b;
        }

        // Method that accepts delegate as parameter
        public static void ProcessNumbers(int x, int y, MathOperation operation)
        {
            Console.WriteLine($"Processing numbers {x} and {y}");
            int result = operation(x, y);
            Console.WriteLine($"Result: {result}");
            Console.WriteLine(new string('-', 30));
        }

        // Method for printing
        public static void PrintToConsole(string msg)
        {
            Console.WriteLine($"Console: {msg}");
        }

        public static void PrintToLog(string msg)
        {
            Console.WriteLine($"LOG: {DateTime.Now} - {msg}");
        }

        // Demonstrating delegate properties
        public static void ShowDelegateProperties(MathOperation del)
        {
            Console.WriteLine($"Delegate Target: {del.Target}");
            Console.WriteLine($"Delegate Method: {del.Method.Name}");
            Console.WriteLine($"Delegate Method Return Type: {del.Method.ReturnType}");
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("=== DELEGATE BASICS ===");
            
            // 2. Instantiate delegates
            MathOperation mathDel = Add;
            
            // 3. Use delegate
            int result = mathDel(10, 5);
            Console.WriteLine($"Direct call result: {result}");
            Console.WriteLine();

            // Show delegate properties
            Console.WriteLine("=== DELEGATE PROPERTIES ===");
            ShowDelegateProperties(mathDel);
            Console.WriteLine();

            // 4. Reassign delegate
            mathDel = Subtract;
            result = mathDel(10, 5);
            Console.WriteLine($"After reassignment: {result}");
            Console.WriteLine();

            Console.WriteLine("=== PASSING DELEGATES AS PARAMETERS ===");
            
            // Passing different operations
            ProcessNumbers(15, 3, Add);
            ProcessNumbers(15, 3, Subtract);
            ProcessNumbers(15, 3, Multiply);

            // Using delegate with different methods
            Console.WriteLine("=== DELEGATE WITH PRINT METHODS ===");
            PrintDelegate printDel = PrintToConsole;
            printDel("Hello from Console!");

            printDel = PrintToLog;
            printDel("Hello from Log!");

            Console.WriteLine();

            // Demonstrating delegate flexibility
            Console.WriteLine("=== ARRAY OF DELEGATES ===");
            MathOperation[] operations = { Add, Subtract, Multiply };
            
            for (int i = 0; i < operations.Length; i++)
            {
                Console.WriteLine($"Operation {i + 1}:");
                ProcessNumbers(8, 2, operations[i]);
            }

            Console.ReadKey();
        }
    }
}