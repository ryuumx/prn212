using System;
using System.Threading;

namespace DesignPatternsDemo
{
    // Basic Singleton implementation with eager loading
    public sealed class Singleton
    {
        // Private static instance - initialized when the class is loaded
        private static readonly Singleton _instance = new Singleton();
        
        // Track how many times singleton is constructed
        private static int _instanceCreationCount = 0;
        
        // Private constructor prevents external instantiation
        private Singleton()
        {
            _instanceCreationCount++;
            Console.WriteLine("-- Private constructor is called.");
            Console.WriteLine($"-- Singleton instance #{_instanceCreationCount} is created.");
        }
        
        // Public access point to the singleton instance
        public static Singleton GetInstance
        {
            get
            {
                Console.WriteLine("-- GetInstance property accessed.");
                return _instance;
            }
        }
        
        // Example method on the singleton
        public void Print()
        {
            Console.WriteLine("Hello World from Singleton.");
        }
    }
    
    // Thread-safe Singleton with lazy initialization using double-check locking
    public sealed class ThreadSafeSingleton
    {
        // Private static instance - initialized on first access
        private static ThreadSafeSingleton _instance;
        
        // Object for locking during instance creation
        private static readonly object _lock = new object();
        
        // Track instance creation
        private static int _instanceCount = 0;
        
        // Private constructor prevents external instantiation
        private ThreadSafeSingleton()
        {
            _instanceCount++;
            Console.WriteLine("-- ThreadSafeSingleton: Private constructor called.");
            Console.WriteLine($"-- ThreadSafeSingleton: Instance #{_instanceCount} created.");
            // Simulate some initialization work
            Thread.Sleep(100);
        }
        
