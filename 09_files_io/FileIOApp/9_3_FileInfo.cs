using System;
using System.IO;

namespace FileIODemos
{
    public class FileInfoDemo
    {
        public static void RunDemo()
        {
            string fileName = "fileinfo_test.txt";
            
            Console.WriteLine("=== FileInfo Class Operations ===");
            DemonstrateFileInfoCreation(fileName);
            DemonstrateFileInfoProperties(fileName);
            DemonstrateFileInfoMethods(fileName);
        }
        
        private static void DemonstrateFileInfoCreation(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            
            Console.WriteLine($"FileInfo created for: {fileInfo.Name}");
            Console.WriteLine($"Full path: {fileInfo.FullName}");
            Console.WriteLine($"Directory: {fileInfo.DirectoryName}");
            Console.WriteLine($"Extension: {fileInfo.Extension}");
            Console.WriteLine($"Exists: {fileInfo.Exists}");
            
            if (!fileInfo.Exists)
            {
                // Create file using FileInfo
                using (StreamWriter writer = fileInfo.CreateText())
                {
                    writer.WriteLine("Created using FileInfo.CreateText()");
                    writer.WriteLine($"Timestamp: {DateTime.Now}");
                    writer.WriteLine("This demonstrates FileInfo file creation");
                }
                Console.WriteLine("File created using FileInfo");
                
                // Refresh to update properties
                fileInfo.Refresh();
            }
        }
        
        private static void DemonstrateFileInfoProperties(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            
            Console.WriteLine($"\n=== FileInfo Properties ===");
            Console.WriteLine($"File size: {fileInfo.Length} bytes");
            Console.WriteLine($"Creation time: {fileInfo.CreationTime}");
            Console.WriteLine($"Last write time: {fileInfo.LastWriteTime}");
            Console.WriteLine($"Last access time: {fileInfo.LastAccessTime}");
            Console.WriteLine($"Attributes: {fileInfo.Attributes}");
            Console.WriteLine($"Is read-only: {fileInfo.IsReadOnly}");
        }
        
        private static void DemonstrateFileInfoMethods(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            string copyName = "copy_" + fileName;
            
            Console.WriteLine($"\n=== FileInfo Methods ===");
            
            // Copy file
            FileInfo copiedFile = fileInfo.CopyTo(copyName, true);
            Console.WriteLine($"Copied to: {copiedFile.Name}");
            
            // Append to original
            using (StreamWriter writer = fileInfo.AppendText())
            {
                writer.WriteLine($"Appended at: {DateTime.Now}");
            }
            
            // Read content
            string content;
            using (StreamReader reader = fileInfo.OpenText())
            {
                content = reader.ReadToEnd();
            }
            
            Console.WriteLine("File content after append:");
            Console.WriteLine(content);
            
            // Clean up copy
            copiedFile.Delete();
            Console.WriteLine($"Deleted copy: {copyName}");
        }
    }
}