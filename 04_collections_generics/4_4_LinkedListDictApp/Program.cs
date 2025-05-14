using System;
using System.Collections.Generic;

namespace CollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LinkedList<T> Demo ===");
            DemonstrateLinkedList();
            
            Console.WriteLine("\n=== Dictionary<TKey, TValue> Demo ===");
            DemonstrateDictionary();
            
            Console.ReadLine();
        }
        
        static void DemonstrateLinkedList()
        {
            // Create a new LinkedList of integers
            LinkedList<int> numbers = new LinkedList<int>();
            
            // Add elements at the start and end
            numbers.AddFirst(1);  // List is now: 1
            numbers.AddLast(4);   // List is now: 1, 4
            numbers.AddFirst(0);  // List is now: 0, 1, 4
            numbers.AddLast(5);   // List is now: 0, 1, 4, 5
            
            Console.WriteLine("Initial LinkedList:");
            foreach (int number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Insert elements in the middle
            LinkedListNode<int> node = numbers.Find(1);
            numbers.AddAfter(node, 2);   // List is now: 0, 1, 2, 4, 5
            numbers.AddAfter(node.Next, 3); // List is now: 0, 1, 2, 3, 4, 5
            
            Console.WriteLine("After insertions:");
            foreach (int number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Accessing nodes directly
            Console.WriteLine($"First: {numbers.First.Value}");
            Console.WriteLine($"Last: {numbers.Last.Value}");
            
            // Removing nodes
            numbers.Remove(3);  // Remove by value
            numbers.RemoveFirst(); // Remove first node
            numbers.RemoveLast();  // Remove last node
            
            Console.WriteLine("After removals:");
            foreach (int number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Traversing in both directions
            Console.WriteLine("\nTraversing forwards:");
            LinkedListNode<int> current = numbers.First;
            while (current != null)
            {
                Console.Write($"{current.Value} ");
                current = current.Next;
            }
            Console.WriteLine();
            
            Console.WriteLine("Traversing backwards:");
            current = numbers.Last;
            while (current != null)
            {
                Console.Write($"{current.Value} ");
                current = current.Previous;
            }
            Console.WriteLine();
            
            // Working with a LinkedList of custom objects
            LinkedList<Person> people = new LinkedList<Person>();
            people.AddLast(new Person { FirstName = "John", LastName = "Doe", Age = 30 });
            people.AddLast(new Person { FirstName = "Jane", LastName = "Smith", Age = 25 });
            people.AddLast(new Person { FirstName = "Bob", LastName = "Johnson", Age = 45 });
            
            Console.WriteLine("\nLinkedList of people:");
            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }
        }
        
        static void DemonstrateDictionary()
        {
            // Creating a Dictionary
            Dictionary<string, Person> peopleBySSN = new Dictionary<string, Person>();
            
            // Adding key-value pairs
            peopleBySSN.Add("123-45-6789", new Person { FirstName = "John", LastName = "Doe", Age = 30 });
            peopleBySSN.Add("987-65-4321", new Person { FirstName = "Jane", LastName = "Smith", Age = 25 });
            
            // Alternative syntax for adding
            peopleBySSN["555-55-5555"] = new Person { FirstName = "Bob", LastName = "Johnson", Age = 45 };
            
            // Accessing values
            Console.WriteLine("Looking up by SSN 123-45-6789:");
            Person person = peopleBySSN["123-45-6789"];
            Console.WriteLine($"Found: {person}");
            
            // Safe lookup using TryGetValue
            Console.WriteLine("\nSafe lookup for SSN 999-99-9999:");
            if (peopleBySSN.TryGetValue("999-99-9999", out Person unknownPerson))
            {
                Console.WriteLine($"Found: {unknownPerson}");
            }
            else
            {
                Console.WriteLine("Person not found!");
            }
            
            // Checking if key exists
            string ssnToCheck = "555-55-5555";
            Console.WriteLine($"\nDoes dictionary contain SSN {ssnToCheck}? {peopleBySSN.ContainsKey(ssnToCheck)}");
            
            // Iterating through key-value pairs
            Console.WriteLine("\nAll people in dictionary:");
            foreach (KeyValuePair<string, Person> pair in peopleBySSN)
            {
                Console.WriteLine($"SSN: {pair.Key}, Person: {pair.Value}");
            }
            
            // Working with keys and values collections
            Console.WriteLine("\nAll SSNs:");
            foreach (string ssn in peopleBySSN.Keys)
            {
                Console.WriteLine(ssn);
            }
            
            Console.WriteLine("\nAll People:");
            foreach (Person p in peopleBySSN.Values)
            {
                Console.WriteLine(p);
            }
            
            // Removing items
            peopleBySSN.Remove("123-45-6789");
            Console.WriteLine($"\nCount after removing one person: {peopleBySSN.Count}");
            
            // Dictionary with other key types
            Console.WriteLine("\nDictionary with enum keys:");
            Dictionary<DayOfWeek, string> schedule = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Team Meeting" },
                { DayOfWeek.Wednesday, "Project Review" },
                { DayOfWeek.Friday, "Weekly Report" }
            };
            
            foreach (var appointment in schedule)
            {
                Console.WriteLine($"{appointment.Key}: {appointment.Value}");
            }
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