using System;
using System.Collections.Generic;

// Enable nullable reference types for the whole file
#nullable enable

namespace NullableTypesAndSafety
{
    /// <summary>
    /// This program demonstrates nullable value types, nullable reference types,
    /// null-conditional operators, and null-coalescing operators.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Nullable Types and Null Safety Demo =====");
            
            // 1. Nullable Value Types
            Console.WriteLine("\n1. Nullable Value Types:");
            
            // Regular value types cannot be null
            int regularInt = 10;
            // This would cause a compile error: regularInt = null;
            
            // Nullable int can be null
            int? nullableInt = null;
            Console.WriteLine($"Nullable int value: {nullableInt}");
            
            // Check if nullable type has a value
            if (nullableInt.HasValue)
            {
                Console.WriteLine($"Value: {nullableInt.Value}");
            }
            else
            {
                Console.WriteLine("No value (null)");
            }
            
            // Assign a value
            nullableInt = 42;
            Console.WriteLine($"After assignment: {nullableInt}");
            Console.WriteLine($"HasValue: {nullableInt.HasValue}, Value: {nullableInt.Value}");
            
            // 2. Nullable value types with different operators
            Console.WriteLine("\n2. Nullable Value Types with Operators:");
            
            // Nullable operations
            int? a = 5;
            int? b = null;
            
            Console.WriteLine($"a + b = {a + b}");  // Result is null
            Console.WriteLine($"a * 2 = {a * 2}");  // Result is 10
            Console.WriteLine($"b * 2 = {b * 2}");  // Result is null
            
            // Comparisons
            Console.WriteLine($"a > 3? {a > 3}");     // True
            Console.WriteLine($"a < b? {a < b}");     // False (null is not less than any value)
            Console.WriteLine($"a == b? {a == b}");   // False
            Console.WriteLine($"b == null? {b == null}"); // True
            
            // 3. Null-Coalescing Operator (??)
            Console.WriteLine("\n3. Null-Coalescing Operator (??):");
            
            int? c = null;
            int d = c ?? 5;  // d will be 5 because c is null
            Console.WriteLine($"c is {(c.HasValue ? c.ToString() : "null")}, d is {d}");
            
            c = 10;
            int e = c ?? 5;  // e will be 10 because c is not null
            Console.WriteLine($"c is {c}, e is {e}");
            
            // Null-coalescing assignment operator (??=)
            int? f = null;
            f ??= 20;  // f will be assigned 20 because it's null
            Console.WriteLine($"f after ??= 20: {f}");
            
            f ??= 30;  // f will remain 20 because it's not null
            Console.WriteLine($"f after ??= 30: {f}");
            
            // 4. Null-Conditional Operator (?.)
            Console.WriteLine("\n4. Null-Conditional Operator (?.):");
            
            string? name = null;
            // Without null-conditional operator - would throw NullReferenceException
            // int length = name.Length;  
            
            // With null-conditional operator
            int? conditionalLength = name?.Length;
            Console.WriteLine($"Length with null-conditional: {conditionalLength}");
            
            name = "Alice";
            conditionalLength = name?.Length;
            Console.WriteLine($"Length after assignment: {conditionalLength}");
            
            // Chaining null-conditional operators
            List<string?>? names = new List<string?> { "Alice", null, "Bob" };
            List<string?>? nullList = null;
            
            // Safe access to elements
            Console.WriteLine($"First name length: {names?[0]?.Length}");
            Console.WriteLine($"Second name length: {names?[1]?.Length}");  // Will be null
            Console.WriteLine($"Null list first element: {nullList?[0]}");  // Will be null
            
            // 5. Combining Null-Conditional with Null-Coalescing
            Console.WriteLine("\n5. Combining Null Operators:");
            
            string? message = null;
            // Get the length, or -1 if message is null or message.Length is null
            int length = message?.Length ?? -1;
            Console.WriteLine($"Length or default: {length}");
            
            // Chain of operations with null safety
            string? firstName = names?[0];
            string displayName = firstName?.ToUpper() ?? "UNKNOWN";
            Console.WriteLine($"Display name: {displayName}");
            
            // 6. Nullable Reference Types
            Console.WriteLine("\n6. Nullable Reference Types:");
            
            // In a #nullable enable context, reference types are non-nullable by default
            string nonNullableName = "John";  // Cannot be null
            string? nullableName = null;      // Can be null
            
            // The compiler warns about potential null reference exceptions
            // This would give a warning: nonNullableName = null;
            
