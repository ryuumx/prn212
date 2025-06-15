using System;
using System.IO;

namespace FileIODemos
{
    public class FileClassDemo
    {
        public static void RunDemo()
        {
            string testFile = "test_file.txt";
            string backupFile = "backup_file.txt";
            
            // File existence and creation
            Console.WriteLine("=== File Class Operations ===");
            DemonstrateFileOperations(testFile, backupFile);
            
            // File information
            DemonstrateFileInformation(testFile);
            
            // File manipulation
            DemonstrateFileManipulation(testFile, backupFile);
        }
        
        private static void DemonstrateFileOperations(string fileName, string backupFile)
        {
            Console.WriteLine($"File '{fileName}' exists: {File.Exists(fileName)}");
            
            // Create and write
            string[] lines = {
                "First line of content",
                "Second line with timestamp: " + DateTime.Now,
                "Third line with random number: " + new Random().Next(1000)
            };
            
            File.WriteAllLines(fileName, lines);
            Console.WriteLine($"Created file '{fileName}' with {lines.Length} lines");
            
            // Append content
            File.AppendAllText(fileName, "\nAppended line at: " + DateTime.Now);
            Console.WriteLine("Appended additional content");
        }
        
        private static void DemonstrateFileInformation(string fileName)
        {
            if (File.Exists(fileName))
            {
                Console.WriteLine($"\n=== File Information for '{fileName}' ===");
                Console.WriteLine($"Creation time: {File.GetCreationTime(fileName)}");
                Console.WriteLine($"Last write time: {File.GetLastWriteTime(fileName)}");
                Console.WriteLine($"Last access time: {File.GetLastAccessTime(fileName)}");
                
                // Read and display content
                string content = File.ReadAllText(fileName);
                Console.WriteLine($"File size: {content.Length} characters");
                Console.WriteLine("Content:");
                Console.WriteLine(content);
            }
        }
        
        private static void DemonstrateFileManipulation(string sourceFile, string backupFile)
        {
            Console.WriteLine($"\n=== File Manipulation ===");
            
            // Copy file
            File.Copy(sourceFile, backupFile, true);
            Console.WriteLine($"Copied '{sourceFile}' to '{backupFile}'");
            
            // Move/rename (commented out to preserve original)
            // File.Move(backupFile, "renamed_file.txt");
            
            // Compare files
            string originalContent = File.ReadAllText(sourceFile);
            string backupContent = File.ReadAllText(backupFile);
            Console.WriteLine($"Files are identical: {originalContent == backupContent}");
        }
    }
}