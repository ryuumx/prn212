using System;
using System.Threading;

namespace ConcurrencyDemos
{
    public class ThreadPoolDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== ThreadPool Demonstrations ===");
            
            DemonstrateThreadPoolInfo();
            DemonstrateQueueUserWorkItem();
            DemonstrateWaitCallback();
            DemonstrateThreadPoolVsManualThreads();
        }

        private static void DemonstrateThreadPoolInfo()
        {
            Console.WriteLine("\n=== ThreadPool Information ===");
            
            int workerThreads, completionPortThreads;
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Max threads - Worker: {workerThreads}, Completion Port: {completionPortThreads}");
            
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Min threads - Worker: {workerThreads}, Completion Port: {completionPortThreads}");
            
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"Available threads - Worker: {workerThreads}, Completion Port: {completionPortThreads}");
            
            Console.WriteLine($"Is background thread: {Thread.CurrentThread.IsBackground}");
            Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
        }

        private static void DemonstrateQueueUserWorkItem()
        {
            Console.WriteLine("\n=== QueueUserWorkItem Demo ===");
            
            // Queue work items without parameters
            Console.WriteLine("Queueing 5 work items without parameters...");
            for (int i = 1; i <= 5; i++)
            {
                int workItemId = i; // Capture for closure
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Console.WriteLine($"Work item {workItemId} starting on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000); // Simulate work
                    Console.WriteLine($"Work item {workItemId} completed");
                });
            }
            
            // Wait a bit for work items to start
            Thread.Sleep(500);
            
            // Queue work items with parameters
            Console.WriteLine("\nQueueing work items with parameters...");
            for (int i = 1; i <= 3; i++)
            {
                var workData = new WorkItemData { Id = i + 10, Duration = 800, Description = $"Parameterized task {i}" };
                ThreadPool.QueueUserWorkItem(ParameterizedWorkMethod, workData);
            }
            
            // Wait for all work to complete
            Thread.Sleep(3000);
            Console.WriteLine("All queued work items should be completed");
        }

        private static void ParameterizedWorkMethod(object state)
        {
            if (state is WorkItemData data)
            {
                Console.WriteLine($"Parameterized work {data.Id} ({data.Description}) starting on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(data.Duration);
                Console.WriteLine($"Parameterized work {data.Id} completed");
            }
        }

        private static void DemonstrateWaitCallback()
        {
            Console.WriteLine("\n=== WaitCallback Delegate Demo ===");
            
            // Using WaitCallback delegate explicitly
            WaitCallback callback = (state) =>
            {
                var taskInfo = (string)state;
                Console.WriteLine($"WaitCallback task '{taskInfo}' executing on thread {Thread.CurrentThread.ManagedThreadId}");
                
                // Simulate varying work durations
                var random = new Random();
                int workDuration = random.Next(500, 1500);
                Thread.Sleep(workDuration);
                
                Console.WriteLine($"WaitCallback task '{taskInfo}' completed after {workDuration}ms");
            };
            
            // Queue several tasks
            string[] taskNames = { "DataProcessing", "FileOperation", "NetworkCall", "Calculation" };
            
            Console.WriteLine("Queueing WaitCallback tasks...");
            foreach (string taskName in taskNames)
            {
                ThreadPool.QueueUserWorkItem(callback, taskName);
            }
            
            // Monitor thread pool
            Console.WriteLine("\nMonitoring thread pool usage:");
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(300);
                int workerThreads, completionPortThreads;
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                Console.WriteLine($"  Available threads: {workerThreads} worker, {completionPortThreads} completion port");
            }
            
            Thread.Sleep(2000); // Wait for tasks to complete
        }

        private static void DemonstrateThreadPoolVsManualThreads()
        {
            Console.WriteLine("\n=== ThreadPool vs Manual Threads Performance ===");
            
            const int taskCount = 20;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Test ThreadPool performance
            Console.WriteLine($"Testing ThreadPool with {taskCount} tasks...");
            var threadPoolStart = stopwatch.ElapsedMilliseconds;
            var countdown = new CountdownEvent(taskCount);
            
            for (int i = 0; i < taskCount; i++)
            {
                int taskId = i + 1;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Thread.Sleep(100); // Simulate work
                    Console.WriteLine($"ThreadPool task {taskId} completed on thread {Thread.CurrentThread.ManagedThreadId}");
                    countdown.Signal();
                });
            }
            
            countdown.Wait(); // Wait for all ThreadPool tasks
            var threadPoolTime = stopwatch.ElapsedMilliseconds - threadPoolStart;
            Console.WriteLine($"ThreadPool tasks completed in {threadPoolTime}ms");
            
            // Test manual threads performance
            Console.WriteLine($"\nTesting Manual Threads with {taskCount} tasks...");
            var manualThreadStart = stopwatch.ElapsedMilliseconds;
            var manualThreads = new Thread[taskCount];
            
            for (int i = 0; i < taskCount; i++)
            {
                int taskId = i + 1;
                manualThreads[i] = new Thread(() =>
                {
                    Thread.Sleep(100); // Simulate work
                    Console.WriteLine($"Manual thread task {taskId} completed on thread {Thread.CurrentThread.ManagedThreadId}");
                });
                manualThreads[i].Start();
            }
            
            // Wait for all manual threads
            foreach (Thread thread in manualThreads)
            {
                thread.Join();
            }
            
            var manualThreadTime = stopwatch.ElapsedMilliseconds - manualThreadStart;
            stopwatch.Stop();
            
            Console.WriteLine($"Manual threads completed in {manualThreadTime}ms");
            Console.WriteLine($"Performance difference: ThreadPool was {(double)manualThreadTime / threadPoolTime:F1}x faster");
            Console.WriteLine("\nThreadPool reuses threads and avoids creation overhead!");
        }
    }

    public class WorkItemData
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
    }
}