using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CrossPlatformExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cross-Platform File Operations Example");
            Console.WriteLine($"Operating System: {GetOperatingSystem()}");

            // Create platform-specific file path
            string filePath = CreatePlatformPath("dotnet_example.txt");
            Console.WriteLine($"Using path: {filePath}");

            try
            {
                // Write to file
                File.WriteAllText(filePath, "This is a cross-platform .NET Core application!");
                Console.WriteLine("File written successfully");

                // Read from file
                string content = File.ReadAllText(filePath);
                Console.WriteLine("File content:");
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static string GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "Linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "macOS";
            else
                return "Unknown";
        }

        static string CreatePlatformPath(string fileName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileName);
        }
    }
}