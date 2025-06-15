using System;
using System.Threading;

namespace ConcurrencyDemos
{
    public class BasicThreadingDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Basic Threading Demonstrations ===");
            
            DemonstrateThreadStart();
            DemonstrateParameterizedThreadStart();
            DemonstrateForegroundVsBackground();
            DemonstrateThreadProperties();
        }

        private static void DemonstrateThreadStart()
        {
            Console.WriteLine("\n=== ThreadStart Delegate Demo ===");
            
            Console.WriteLine($"Main thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Creating and starting a new thread...");
            
            // Create thread using ThreadStart delegate
            Thread workerThread = new Thread(SimpleWorkerMethod);
            workerThread.Name = "SimpleWorker";
            
            // Start the thread
            workerThread.Start();
            
            // Do some work on main thread
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Main thread working: Step {i}");
                Thread.Sleep(500); // Simulate work
            }
            
            // Wait for worker thread to complete
            workerThread.Join();
            Console.WriteLine("Worker thread has completed. Main thread continuing...");
        }

        private static void SimpleWorkerMethod()
        {
            Console.WriteLine($"Worker thread started. ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Worker thread name: {Thread.CurrentThread.Name}");
            
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"  Worker thread: Task {i}");
                Thread.Sleep(800); // Simulate work
            }
            
            Console.WriteLine("Worker thread completed its tasks.");
        }

        private static void DemonstrateParameterizedThreadStart()
        {
            Console.WriteLine("\n=== ParameterizedThreadStart Demo ===");
            
            // Create thread that accepts parameters
            Thread paramThread = new Thread(ParameterizedWorkerMethod);
            paramThread.Name = "ParameterizedWorker";
            
            // Pass data to the thread
            var threadData = new WorkerData
            {
                WorkerId = 101,
                TaskCount = 4,
                DelayMs = 300
            };
            
            paramThread.Start(threadData);
            
            // Main thread continues
            Console.WriteLine("Main thread started parameterized worker and continues...");
            
            // Wait for completion
            paramThread.Join();
            Console.WriteLine("Parameterized worker completed.");
        }

        private static void ParameterizedWorkerMethod(object data)
        {
            if (data is WorkerData workerData)
            {
                Console.WriteLine($"Parameterized worker started:");
                Console.WriteLine($"  Worker ID: {workerData.WorkerId}");
                Console.WriteLine($"  Task Count: {workerData.TaskCount}");
                Console.WriteLine($"  Thread ID: {Thread.CurrentThread.ManagedThreadId}");
                
                for (int i = 1; i <= workerData.TaskCount; i++)
                {
                    Console.WriteLine($"  Worker {workerData.WorkerId}: Processing task {i}");
                    Thread.Sleep(workerData.DelayMs);
                }
                
                Console.WriteLine($"Worker {workerData.WorkerId} finished all tasks.");
            }
            else
            {
                Console.WriteLine("Invalid data passed to parameterized worker.");
            }
        }

        private static void DemonstrateForegroundVsBackground()
        {
            Console.WriteLine("\n=== Foreground vs Background Threads ===");
            
            // Foreground thread (default)
            Thread foregroundThread = new Thread(() =>
            {
                Console.WriteLine("Foreground thread started");
                for (int i = 1; i <= 3; i++)
                {
                    Console.WriteLine($"  Foreground: {i}");
                    Thread.Sleep(400);
                }
                Console.WriteLine("Foreground thread completed");
            })
            {
                Name = "ForegroundWorker",
                IsBackground = false // This is default
            };
            
            // Background thread
            Thread backgroundThread = new Thread(() =>
            {
                Console.WriteLine("Background thread started");
                for (int i = 1; i <= 10; i++) // Intentionally longer
                {
                    Console.WriteLine($"  Background: {i}");
                    Thread.Sleep(300);
                }
                Console.WriteLine("Background thread completed (might not print)");
            })
            {
                Name = "BackgroundWorker",
                IsBackground = true
            };
            
            foregroundThread.Start();
            backgroundThread.Start();
            
            // Wait only for foreground thread
            foregroundThread.Join();
            
            Console.WriteLine("Main thread ending - background thread will be terminated");
            // Note: Background thread will be terminated when main thread exits
        }

        private static void DemonstrateThreadProperties()
        {
            Console.WriteLine("\n=== Thread Properties Demo ===");
            
            Thread demoThread = new Thread(() =>
            {
                Thread current = Thread.CurrentThread;
                Console.WriteLine($"Thread Information:");
                Console.WriteLine($"  Name: {current.Name ?? "Unnamed"}");
                Console.WriteLine($"  ID: {current.ManagedThreadId}");
                Console.WriteLine($"  Is Background: {current.IsBackground}");
                Console.WriteLine($"  Is Alive: {current.IsAlive}");
                Console.WriteLine($"  Priority: {current.Priority}");
                Console.WriteLine($"  Thread State: {current.ThreadState}");
                
                Thread.Sleep(1000);
                Console.WriteLine("Demo thread work completed");
            })
            {
                Name = "PropertiesDemo",
                Priority = ThreadPriority.AboveNormal
            };
            
            Console.WriteLine($"Before Start - Thread State: {demoThread.ThreadState}");
            Console.WriteLine($"Before Start - Is Alive: {demoThread.IsAlive}");
            
            demoThread.Start();
            
            Console.WriteLine($"After Start - Thread State: {demoThread.ThreadState}");
            Console.WriteLine($"After Start - Is Alive: {demoThread.IsAlive}");
            
            demoThread.Join();
            
            Console.WriteLine($"After Join - Thread State: {demoThread.ThreadState}");
            Console.WriteLine($"After Join - Is Alive: {demoThread.IsAlive}");
        }
    }

    // Supporting class for parameterized thread demo
    public class WorkerData
    {
        public int WorkerId { get; set; }
        public int TaskCount { get; set; }
        public int DelayMs { get; set; }
    }
}