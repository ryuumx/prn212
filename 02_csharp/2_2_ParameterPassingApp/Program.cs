using System;

namespace AdvancedParameterPassing
{
    /// <summary>
    /// This program demonstrates advanced parameter passing mechanisms in C#
    /// including ref, out, params, and ref returns.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Advanced Parameter Passing Demo =====");
            
            // 1. Ref Parameters
            Console.WriteLine("\n1. Ref Parameters (Bidirectional):");
            int x = 10;
            Console.WriteLine($"Original value: x = {x}");
            
            ModifyValue(ref x);
            Console.WriteLine($"After ModifyValue: x = {x}");
            
            // 2. Out Parameters
            Console.WriteLine("\n2. Out Parameters (Output only):");
            DivideWithRemainder(10, 3, out int quotient, out int remainder);
            Console.WriteLine($"10 / 3 = {quotient} remainder {remainder}");
            
            // Out parameter doesn't need to be initialized
            GetCurrentDateTime(out DateTime now, out string formattedDate, out string formattedTime);
            Console.WriteLine($"Current date/time: {now}");
            Console.WriteLine($"Formatted date: {formattedDate}");
            Console.WriteLine($"Formatted time: {formattedTime}");
            
            // 3. Params Parameter
            Console.WriteLine("\n3. Params Parameter (Variable Number of Arguments):");
            
            // Different ways to call a method with params
            double avg1 = CalculateAverage(1, 2, 3, 4, 5);
            Console.WriteLine($"Average of 1,2,3,4,5: {avg1}");
            
            int[] numbers = { 10, 20, 30, 40, 50 };
            double avg2 = CalculateAverage(numbers);
            Console.WriteLine($"Average of 10,20,30,40,50: {avg2}");
            
            double avg3 = CalculateAverage();  // Empty array
            Console.WriteLine($"Average of empty array: {avg3}");
            
            // 4. Ref Locals and Returns
            Console.WriteLine("\n4. Ref Locals and Returns:");
            
            // Array to demonstrate ref returns
            int[] array = { 1, 2, 3, 4, 5 };
            Console.WriteLine("Original array: " + string.Join(", ", array));
            
            // Get a reference to the largest element
            ref int largestElement = ref FindLargest(array);
            Console.WriteLine($"Largest element: {largestElement}");
            
            // Modify the array through the reference
            largestElement = 10;  // This changes the value in the original array
            Console.WriteLine("Array after modification: " + string.Join(", ", array));
            
            // 5. Ref readonly
            Console.WriteLine("\n5. Ref Readonly Parameters:");
            Person person = new Person { Name = "John", Age = 30 };
            PrintPersonDetails(in person);  // 'in' is like 'ref' but prevents modification
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
        
        // Ref parameter example - bidirectional (input and output)
        static void ModifyValue(ref int value)
        {
            Console.WriteLine($"  Inside ModifyValue, received value: {value}");
            value = value * 2;  // Modify the original variable
            Console.WriteLine($"  Inside ModifyValue, modified to: {value}");
        }
        
        // Out parameters example - output only
        static void DivideWithRemainder(int dividend, int divisor, out int quotient, out int remainder)
        {
            quotient = dividend / divisor;
            remainder = dividend % divisor;
            
            // Out parameters must be assigned before the method returns
        }
        
        // Multiple out parameters
        static void GetCurrentDateTime(out DateTime now, out string formattedDate, out string formattedTime)
        {
            now = DateTime.Now;
            formattedDate = now.ToString("yyyy-MM-dd");
            formattedTime = now.ToString("HH:mm:ss");
        }
        
        // Params parameter example - variable number of arguments
        static double CalculateAverage(params int[] numbers)
        {
            if (numbers.Length == 0)
                return 0;
                
            int sum = 0;
            foreach (int num in numbers)
            {
                sum += num;
            }
            
            return (double)sum / numbers.Length;
        }
        
        // Ref return example - returns a reference to an array element
        static ref int FindLargest(int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                throw new ArgumentException("Array cannot be null or empty");
                
            int largestIndex = 0;
            
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] > numbers[largestIndex])
                    largestIndex = i;
            }
            
            // Return a reference to the largest element
            return ref numbers[largestIndex];
        }
        
        // Simple Person class for demonstration
        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        
        // In parameter example (ref readonly) - prevents modification
        static void PrintPersonDetails(in Person person)
        {
            Console.WriteLine($"Person: {person.Name}, {person.Age} years old");
            
            // The following line would cause a compilation error
            // person = new Person();  // Error: Cannot assign to parameter 'person' because it is read-only
            
            // However, this is allowed (modifying the object's properties)
            // person.Name = "Jane";  // Works fine
            
            // For true immutability, use a readonly struct
        }
    }
}