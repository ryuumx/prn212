using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatterns.Homework
{
    // This homework is based on the Singleton Pattern with a practical application
    // You will implement a basic logging system using the Singleton pattern
    
    public class Logger
    {
        // Private static instance of the Logger class
        private static Logger _instance;
        
        // Private lock object for thread safety
        private static readonly object _lock = new object();
        
        // Counter to track instance creation attempts
        private static int _instanceCount = 0;
        
        // Collection to hold log messages
        private List<string> _logMessages;
        
        // Private constructor to prevent instantiation from outside
        private Logger()
        {
            _logMessages = new List<string>();
            _instanceCount++;
            Console.WriteLine($"Logger instance created. Instance count: {_instanceCount}");
        }
        
        // Public static method to get the single instance
        public static Logger GetInstance
        {
            get
            {
                // TODO: Implement the GetInstance property using double-check locking pattern
                // 1. First check if _instance is null without locking (for performance)
                // 2. If it's null, acquire the lock using the _lock object
                // 3. Double-check if _instance is still null after acquiring the lock
                // 4. If still null, create a new instance
                // 5. Return the instance
                
                return null; // Replace this with your implementation
            }
        }
        
        // Alternative implementation - Option #1 (eager initialization)
        // Students can uncomment and implement this approach as an alternative
        // Comment: This approach creates the instance when the class is loaded
        /*
        // Private static instance initialized immediately (eager)
        private static readonly Logger _eagerInstance = new Logger();
        
        // Simple property returning the already-created instance
        public static Logger GetEagerInstance
        {
            get
            {
                return _eagerInstance;
            }
        }
        */
        
        // Alternative implementation - Option #2 (simple thread-safe using lock only)
        // Students can uncomment and implement this approach as an alternative
        // Comment: This approach is thread-safe but less efficient
        /*
        public static Logger GetSimpleThreadSafeInstance
        {
            get
            {
                // TODO: Implement simple thread-safe singleton using only lock
                // 1. Acquire the lock
                // 2. Check if _instance is null
                // 3. Create new instance if null
                // 4. Return the instance
                
                return null; // Replace with implementation
            }
        }
        */
        
        // Public property to access instance count (for demonstration)
        public static int InstanceCount
        {
            get { return _instanceCount; }
        }
        
        // Method to log an information message
        public void LogInfo(string message)
        {
            // TODO: Implement LogInfo method
            // 1. Format the log entry with current timestamp, log level (INFO), and the message
            //    Format example: "[2023-05-20 14:30:45] [INFO] User logged in"
            // 2. Add the formatted entry to _logMessages list
            // 3. Print the log entry to the console
        }
        
        // Method to log an error message
        public void LogError(string message)
        {
            // TODO: Implement LogError method
            // Similar to LogInfo but with ERROR level
            // Format example: "[2023-05-20 14:30:45] [ERROR] Database connection failed"
        }
        
        // Method to log a warning message
        public void LogWarning(string message)
        {
            // TODO: Implement LogWarning method
            // Similar to LogInfo but with WARNING level
            // Format example: "[2023-05-20 14:30:45] [WARNING] Disk space low"
        }
        
        // Method to display all log entries
        public void DisplayLogs()
        {
            Console.WriteLine("\n----- LOG ENTRIES -----");
            
            // TODO: Implement DisplayLogs method
            // 1. Check if there are any logs (_logMessages.Count > 0)
            // 2. If no logs, print a message saying "No log entries found."
            // 3. Otherwise, iterate through _logMessages and print each entry
            
            Console.WriteLine("----- END OF LOGS -----\n");
        }
        
        // Method to clear all logs
        public void ClearLogs()
        {
            // TODO: Implement ClearLogs method
            // 1. Clear the _logMessages list (_logMessages.Clear())
            // 2. Print a message indicating that logs have been cleared
        }
    }
    
    // Example application classes using the logger
    
    public class UserService
    {
        private Logger _logger;
        
        public UserService()
        {
            // Get logger instance
            _logger = Logger.GetInstance;
        }
        
        public void RegisterUser(string username)
        {
            try
            {
                // Simulate user registration
                if (string.IsNullOrEmpty(username))
                {
                    throw new ArgumentException("Username cannot be empty");
                }
                
                _logger.LogInfo($"User '{username}' registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register user: {ex.Message}");
            }
        }
    }
    
    public class PaymentService
    {
        private Logger _logger;
        
        public PaymentService()
        {
            // Get logger instance
            _logger = Logger.GetInstance;
        }
        
        public void ProcessPayment(string userId, decimal amount)
        {
            try
            {
                // Simulate payment processing
                if (amount <= 0)
                {
                    throw new ArgumentException("Payment amount must be positive");
                }
                
                _logger.LogInfo($"Payment of ${amount} processed for user '{userId}'");
                
                // Simulate a business rule check
                if (amount > 1000)
                {
                    _logger.LogWarning($"Large payment of ${amount} detected for user '{userId}'. Verification required.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Payment processing failed: {ex.Message}");
            }
        }
    }
    
    // Demonstrate threading issues with singletons
    public class ThreadingDemo
    {
        public static void RunThreadingTest()
        {
            Console.WriteLine("\n----- THREADING TEST -----");
            Console.WriteLine("Creating logger instances from multiple threads...");
            
            // Create and start 5 threads
            Thread[] threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Getting logger instance");
                    Logger logger = Logger.GetInstance;
                    logger.LogInfo($"Log from thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(100); // Small delay
                });
                
                threads[i].Start();
            }
            
            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            Console.WriteLine($"Threading test complete. Instance count: {Logger.InstanceCount}");
            Console.WriteLine("----- END THREADING TEST -----\n");
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Singleton Pattern Homework - Logger System\n");
            
            // Test that we're working with the same instance
            Console.WriteLine("Creating first logger instance...");
            Logger logger1 = Logger.GetInstance;
            
            Console.WriteLine("\nCreating second logger instance...");
            Logger logger2 = Logger.GetInstance;
            
            Console.WriteLine($"\nAre both loggers the same instance? {ReferenceEquals(logger1, logger2)}");
            Console.WriteLine($"Total instances created: {Logger.InstanceCount} (should be 1)\n");
            
            // Test threading to demonstrate potential issues without proper thread safety
            ThreadingDemo.RunThreadingTest();
            
            // Use the services which use the logger
            UserService userService = new UserService();
            PaymentService paymentService = new PaymentService();
            
            // Test with valid data
            userService.RegisterUser("john_doe");
            paymentService.ProcessPayment("john_doe", 99.99m);
            
            // Test with invalid data
            userService.RegisterUser("");
            paymentService.ProcessPayment("jane_doe", -50);
            
            // Test with data that triggers a warning
            paymentService.ProcessPayment("big_spender", 5000m);
            
            // Display all logs
            Logger.GetInstance.DisplayLogs();
            
            // Clear logs
            Logger.GetInstance.ClearLogs();
            
            // Add a few more logs after clearing
            Logger.GetInstance.LogInfo("Application shutting down");
            
            // Display logs again
            Logger.GetInstance.DisplayLogs();
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
