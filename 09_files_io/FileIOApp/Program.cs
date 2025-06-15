using System;

namespace FileIODemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Chapter 09: File and System.IO Demonstrations ===\n");
            
            try
            {
                // Demo 1: Basic FileStream
                BasicFileStreamDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to File class demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 2: File Class
                FileClassDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to FileInfo demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 3: FileInfo Class
                FileInfoDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Directory demo...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 4: Directory Operations
                DirectoryDemo.RunDemo();
                Console.WriteLine("\nPress any key to continue to Text operations...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 5: Text File Operations
                TextFileOperations.RunDemo();
                Console.WriteLine("\nPress any key to continue to Binary operations...");
                Console.ReadKey();
                Console.Clear();
                
                // Demo 6: Binary File Operations
                BinaryFileOperations.RunDemo();
                
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