            // Using nullable reference types in a class
            var person1 = new Person("John", null, 30);
            var person2 = new Person("Jane", "Elizabeth", 25);
            
            // Safe access to nullable properties
            Console.WriteLine($"Person 1: {person1.FirstName} {person1.MiddleName ?? "(no middle name)"} {person1.LastName}");
            Console.WriteLine($"Person 2: {person2.FirstName} {person2.MiddleName} {person2.LastName}");
            
            // 7. Nullable Context and Warnings
            Console.WriteLine("\n7. Nullable Context and Warnings:");
            
            // In a nullable context, the compiler helps prevent null reference exceptions
            Console.WriteLine("In a nullable context:");
            Console.WriteLine("- Reference types are non-nullable by default");
            Console.WriteLine("- Use T? syntax to declare nullable reference types");
            Console.WriteLine("- Compiler warns about potential null references");
            Console.WriteLine("- Forces you to check for null before dereferencing");
            
            // 8. Default values for nullable types
            Console.WriteLine("\n8. Default Values for Nullable Types:");
            
            int? defaultInt = default;
            bool? defaultBool = default;
            DateTime? defaultDate = default;
            string? defaultString = default;
            
            Console.WriteLine($"Default int?: {defaultInt}");
            Console.WriteLine($"Default bool?: {defaultBool}");
            Console.WriteLine($"Default DateTime?: {defaultDate}");
            Console.WriteLine($"Default string?: {defaultString}");
            
            // 9. Practical Example: Safe Configuration Reading
            Console.WriteLine("\n9. Practical Example: Safe Configuration Reading:");
            
            var config = new AppConfig();
            // Safely retrieve potentially missing configuration values
            DisplayConfiguration(config);
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
        
        // Method demonstrating null safety with configuration values
        static void DisplayConfiguration(AppConfig config)
        {
            // Use null-conditional and null-coalescing operators for safety
            string appName = config.ApplicationName ?? "Default App";
            int port = config.Port ?? 8080;
            string? adminEmail = config.AdminEmail;
            bool isDebugMode = config.IsDebugMode ?? false;
            
            Console.WriteLine($"Application: {appName}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Admin Email: {adminEmail ?? "Not configured"}");
            Console.WriteLine($"Debug Mode: {isDebugMode}");
            
            // Safe handling of nested nullable properties
            string dbConnection = config.DatabaseSettings?.ConnectionString ?? "No connection string";
            int timeout = config.DatabaseSettings?.TimeoutSeconds ?? 30;
            Console.WriteLine($"Database Connection: {dbConnection}");
            Console.WriteLine($"Timeout: {timeout} seconds");
        }
    }
    
    class Person
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }  // Nullable - may not have a middle name
        public string LastName { get; set; }
        public int Age { get; set; }
        
        // Constructor with nullable parameter
        public Person(string firstName, string? middleName, int age)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            MiddleName = middleName;  // Can be null
            LastName = "Doe";  // Default value
            Age = age;
        }
        
        // Method demonstrating null-forgiving operator
        public string GetFullName()
        {
            // If we're certain MiddleName won't be null in a particular case,
            // we can use the null-forgiving operator (!)
            if (MiddleName?.Length > 2)
            {
                // null-forgiving operator tells compiler "trust me, it's not null here"
                return $"{FirstName} {MiddleName!.Substring(0, 1)}. {LastName}";
            }
            
            return $"{FirstName} {LastName}";
        }
    }
    
    // Class that demonstrates nullable type constraints
    class Repository<T> where T : class?
    {
        // Can store null values of type T
        private readonly List<T?> _items = new List<T?>();
        
        public void Add(T? item)
        {
            _items.Add(item);
        }
        
        public T? GetFirst()
        {
            return _items.Count > 0 ? _items[0] : default;
        }
        
        public IEnumerable<T> GetNonNullItems()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    yield return item;
                }
            }
        }
    }
    
    // Example class for demonstrating nullable properties in a real-world scenario
    class AppConfig
    {
        // Non-nullable property - always expected to have a value
        public string ApplicationName { get; set; } = "MyApp";
        
        // Nullable properties - may not be set
        public int? Port { get; set; }
        public string? AdminEmail { get; set; }
        public bool? IsDebugMode { get; set; }
        
        // Nullable complex type
        public DatabaseConfig? DatabaseSettings { get; set; }
    }
    
    class DatabaseConfig
    {
        public string? ConnectionString { get; set; }
        public int? TimeoutSeconds { get; set; }
        public bool IsEncrypted { get; set; }
    }
}