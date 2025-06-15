using System;

namespace ConcurrencyDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Chapter 11: Concurrency Programming Demonstrations ===\n");
            
            try
            {
                // Demo 1: Basic Threading
                BasicThreadingDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Thread Communication demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 2: Thread Communication
                ThreadCommunicationDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Race Conditions demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 3: Race Conditions and Synchronization
                RaceConditionDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to ThreadPool demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 4: ThreadPool
                ThreadPoolDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Timer demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 5: Timer
                TimerDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Advanced Concurrency...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 6: Advanced Concurrency Patterns
                AdvancedConcurrencyDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Best Practices...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 7: Best Practices
                BestPracticesDemo.RunDemo();
                
                Console.WriteLine("\n=== All concurrency demonstrations completed! ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}