using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomCollectionsDemo
{
    // Custom collection that implements IEnumerable<T>
    public class PersonCollection : IEnumerable<Person>
    {
        private List<Person> _people = new List<Person>();
        
        // Add functionality
        public void Add(Person person)
        {
            _people.Add(person);
        }
        
        // Add range functionality
        public void AddRange(params Person[] people)
        {
            _people.AddRange(people);
        }
        
        // Filtered view functionality
        public IEnumerable<Person> GetAdults()
        {
            foreach (Person person in _people)
            {
                if (person.Age >= 18)
                {
                    yield return person;
                }
            }
        }
        
        // Get people by last name
        public IEnumerable<Person> GetByLastName(string lastName)
        {
            foreach (Person person in _people)
            {
                if (person.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return person;
                }
            }
        }
        
        // Implementation of IEnumerable<Person>
        public IEnumerator<Person> GetEnumerator()
        {
            return _people.GetEnumerator();
        }
        
        // Implementation of IEnumerable (non-generic)
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    // Another custom collection - a simple generic stack
    public class SimpleStack<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        
        public int Count => _items.Count;
        
        public void Push(T item)
        {
            _items.Add(item);
        }
        
        public T Pop()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Stack is empty");
                
            T item = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            return item;
        }
        
        public T Peek()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Stack is empty");
                
            return _items[_items.Count - 1];
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            // Return items in LIFO order for enumeration
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== CustomCollection Demo ===");
            DemonstratePersonCollection();
            
            Console.WriteLine("\n=== Generic Stack Demo ===");
            DemonstrateGenericStack();
            
            Console.ReadLine();
        }
        
        static void DemonstratePersonCollection()
        {
            // Create the collection
            PersonCollection people = new PersonCollection();
            
            // Add some people
            people.AddRange(
                new Person { FirstName = "David", LastName = "Simpson", Age = 50 },
                new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 },
                new Person { FirstName = "Lisa", LastName = "Simpson", Age = 19 },
                new Person { FirstName = "Jack", LastName = "Simpson", Age = 16 },
                new Person { FirstName = "Bob", LastName = "Johnson", Age = 35 }
            );
            
            // Iterate using foreach (uses IEnumerable<T>)
            Console.WriteLine("All people:");
            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }
            
            // Using the filtered view
            Console.WriteLine("\nAdults only:");
            foreach (Person adult in people.GetAdults())
            {
                Console.WriteLine(adult);
            }
            
            // Using another filtered view
            Console.WriteLine("\nSimpsons only:");
            foreach (Person simpson in people.GetByLastName("Simpson"))
            {
                Console.WriteLine(simpson);
            }
            
            // Show that LINQ works on our collection (because it implements IEnumerable<T>)
            Console.WriteLine("\nPeople sorted by age (using LINQ):");
            foreach (Person person in people.OrderBy(p => p.Age))
            {
                Console.WriteLine(person);
            }
            
            Console.WriteLine("\nPeople with 'a' in their first name (using LINQ):");
            var filtered = people.Where(p => p.FirstName.Contains("a"));
            foreach (Person person in filtered)
            {
                Console.WriteLine(person);
            }
        }
        
        static void DemonstrateGenericStack()
        {
            // Create a stack of strings
            SimpleStack<string> stack = new SimpleStack<string>();
            
            // Push items onto the stack
            stack.Push("First");
            stack.Push("Second");
            stack.Push("Third");
            
            Console.WriteLine($"Items in stack: {stack.Count}");
            Console.WriteLine($"Top item: {stack.Peek()}");
            
            // Pop an item
            string popped = stack.Pop();
            Console.WriteLine($"Popped: {popped}");
            Console.WriteLine($"Items in stack: {stack.Count}");
            
            // Enumerate the stack (in LIFO order)
            Console.WriteLine("\nStack contents (LIFO order):");
            foreach (string item in stack)
            {
                Console.WriteLine(item);
            }
            
            // Creating a stack of integers
            SimpleStack<int> intStack = new SimpleStack<int>();
            intStack.Push(10);
            intStack.Push(20);
            intStack.Push(30);
            
            Console.WriteLine("\nInteger stack:");
            foreach (int item in intStack)
            {
                Console.WriteLine(item);
            }
            
            // Using LINQ with our custom collection
            var sum = intStack.Sum();
            Console.WriteLine($"Sum of items in integer stack: {sum}");
        }
    }
    
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        
        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, Age: {Age}";
        }
    }
}