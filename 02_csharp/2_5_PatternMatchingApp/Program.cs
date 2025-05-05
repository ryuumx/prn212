using System;
using System.Collections.Generic;

namespace PatternMatching
{
    /// <summary>
    /// This program demonstrates various pattern matching techniques in C#
    /// including type patterns, property patterns, tuple patterns, and switch expressions.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== Pattern Matching Techniques Demo =====");
            
            // 1. Type Pattern Matching with 'is'
            Console.WriteLine("\n1. Type Pattern Matching with 'is':");
            
            object[] objects = { 
                "Hello", 
                42, 
                3.14, 
                true, 
                new Person("Alice", 25), 
                null,
                new DateTime(2023, 1, 1)
            };
            
            foreach (var obj in objects)
            {
                IdentifyType(obj);
            }
            
            // 2. Switch Pattern Matching
            Console.WriteLine("\n2. Switch Pattern Matching:");
            
            foreach (var obj in objects)
            {
                string result = IdentifyTypeWithSwitch(obj);
                Console.WriteLine($"  Switch pattern result: {result}");
            }
            
            // 3. Property Pattern Matching
            Console.WriteLine("\n3. Property Pattern Matching:");
            
            Person[] people = {
                new Person("Alice", 25),
                new Person("Bob", 17),
                new Person("Charlie", 65),
                new Person("Diana", 35),
                new Person("", 30)
            };
            
            foreach (var person in people)
            {
                ClassifyPerson(person);
            }
            
            // 4. Tuple Pattern Matching
            Console.WriteLine("\n4. Tuple Pattern Matching:");
            
            CheckCoordinates((0, 0));
            CheckCoordinates((0, 5));
            CheckCoordinates((5, 0));
            CheckCoordinates((3, 4));
            
            // 5. Switch Expressions (Modern C# Feature)
            Console.WriteLine("\n5. Switch Expressions:");
            
            foreach (var person in people)
            {
                string category = GetAgeCategory(person);
                Console.WriteLine($"  {person.Name} ({person.Age}) is {category}");
            }
            
            // 6. Combined Patterns
            Console.WriteLine("\n6. Combined Patterns:");
            
            ProcessValue(42);
            ProcessValue("Hello");
            ProcessValue(null);
            ProcessValue(new List<int> { 1, 2, 3 });
            ProcessValue(new List<int>());
            ProcessValue(new Person("Alice", 25));
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
        
        // Type pattern matching with 'is'
        static void IdentifyType(object obj)
        {
            // Traditional approach
            if (obj is string)
            {
                string s = (string)obj;
                Console.WriteLine($"  String value: \"{s}\" (Length: {s.Length})");
            }
            
            // Type pattern with declaration
            if (obj is int intValue)
            {
                Console.WriteLine($"  Integer value: {intValue}");
            }
            
            // Type patterns can check for null
            if (obj is null)
            {
                Console.WriteLine("  Null value");
            }
            
            // Type pattern with additional condition
            if (obj is double d && d > 3)
            {
                Console.WriteLine($"  Double value greater than 3: {d}");
            }
            
            // Type pattern with class
            if (obj is Person person && !string.IsNullOrEmpty(person.Name))
            {
                Console.WriteLine($"  Person: {person.Name}, {person.Age} years old");
            }
        }
        
        // Switch pattern matching
        static string IdentifyTypeWithSwitch(object obj)
        {
            switch (obj)
            {
                case string s:
                    return $"String: \"{s}\" (Length: {s.Length})";
                
                case int i:
                    return $"Integer: {i}";
                
                case double d when d > 3:
                    return $"Double > 3: {d}";
                
                case double d:
                    return $"Double: {d}";
                
                case bool b:
                    return $"Boolean: {b}";
                
                case Person p when string.IsNullOrEmpty(p.Name):
                    return "Person with no name";
                
                case Person p:
                    return $"Person: {p.Name}, {p.Age} years old";
                
                case null:
                    return "Null value";
                
                default:
                    return $"Unknown type: {obj.GetType().Name}";
            }
        }
        
        // Property pattern matching
        static void ClassifyPerson(Person person)
        {
            // Property pattern with the 'is' operator
            if (person is { Age: >= 65 })
            {
                Console.WriteLine($"  {person.Name} is a senior");
            }
            else if (person is { Age: < 18 })
            {
                Console.WriteLine($"  {person.Name} is a minor");
            }
            else if (person is { Name: "" } or { Name: null })
            {
                Console.WriteLine($"  Anonymous person, age {person.Age}");
            }
            else
            {
                Console.WriteLine($"  {person.Name} is an adult");
            }
        }
        
        // Tuple pattern matching
        static void CheckCoordinates((int X, int Y) point)
        {
            string description = point switch
            {
                (0, 0) => "Origin",
                (0, _) => "Y-axis",
                (_, 0) => "X-axis",
                var (x, y) when x == y => "Diagonal",
                var (x, y) => $"Point ({x}, {y})"
            };
            
            Console.WriteLine($"  Coordinate {point}: {description}");
        }
        
        // Switch expression (Modern C# feature)
        static string GetAgeCategory(Person person) => person.Age switch
        {
            < 13 => "a child",
            >= 13 and < 18 => "a teenager",
            >= 18 and < 65 => "an adult",
            >= 65 => "a senior",
            //_ => "unknown age category"
        };
        
        // Combined patterns
        static void ProcessValue(object value)
        {
            string result = value switch
            {
                null => "Null value",
                
                // Type + condition pattern
                int i when i > 0 => $"Positive integer: {i}",
                int i when i < 0 => $"Negative integer: {i}",
                int i => $"Zero integer",
                
                // Type + property pattern
                string { Length: 0 } => "Empty string",
                string s => $"String with length {s.Length}",
                
                // Collection pattern
                ICollection<int> { Count: 0 } => "Empty integer collection",
                ICollection<int> c => $"Collection with {c.Count} integers",
                
                // Property pattern
                Person { Name: null or "" } => "Anonymous person",
                Person p => $"Person: {p.Name}, {p.Age} years old",
                
                // Default case (discard pattern)
                _ => $"Unhandled type: {value.GetType().Name}"
            };
            
            Console.WriteLine($"  Result: {result}");
        }
    }
    
    // Simple Person class for demonstration
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}