        // Public access point with double-check locking pattern
        public static ThreadSafeSingleton Instance
        {
            get
            {
                // First check without locking
                if (_instance == null)
                {
                    // Lock for thread safety
                    lock (_lock)
                    {
                        // Double-check after acquiring the lock
                        if (_instance == null)
                        {
                            Console.WriteLine("-- ThreadSafeSingleton: Creating new instance.");
                            _instance = new ThreadSafeSingleton();
                        }
                        else
                        {
                            Console.WriteLine("-- ThreadSafeSingleton: Instance already created inside lock.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("-- ThreadSafeSingleton: Instance already exists, returning existing instance.");
                }
                
                return _instance;
            }
        }
        
        public void PrintThreadInfo()
        {
            Console.WriteLine($"-- ThreadSafeSingleton accessed by thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
    
    // Real-world example: Database Connection Manager
    public sealed class DbConnectionManager
    {
        // Private static instance - not initialized until needed
        private static DbConnectionManager _instance;
        
        // Lock object for thread safety
        private static readonly object _lock = new object();
        
        // Connection information
        private string _connectionString;
        private bool _isConnected;
        
        // Private constructor prevents external instantiation
        private DbConnectionManager()
        {
            // Default connection settings
            _connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
            _isConnected = false;
            Console.WriteLine("-- DbConnectionManager: Connection manager initialized");
        }
        
        // Public access point with double-check locking pattern
        public static DbConnectionManager Instance
        {
            get
            {
                // First check without locking (for performance)
                if (_instance == null)
                {
                    // Lock for thread safety
                    lock (_lock)
                    {
                        // Double-check after acquiring the lock
                        if (_instance == null)
                        {
                            _instance = new DbConnectionManager();
                        }
                    }
                }
                
                return _instance;
            }
        }
        
        // Database connection methods
        public bool Connect()
        {
            if (!_isConnected)
            {
                Console.WriteLine($"-- DbConnectionManager: Connecting to database using {_connectionString}");
                // Simulate connection
                Thread.Sleep(1000);
                _isConnected = true;
                Console.WriteLine("-- DbConnectionManager: Connection established");
            }
            
            return _isConnected;
        }
        
        public void Disconnect()
        {
            if (_isConnected)
            {
                Console.WriteLine("-- DbConnectionManager: Disconnecting from database");
                // Simulate disconnection
                Thread.Sleep(500);
                _isConnected = false;
                Console.WriteLine("-- DbConnectionManager: Connection closed");
            }
        }
        
        public void ExecuteQuery(string query)
        {
            if (!_isConnected)
            {
                Console.WriteLine("-- DbConnectionManager: Not connected! Connecting first...");
                Connect();
            }
            
            Console.WriteLine($"-- DbConnectionManager: Executing query: {query}");
            // Simulate query execution
            Thread.Sleep(500);
            Console.WriteLine("-- DbConnectionManager: Query executed successfully");
        }
        
        public void SetConnectionString(string connectionString)
        {
            if (_isConnected)
            {
                Disconnect();
            }
            
            _connectionString = connectionString;
            Console.WriteLine($"-- DbConnectionManager: Connection string updated to {_connectionString}");
        }
    }
    
    // Demonstrates potential threading issues
    public class ThreadingDemo
    {
        public static void RunTest()
        {
            Console.WriteLine("\n*** Testing Thread Safety ***");
            
            // Create several threads that all try to access the singleton
            Thread[] threads = new Thread[5];
            
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    // Get singleton instance from each thread
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} trying to get ThreadSafeSingleton instance");
                    ThreadSafeSingleton singleton = ThreadSafeSingleton.Instance;
                    singleton.PrintThreadInfo();
                    
                    // Small delay
                    Thread.Sleep(100);
                });
                
                threads[i].Start();
            }
            
            // Wait for all threads to finish
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            Console.WriteLine("All threads completed.\n");
        }
    }
    
    class SingletonPatternDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Singleton Pattern Demo ***\n");
            
            // Basic Singleton example
            Console.WriteLine("#1. Getting first Singleton instance");
            Singleton firstInstance = Singleton.GetInstance;
            Console.WriteLine("--Invoke Print() method:");
            firstInstance.Print();
            
            Console.WriteLine("\n#2. Getting second Singleton instance");
            Singleton secondInstance = Singleton.GetInstance;
            Console.WriteLine("--Invoke Print() method:");
            secondInstance.Print();
            
            if (ReferenceEquals(firstInstance, secondInstance))
            {
                Console.WriteLine("=> The firstInstance and secondInstance are the same object.");
            }
            else
            {
                Console.WriteLine("=> Different instances exist.");
            }
            
            // Thread-safe Singleton example with lazy loading
            Console.WriteLine("\n*** Thread-safe Singleton Example ***");
            
            // First access will initialize the singleton
            Console.WriteLine("First access to ThreadSafeSingleton:");
            ThreadSafeSingleton.Instance.PrintThreadInfo();
            
            // Second access uses the existing instance
            Console.WriteLine("\nSecond access to ThreadSafeSingleton:");
            ThreadSafeSingleton.Instance.PrintThreadInfo();
            
            // Run threading test to demonstrate thread safety
            ThreadingDemo.RunTest();
            
            // Database Connection Manager example
            Console.WriteLine("\n*** Database Connection Manager Example ***");
            
            // Get the connection manager instance
            DbConnectionManager dbManager = DbConnectionManager.Instance;
            
            // Use the connection manager
            dbManager.Connect();
            dbManager.ExecuteQuery("SELECT * FROM Users");
            dbManager.Disconnect();
            
            // Change connection and run a different query
            dbManager.SetConnectionString("Server=production.example.com;Database=app;User=admin;Password=****;");
            dbManager.ExecuteQuery("INSERT INTO Logs (Message) VALUES ('User logged in')");
            dbManager.Disconnect();
            
            // Another part of the application using the same connection manager
            Console.WriteLine("\nAccessing the connection manager from another component:");
            DbConnectionManager sameDbManager = DbConnectionManager.Instance;
            if (ReferenceEquals(sameDbManager, dbManager))
            {
                Console.WriteLine("-- Using the same database connection manager instance");
            }
            
            sameDbManager.ExecuteQuery("UPDATE Settings SET Value = 'new value' WHERE Key = 'theme'");
            sameDbManager.Disconnect();
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}