using System;
using System.Collections.Generic;
using System.Globalization;

namespace StringInterpolationAndTuples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== String Interpolation and Tuples Demo =====");
            
            // PART 1: STRING INTERPOLATION
            Console.WriteLine("\n1. STRING INTERPOLATION");
            
            // Traditional vs. Interpolated Strings
            string name = "Alice";
            int age = 30;
            double salary = 75000.50;
            DateTime hireDate = new DateTime(2021, 3, 15);
            
            Console.WriteLine("## Traditional vs. Interpolated Strings:");
            
            // Traditional string formatting
            string traditional = string.Format("Employee: {0}, Age: {1}, Salary: {2:C}, Hired: {3:d}", 
                name, age, salary, hireDate);
            
            // String interpolation
            string interpolated = $"Employee: {name}, Age: {age}, Salary: {salary:C}, Hired: {hireDate:d}";
            
            Console.WriteLine($"Traditional: {traditional}");
            Console.WriteLine($"Interpolated: {interpolated}");
            
            // Formatting specifiers and expressions
            Console.WriteLine("\n## Formatting and Expressions:");
            
            // Numeric formatting
            double value = 12345.6789;
            Console.WriteLine($"Currency: {value:C}");
            Console.WriteLine($"Fixed decimal (2 places): {value:F2}");
            Console.WriteLine($"Percent: {0.1756:P1}");
            
            // Date formatting
            DateTime now = DateTime.Now;
            Console.WriteLine($"Short date: {now:d}");
            Console.WriteLine($"Long date: {now:D}");
            Console.WriteLine($"Custom date: {now:yyyy-MM-dd HH:mm}");
            
            // Expressions in interpolation
            int x = 10, y = 5;
            Console.WriteLine($"Calculation: {x} + {y} = {x + y}");
            Console.WriteLine($"Conditional: Status is {(x > y ? "Positive" : "Negative")}");
            
            // Multi-line interpolated strings
            Console.WriteLine("\n## Multi-line interpolation:");
            string multiLine = $@"
Employee Profile:
  Name: {name}
  Age: {age}
  Salary: {salary:C}
  Years until retirement: {65 - age}
";
            Console.WriteLine(multiLine);
            
            // PART 2: TUPLES
            Console.WriteLine("\n2. TUPLES");
            
            // Basic tuple creation
            Console.WriteLine("## Basic Tuples:");
            
            // Method 1: Implicit typing with var
            var person = (name, age, "Developer");
            Console.WriteLine($"Person: {person.name}, {person.age}, {person.Item3}");
            
            // Method 2: Explicit tuple type
            (string Name, int Age, string Role) employee = ("Bob", 42, "Manager");
            Console.WriteLine($"Employee: {employee.Name}, {employee.Age}, {employee.Role}");
            
            // Method 3: Using Tuple.Create (older style)
            var legacyTuple = Tuple.Create("Charlie", 35, "Designer");
            Console.WriteLine($"Legacy: {legacyTuple.Item1}, {legacyTuple.Item2}, {legacyTuple.Item3}");
            
            // Tuple as return value from method
            Console.WriteLine("\n## Method Returns:");
            var stats = CalculateStatistics(new[] { 78, 92, 65, 87, 90 });
            Console.WriteLine($"Stats: Min={stats.Min}, Max={stats.Max}, Avg={stats.Average:F1}");
            
            // Tuple deconstruction
            Console.WriteLine("\n## Deconstruction:");
            
            // Method 1: Into existing variables
            string firstName;
            int userAge;
            (firstName, userAge, _) = employee; // Using discard for Role
            Console.WriteLine($"Deconstructed: name={firstName}, age={userAge}");
            
            // Method 2: Inline declaration
            var (min, max, avg) = CalculateStatistics(new[] { 3, 7, 1, 9, 4 });
            Console.WriteLine($"Inline deconstruction: Min={min}, Max={max}, Avg={avg:F1}");
            
            // PART 3: PRACTICAL APPLICATIONS
            Console.WriteLine("\n3. PRACTICAL APPLICATIONS");
            
            // Combining tuples and string interpolation for data formatting
            Console.WriteLine("## Formatting Database Results:");
            List<(int Id, string Name, DateTime JoinDate, decimal Balance)> customers = new List<(int, string, DateTime, decimal)>
            {
                (1001, "John Smith", new DateTime(2019, 5, 10), 1250.75m),
                (1002, "Lisa Jones", new DateTime(2020, 8, 22), 3750.25m),
                (1003, "David Wilson", new DateTime(2021, 2, 15), 840.50m)
            };
            
            // Generate formatted report
            Console.WriteLine("Customer Report:");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("| ID    | Name         | Join Date  | Balance   |");
            Console.WriteLine("-------------------------------------------");
            
            foreach (var customer in customers)
            {
                Console.WriteLine($"| {customer.Id,-5} | {customer.Name,-12} | {customer.JoinDate:yyyy-MM-dd} | {customer.Balance,9:C} |");
            }
            
            Console.WriteLine("-------------------------------------------");
            
            // Tuple in dictionary key
            Console.WriteLine("\n## Composite Keys with Tuples:");
            var salesData = new Dictionary<(string Region, string Product), int>
            {
                { ("North", "Widgets"), 150 },
                { ("North", "Gadgets"), 200 },
                { ("South", "Widgets"), 100 },
                { ("South", "Gadgets"), 300 }
            };
            
            foreach (var entry in salesData)
            {
                Console.WriteLine($"Sales in {entry.Key.Region} region for {entry.Key.Product}: {entry.Value} units");
            }
            
            // Function arguments with tuples
            Console.WriteLine("\n## Function with Tuple Parameters:");
            
            PrintPersonInfo(("Maria", "Garcia", 28));
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
        
        // Return a tuple of statistics
        static (int Min, int Max, double Average) CalculateStatistics(int[] values)
        {
            if (values == null || values.Length == 0)
                return (0, 0, 0);
                
            int min = values[0];
            int max = values[0];
            int sum = 0;
            
            foreach (int value in values)
            {
                min = Math.Min(min, value);
                max = Math.Max(max, value);
                sum += value;
            }
            
            double average = (double)sum / values.Length;
            
            return (min, max, average);
        }
        
        // Function taking a tuple as a parameter
        static void PrintPersonInfo((string FirstName, string LastName, int Age) person)
        {
            Console.WriteLine($"Person Information:");
            Console.WriteLine($"  Name: {person.FirstName} {person.LastName}");
            Console.WriteLine($"  Age: {person.Age} years old");
            Console.WriteLine($"  Birth Year (approx.): {DateTime.Now.Year - person.Age}");
        }
    }
}