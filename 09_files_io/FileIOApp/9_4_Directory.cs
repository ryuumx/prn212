using System;
using System.IO;
using System.Linq;

namespace FileIODemos
{
    public class DirectoryDemo
    {
        public static void RunDemo()
        {
            string testDir = "TestDirectory";
            string subDir = Path.Combine(testDir, "SubDirectory");
            
            Console.WriteLine("=== Directory Class Operations ===");
            DemonstrateDirectoryOperations(testDir, subDir);
            DemonstrateDirectoryListing(testDir);
            DemonstrateDirectoryInfo(testDir);
        }
        
        private static void DemonstrateDirectoryOperations(string testDir, string subDir)
        {
            Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
            
            // Create directories
            if (!Directory.Exists(testDir))
            {
                Directory.CreateDirectory(testDir);
                Console.WriteLine($"Created directory: {testDir}");
            }
            
            if (!Directory.Exists(subDir))
            {
                Directory.CreateDirectory(subDir);
                Console.WriteLine($"Created subdirectory: {subDir}");
            }
            
            // Create some test files
            CreateTestFiles(testDir, subDir);
        }
        
        private static void CreateTestFiles(string testDir, string subDir)
        {
            string[] filesToCreate = {
                Path.Combine(testDir, "file1.txt"),
                Path.Combine(testDir, "file2.log"),
                Path.Combine(testDir, "document.pdf"),
                Path.Combine(subDir, "subfile1.txt"),
                Path.Combine(subDir, "subfile2.xml")
            };
            
            foreach (string file in filesToCreate)
            {
                File.WriteAllText(file, $"Test content for {Path.GetFileName(file)}\nCreated: {DateTime.Now}");
            }
            
            Console.WriteLine($"Created {filesToCreate.Length} test files");
        }
        
        private static void DemonstrateDirectoryListing(string testDir)
        {
            Console.WriteLine($"\n=== Directory Listing for '{testDir}' ===");
            
            // Get all files
            string[] files = Directory.GetFiles(testDir);
            Console.WriteLine($"Files in root directory ({files.Length}):");
            foreach (string file in files)
            {
                Console.WriteLine($"  - {Path.GetFileName(file)}");
            }
            
            // Get all directories
            string[] directories = Directory.GetDirectories(testDir);
            Console.WriteLine($"\nSubdirectories ({directories.Length}):");
            foreach (string dir in directories)
            {
                Console.WriteLine($"  - {Path.GetFileName(dir)}");
            }
            
            // Get all files recursively
            string[] allFiles = Directory.GetFiles(testDir, "*", SearchOption.AllDirectories);
            Console.WriteLine($"\nAll files (recursive) ({allFiles.Length}):");
            foreach (string file in allFiles)
            {
                Console.WriteLine($"  - {file.Replace(testDir + Path.DirectorySeparatorChar, "")}");
            }
            
            // Filter by extension
            string[] txtFiles = Directory.GetFiles(testDir, "*.txt", SearchOption.AllDirectories);
            Console.WriteLine($"\nText files only ({txtFiles.Length}):");
            foreach (string file in txtFiles)
            {
                Console.WriteLine($"  - {file}");
            }
        }
        
        private static void DemonstrateDirectoryInfo(string testDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(testDir);
            
            Console.WriteLine($"\n=== DirectoryInfo for '{testDir}' ===");
            Console.WriteLine($"Full name: {dirInfo.FullName}");
            Console.WriteLine($"Parent: {dirInfo.Parent?.Name ?? "None"}");
            Console.WriteLine($"Root: {dirInfo.Root}");
            Console.WriteLine($"Creation time: {dirInfo.CreationTime}");
            Console.WriteLine($"Attributes: {dirInfo.Attributes}");
            
            // Use DirectoryInfo methods
            FileInfo[] fileInfos = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            Console.WriteLine($"\nFiles using DirectoryInfo ({fileInfos.Length}):");
            foreach (FileInfo file in fileInfos)
            {
                Console.WriteLine($"  - {file.Name} ({file.Length} bytes)");
            }
            
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            Console.WriteLine($"\nSubdirectories using DirectoryInfo ({subDirs.Length}):");
            foreach (DirectoryInfo subDir in subDirs)
            {
                Console.WriteLine($"  - {subDir.Name}");
                
                // List files in subdirectory
                FileInfo[] subFiles = subDir.GetFiles();
                foreach (FileInfo subFile in subFiles)
                {
                    Console.WriteLine($"    * {subFile.Name}");
                }
            }
        }
    }
}