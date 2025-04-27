// Logger.cs
// An advanced component that provides cross-platform logging functionality
// Homework: Students need to implement proper logging to both console and file

using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CrossPlatformLogger
{
    // This is a basic interface defining logging functions
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        Task FlushAsync();
    }
    
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }
    
    // TODO: Students need to implement a ConsoleLogger and FileLogger class
    // that implements the ILogger interface
    
    public class ConsoleLogger : ILogger
    {
        // Students will implement this class
        public void Log(LogLevel level, string message)
        {
            // TODO: Implement logging to console with different colors based on level
            throw new NotImplementedException();
        }
        
        public Task FlushAsync()
        {
            // Console logging is immediate, no need to flush
            return Task.CompletedTask;
        }
    }
    
    public class FileLogger : ILogger
    {
        // Students will implement this class
        private readonly string _filePath;
        
        public FileLogger(string filePath)
        {
            _filePath = filePath;
            // TODO: Initialize logging directory and file
        }
        
        public void Log(LogLevel level, string message)
        {
            // TODO: Implement logging to file with timestamps and log levels
            throw new NotImplementedException();
        }
        
        public Task FlushAsync()
        {
            // TODO: Implement any necessary flushing
            throw new NotImplementedException();
        }
    }
    
    // Example of a Logger factory students could implement
    public static class LoggerFactory
    {
        public static ILogger CreateLogger(bool useConsole, string filePath = null)
        {
            // TODO: Students should implement this factory method
            // to return either a ConsoleLogger, FileLogger, or a composite logger
            throw new NotImplementedException();
        }
    }
    
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Cross-Platform Logger Demo");
            
            // Example usage (for students to implement and expand)
            ILogger logger = new ConsoleLogger(); // Default to console logger
            
            // Log some messages
            logger.Log(LogLevel.Info, "Application started");
            logger.Log(LogLevel.Debug, "Debug information");
            logger.Log(LogLevel.Warning, "This is a warning");
            logger.Log(LogLevel.Error, "An error occurred");
            
            await logger.FlushAsync();
            
            Console.WriteLine("\nLogger implementation complete!");
        }
    }
}