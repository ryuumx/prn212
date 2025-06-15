using System;

namespace SerializationDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Chapter 10: XML and JSON Serialization Demonstrations ===\n");
            
            try
            {
                // Demo 1: XML Serialization
                XmlSerializationDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to XML Data Provider demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 2: XML Data Provider
                XmlDataProviderDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to JSON demos...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 3: Basic JSON Serialization
                JsonSerializationDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to advanced JSON features...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 4: Advanced JSON Features
                AdvancedJsonDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to comparison...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 5: Comparison and Best Practices
                SerializationComparison.RunDemo();
                
                Console.WriteLine("\n=== All demonstrations completed! ===");
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