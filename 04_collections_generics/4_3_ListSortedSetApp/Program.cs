using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CollectionsDemo
{
    class Program
    {
        private const int COUNT = 1000000;
        
        static void Main(string[] args)
        {
            Console.WriteLine("=== List<T> Demo ===");
            DemonstrateList();
            
            Console.WriteLine("\n=== List<T> vs ArrayList Performance ===");
            CompareListPerformance();
            
            Console.WriteLine("\n=== SortedSet<T> Demo ===");
            DemonstrateSortedSet();
            
            Console.ReadLine();
        }
        
        static void DemonstrateList()
        {
            // Creating a list of strings
            List<string> names = new List<string>
            {
                "Alice", "Bob", "Charlie", "Dave"
            };
            
            // Display initial list
            Console.WriteLine("Initial list:");
            foreach (string name in names)
            {
                Console.Write($"{name} ");
            }
            Console.WriteLine();
            
            // Adding elements
            names.Add("Eve");
            Console.WriteLine($"Count after adding Eve: {names.Count}");
            
            // Inserting at specific position
            names.Insert(2, "Frank");
            Console.WriteLine("After inserting Frank at position 2:");
            foreach (string name in names)
            {
                Console.Write($"{name} ");
            }
            Console.WriteLine();
            
            // Removing elements
            names.Remove("Bob");
            Console.WriteLine("After removing Bob:");
            foreach (string name in names)
            {
                Console.Write($"{name} ");
            }
            Console.WriteLine();
            
            // Checking if element exists
            bool containsDave = names.Contains("Dave");
            Console.WriteLine($"List contains Dave: {containsDave}");
            
            // Finding elements
            int index = names.FindIndex(n => n.StartsWith("C"));
            Console.WriteLine($"Index of first name starting with C: {index}");
            
            // Sorting
            names.Sort();
            Console.WriteLine("After sorting:");
            foreach (string name in names)
            {
                Console.Write($"{name} ");
            }
            Console.WriteLine();
            
            // Using List<T> with custom objects
            List<Person> people = new List<Person>
            {
                new Person { FirstName = "John", LastName = "Doe", Age = 30 },
                new Person { FirstName = "Jane", LastName = "Smith", Age = 25 },
                new Person { FirstName = "Bob", LastName = "Johnson", Age = 45 }
            };
            
            Console.WriteLine("\nList of people:");
            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }
            
            // Finding with predicates
            Person adult = people.Find(p => p.Age >= 30);
            Console.WriteLine($"\nFirst adult: {adult}");
            
            // Sorting with comparer
            people.Sort((p1, p2) => p1.Age.CompareTo(p2.Age));
            Console.WriteLine("\nPeople sorted by age:");
            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }
        }
        
        static void CompareListPerformance()
        {
            // Adding elements performance test
            Console.WriteLine("Adding elements performance test:");
            MeasureListAdd();
            MeasureArrayListAdd();
            
            // Reading elements performance test
            Console.WriteLine("\nReading elements performance test:");
            MeasureListRead();
            MeasureArrayListRead();
        }
        
        static void MeasureListAdd()
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<int> list = new List<int>();
            for (int i = 0; i < COUNT; i++)
            {
                list.Add(i);
            }
            sw.Stop();
            Console.WriteLine($"List<int> Add: {sw.ElapsedMilliseconds}ms");
        }
        
        static void MeasureArrayListAdd()
        {
            Stopwatch sw = Stopwatch.StartNew();
            ArrayList list = new ArrayList();
            for (int i = 0; i < COUNT; i++)
            {
                list.Add(i); // Boxing occurs here
            }
            sw.Stop();
            Console.WriteLine($"ArrayList Add: {sw.ElapsedMilliseconds}ms");
        }
        
        static void MeasureListRead()
        {
            List<int> list = new List<int>(COUNT);
            for (int i = 0; i < COUNT; i++)
            {
                list.Add(i);
            }
            
            Stopwatch sw = Stopwatch.StartNew();
            int sum = 0;
            for (int i = 0; i < COUNT; i++)
            {
                sum += list[i]; // No unboxing needed
            }
            sw.Stop();
            Console.WriteLine($"List<int> Read: {sw.ElapsedMilliseconds}ms");
        }
        
        static void MeasureArrayListRead()
        {
            ArrayList list = new ArrayList(COUNT);
            for (int i = 0; i < COUNT; i++)
            {
                list.Add(i);
            }
            
            Stopwatch sw = Stopwatch.StartNew();
            int sum = 0;
            for (int i = 0; i < COUNT; i++)
            {
                sum += (int)list[i]; // Unboxing occurs here
            }
            sw.Stop();
            Console.WriteLine($"ArrayList Read: {sw.ElapsedMilliseconds}ms");
        }
        
        static void DemonstrateSortedSet()
        {
            // Create a SortedSet of integers
            SortedSet<int> numbers = new SortedSet<int> { 8, 7, 9, 1, 3 };
            
            // Display initial set
            Console.WriteLine("Initial SortedSet (automatically sorted):");
            foreach (int number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Add more elements
            numbers.Add(5);
            numbers.Add(4);
            numbers.Add(6);
            numbers.Add(2);
            
            // Try adding a duplicate
            bool added = numbers.Add(5);
            Console.WriteLine($"Was 5 added again? {added}");
            
            // Display the sorted elements
            Console.WriteLine("SortedSet after additions:");
            foreach (int number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Check if elements exist
            Console.WriteLine($"Contains 4? {numbers.Contains(4)}");
            Console.WriteLine($"Contains 10? {numbers.Contains(10)}");
            
            // Remove elements
            numbers.Remove(5);
            Console.WriteLine("After removing 5:");
            foreach (int number in numbers)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Set operations
            SortedSet<int> set1 = new SortedSet<int> { 1, 3, 5, 7, 9 };
            SortedSet<int> set2 = new SortedSet<int> { 2, 3, 5, 7, 11 };
            
            // Create a new set with the union
            SortedSet<int> union = new SortedSet<int>(set1);
            union.UnionWith(set2);
            Console.WriteLine("\nUnion of sets:");
            foreach (int number in union)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Create a new set with the intersection
            SortedSet<int> intersection = new SortedSet<int>(set1);
            intersection.IntersectWith(set2);
            Console.WriteLine("\nIntersection of sets:");
            foreach (int number in intersection)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
            
            // Create a new set with the difference
            SortedSet<int> difference = new SortedSet<int>(set1);
            difference.ExceptWith(set2);
            Console.WriteLine("\nDifference of sets (set1 - set2):");
            foreach (int number in difference)
            {
                Console.Write($"{number} ");
            }
            Console.WriteLine();
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