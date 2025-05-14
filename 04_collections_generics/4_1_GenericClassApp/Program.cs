using System;

namespace GenericDemo
{
    // Simple generic class
    public class Box<T>
    {
        private T _item;
        
        public T Item 
        { 
            get => _item; 
            set => _item = value; 
        }
        
        public Box(T item)
        {
            _item = item;
        }
        
        public override string ToString()
        {
            return $"Box containing: {_item}";
        }
    }
    
    // Generic class with constraint
    public class NumberBox<T> where T : struct, IComparable<T>
    {
        private T _number;
        
        public T Number
        {
            get => _number;
            set => _number = value;
        }
        
        public NumberBox(T number)
        {
            _number = number;
        }
        
        public bool IsGreaterThan(T other)
        {
            return _number.CompareTo(other) > 0;
        }
        
        public override string ToString()
        {
            return $"NumberBox containing: {_number}";
        }
    }
    
    public class GenericMethodsDemo
    {
        // Simple generic swap method
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        
        // Generic method with constraints
        public static T Max<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) > 0 ? a : b;
        }
        
        // Generic method with multiple type parameters
        public static void Display<T, U>(T message, U value)
        {
            Console.WriteLine($"{message}: {value}");
        }
        
        // Generic method that creates a new instance
        public static T CreateDefault<T>() where T : new()
        {
            return new T();
        }
    }
    
    // Main demo class
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Classes Demo ===");
            
            // Using Box<T> with different types
            Box<string> stringBox = new Box<string>("Hello Generic World");
            Console.WriteLine(stringBox);
            
            Box<int> intBox = new Box<int>(42);
            Console.WriteLine(intBox);
            
            // Using NumberBox<T> with constraints
            NumberBox<int> intNumberBox = new NumberBox<int>(100);
            Console.WriteLine(intNumberBox);
            Console.WriteLine($"Is {intNumberBox.Number} greater than 50? {intNumberBox.IsGreaterThan(50)}");
            
            NumberBox<double> doubleNumberBox = new NumberBox<double>(3.14);
            Console.WriteLine(doubleNumberBox);
            
            // This won't compile - string doesn't satisfy the struct constraint
            // NumberBox<string> stringNumberBox = new NumberBox<string>("Hello");
            
            Console.WriteLine("\n=== Generic Methods Demo ===");
            
            // Using the swap method
            int a = 5, b = 10;
            Console.WriteLine($"Before swap: a = {a}, b = {b}");
            GenericMethodsDemo.Swap(ref a, ref b);
            Console.WriteLine($"After swap: a = {a}, b = {b}");
            
            // Swap with different types
            string first = "Hello", second = "World";
            Console.WriteLine($"Before swap: first = {first}, second = {second}");
            GenericMethodsDemo.Swap(ref first, ref second);
            Console.WriteLine($"After swap: first = {first}, second = {second}");
            
            // Using the Max method with constraints
            Console.WriteLine($"Max of 42 and 99: {GenericMethodsDemo.Max(42, 99)}");
            Console.WriteLine($"Max of 'apple' and 'banana': {GenericMethodsDemo.Max("apple", "banana")}");
            
            // Using the Display method with different type combinations
            GenericMethodsDemo.Display("Integer", 2050);
            GenericMethodsDemo.Display("Double", 155.9);
            GenericMethodsDemo.Display(358.9, 255.67);
            
            // Using the CreateDefault method
            var defaultPerson = GenericMethodsDemo.CreateDefault<Person>();
            Console.WriteLine($"Default person: {defaultPerson}");
            
            Console.ReadLine();
        }
    }
    
    public class Person
    {
        public string FirstName { get; set; } = "Unknown";
        public string LastName { get; set; } = "Person";
        public int Age { get; set; }
        
        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, Age: {Age}";
        }
    }
}