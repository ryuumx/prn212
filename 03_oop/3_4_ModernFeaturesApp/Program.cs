using System;

namespace ModernCSharpFeatures
{
    // Record type (C# 9.0+)
    public record Person(string FirstName, string LastName, int Age);
    
    // More detailed record with methods and additional properties
    public record Employee
    {
        // Init-only properties
        public string FirstName { get; init; }
        public string LastName { get; init; }
        
        // Regular property
        public int Age { get; set; }
        
        // Required property (C# 11+)
        public required string Department { get; init; }
        
        // Expression-bodied read-only property
        public string FullName => $"{FirstName} {LastName}";
        
        // Constructor (can have multiple)
        public Employee(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        
        // Methods
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Employee: {FullName}, {Age}, Dept: {Department}");
        }
        
        // Expression-bodied method
        public decimal CalculateSalary(decimal baseSalary) => baseSalary * 1.1m;
    }
    
    // Extended record type
    public record Manager : Employee
    {
        public int TeamSize { get; init; }
        
        public Manager(string firstName, string lastName) : base(firstName, lastName)
        {
        }
        
        // Override method
        public override void DisplayInfo()
        {
            Console.WriteLine($"Manager: {FullName}, {Age}, Dept: {Department}, Team: {TeamSize}");
        }
    }
    
    // Static class example
    public static class StringExtensions
    {
        // Extension method
        public static bool IsValidEmail(this string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
        }
        
        public static string Truncate(this string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Record type
            Person person1 = new Person("John", "Doe", 30);
            Console.WriteLine(person1); // Auto-generated ToString
            
            // Records use value-based equality
            Person person2 = new Person("John", "Doe", 30);
            Console.WriteLine($"Equal? {person1 == person2}"); // True
            
            // Immutability and with-expressions
            Person person3 = person1 with { LastName = "Smith" };
            Console.WriteLine(person3);
            
            // Record with init properties
            var employee = new Employee("Jane", "Smith")
            {
                Age = 32,
                Department = "Engineering"
            };
            
            Console.WriteLine(employee.FullName);
            employee.Age = 33; // Can change non-init properties
            // employee.FirstName = "Janet"; // Error - can't change init-only property
            
            // Using extension methods
            string email = "test@example.com";
            Console.WriteLine($"Valid email? {email.IsValidEmail()}");
            
            string longText = "This is a very long text that should be truncated";
            Console.WriteLine(longText.Truncate(20));
            
            // Pattern matching
            object obj = new Manager("Bob", "Johnson") { Age = 45, Department = "Sales", TeamSize = 10 };
            
            // Type pattern
            if (obj is Employee emp)
            {
                Console.WriteLine($"Found employee: {emp.FullName}");
            }
            
            // Property pattern
            if (obj is Manager { TeamSize: > 5 } manager)
            {
                Console.WriteLine($"Found manager with large team: {manager.TeamSize}");
            }
            
            // Switch expression
            var description = obj switch
            {
                Manager m => $"Manager with {m.TeamSize} employees",
                Employee e => $"Regular employee in {e.Department}",
                Person p => $"Person: {p.FirstName}",
                _ => "Unknown object"
            };
            
            Console.WriteLine(description);
        }
    }
}