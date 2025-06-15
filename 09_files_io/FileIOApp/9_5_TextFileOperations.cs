using System;
using System.IO;
using System.Collections.Generic;

namespace FileIODemos
{
    public class TextFileOperations
    {
        public static void RunDemo()
        {
            string textFile = "sample_text.txt";
            string logFile = "application.log";
            
            Console.WriteLine("=== Text File Operations ===");
            DemonstrateStreamWriter(textFile);
            DemonstrateStreamReader(textFile);
            DemonstrateAdvancedTextOperations(logFile);
        }
        
        private static void DemonstrateStreamWriter(string fileName)
        {
            Console.WriteLine($"Writing to {fileName} using StreamWriter");
            
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("=== Sample Text File ===");
                writer.WriteLine($"Created: {DateTime.Now}");
                writer.WriteLine();
                
                // Write different types of content
                for (int i = 1; i <= 5; i++)
                {
                    writer.WriteLine($"Line {i}: Sample content with number {i * 10}");
                }
                
                writer.WriteLine();
                writer.WriteLine("Special characters: àáâãäåæçèéêë");
                writer.WriteLine("Numbers: 12345.67890");
                writer.WriteLine("Symbols: !@#$%^&*()_+-={}[]|\\:;\"'<>?,./");
            }
            
            Console.WriteLine("File written successfully");
        }
        
        private static void DemonstrateStreamReader(string fileName)
        {
            Console.WriteLine($"\nReading from {fileName} using StreamReader");
            
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                int lineNumber = 1;
                
                Console.WriteLine("Content:");
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"{lineNumber:D2}: {line}");
                    lineNumber++;
                }
            }
            
            // Alternative: Read entire file
            Console.WriteLine("\n--- Reading entire file at once ---");
            using (StreamReader reader = new StreamReader(fileName))
            {
                string entireContent = reader.ReadToEnd();
                Console.WriteLine($"Total characters: {entireContent.Length}");
            }
        }
        
        private static void DemonstrateAdvancedTextOperations(string logFile)
        {
            Console.WriteLine($"\n=== Advanced Text Operations with {logFile} ===");
            
            // Simulate writing log entries
            List<string> logEntries = new List<string>
            {
                "Application started",
                "User login: john.doe",
                "Processing request #1001",
                "Warning: High memory usage detected",
                "Processing request #1002",
                "Error: Database connection timeout",
                "User logout: john.doe",
                "Application shutdown"
            };
            
            // Write log with timestamps
            using (StreamWriter writer = new StreamWriter(logFile))
            {
                foreach (string entry in logEntries)
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {entry}");
                    System.Threading.Thread.Sleep(100); // Simulate time passing
                }
            }
            
            // Read and filter log entries
            Console.WriteLine("Log entries containing 'Error' or 'Warning':");
            using (StreamReader reader = new StreamReader(logFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Error") || line.Contains("Warning"))
                    {
                        Console.WriteLine($"  ALERT: {line}");
                    }
                }
            }
        }
    }
}