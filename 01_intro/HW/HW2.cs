// FileAnalyzer.cs
// A utility to analyze text files and provide statistics
// Homework: Students need to implement the analysis functionality

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("File Analyzer - .NET Core");
            Console.WriteLine("This tool analyzes text files and provides statistics.");
            
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command-line argument.");
                Console.WriteLine("Example: dotnet run myfile.txt");
                return;
            }
            
            string filePath = args[0];
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' does not exist.");
                return;
            }
            
            try
            {
                Console.WriteLine($"Analyzing file: {filePath}");
                
                // Read the file content
                string content = File.ReadAllText(filePath);
                
                // TODO: Students will implement analysis functionality
                // 1. Count words
                // 2. Count characters (with and without whitespace)
                // 3. Count sentences
                // 4. Identify most common words
                // 5. Average word length
                
                // Example implementation for counting lines:
                int lineCount = File.ReadAllLines(filePath).Length;
                Console.WriteLine($"Number of lines: {lineCount}");
                
                // Additional analysis to be implemented by students
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file analysis: {ex.Message}");
            }
        }
    }
}