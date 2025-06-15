using System;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrencyDemos
{
    public class BestPracticesDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Concurrency Best Practices and Common Pitfalls ===");
            
            DemonstrateDeadlockPrevention();
            DemonstrateProperExceptionHandling();
            DemonstrateResourceManagement();
            DemonstrateBestPracticesSummary();
        }

        private static void DemonstrateDeadlockPrevention()
        {
            Console.WriteLine("\n=== Deadlock Prevention ===");
            
            var lockA = new object();
            var lockB = new object();
            
            Console.WriteLine("Creating two threads that could deadlock...");
            
            Thread thread1 = new Thread(() =>
            {
                Console.WriteLine("Thread 1: Attempting to acquire Lock A");
                lock (lockA)
                {
                    Console.WriteLine("Thread 1: Acquired Lock A");
                    Thread.Sleep(100);
                    
                    Console.WriteLine("Thread 1: Attempting to acquire Lock B");
                    lock (lockB)
                    {
                        Console.WriteLine("Thread 1: Acquired Lock B - Work completed");
                    }
                }
            })
            { Name = "Thread1" };
            
            Thread thread2 = new Thread(() =>
            {
                Console.WriteLine("Thread 2: Attempting to acquire Lock A (same order as Thread 1)");
                lock (lockA) // Same order as Thread 1 to prevent deadlock
                {
                    Console.WriteLine("Thread 2: Acquired Lock A");
                    Thread.Sleep(100);
                    
                    Console.WriteLine("Thread 2: Attempting to acquire Lock B");
                    lock (lockB)
                    {
                        Console.WriteLine("Thread 2: Acquired Lock B - Work completed");
                    }
                }
            })
            { Name = "Thread2" };
            
            thread1.Start();
            thread2.Start();
            
            thread1.Join();
            thread2.Join();
            
            Console.WriteLine("Deadlock prevention: Both threads completed successfully!");
            Console.WriteLine("Key: Always acquire locks in the same order across all threads");
        }

        private static void DemonstrateProperExceptionHandling()
        {
            Console.WriteLine("\n=== Proper Exception Handling in Threads ===");
            
            // Thread with proper exception handling
            Thread wellBehavedThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("Well-behaved thread: Starting work");
                    
                    // Simulate work that might throw an exception
                    var random = new Random();
                    if (random.Next(1, 3) == 1)
                    {
                        throw new InvalidOperationException("Simulated error in thread");
                    }
                    
                    Console.WriteLine("Well-behaved thread: Work completed successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Well-behaved thread: Caught exception - {ex.Message}");
                    // Log the exception, clean up resources, etc.
                }
                finally
                {
                    Console.WriteLine("Well-behaved thread: Cleanup completed");
                }
            })
            { Name = "WellBehavedThread" };
            
            wellBehavedThread.Start();
            wellBehavedThread.Join();
            
            // Demonstrate thread-safe error collection
            var errors = new List<string>();
            var errorLock = new object();
            
            Thread[] workers = new Thread[3];
            for (int i = 0; i < workers.Length; i++)
            {
                int workerId = i + 1;
                workers[i] = new Thread(() =>
                {
                    try
                    {
                        // Simulate work with potential errors
                        if (workerId == 2)
                        {
                            throw new ArgumentException($"Worker {workerId} encountered an error");
                        }
                        
                        Console.WriteLine($"Worker {workerId}: Completed successfully");
                    }
                    catch (Exception ex)
                    {
                        lock (errorLock)
                        {
                            errors.Add($"Worker {workerId}: {ex.Message}");
                        }
                        Console.WriteLine($"Worker {workerId}: Error handled and recorded");
                    }
                });
            }
            
            foreach (Thread worker in workers)
            {
                worker.Start();
            }
            
            foreach (Thread worker in workers)
            {
                worker.Join();
            }
            
            Console.WriteLine($"Collected {errors.Count} errors:");
            foreach (string error in errors)
            {
                Console.WriteLine($"  {error}");
            }
        }

        private static void DemonstrateResourceManagement()
        {
            Console.WriteLine("\n=== Proper Resource Management ===");
            
            // Using using statement with IDisposable
            Console.WriteLine("Demonstrating proper resource management with IDisposable...");
            
            Thread resourceThread = new Thread(() =>
            {
                using (var resource = new ManagedResource("ResourceA"))
                {
                    Console.WriteLine("Resource thread: Working with managed resource");
                    Thread.Sleep(1000);
                    
                    // Resource will be properly disposed even if an exception occurs
                    if (DateTime.Now.Millisecond % 2 == 0)
                    {
                        throw new InvalidOperationException("Simulated exception");
                    }
                }
            });
            
            try
            {
                resourceThread.Start();
                resourceThread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Resource management: {ex.Message}");
            }
            
            Console.WriteLine("Resource management completed");
        }

        private static void DemonstrateBestPracticesSummary()
        {
            Console.WriteLine("\n=== Concurrency Best Practices Summary ===");
            
            Console.WriteLine("✓ Threading Best Practices:");
            Console.WriteLine("  1. Prefer ThreadPool over manual threads for short-lived tasks");
            Console.WriteLine("  2. Use CancellationToken for cooperative cancellation");
            Console.WriteLine("  3. Always handle exceptions in thread methods");
            Console.WriteLine("  4. Use 'using' statements for proper resource disposal");
            Console.WriteLine("  5. Minimize lock scope and duration");
            
            Console.WriteLine("\n✓ Synchronization Best Practices:");
            Console.WriteLine("  1. Acquire locks in consistent order to prevent deadlocks");
            Console.WriteLine("  2. Use Interlocked for simple atomic operations");
            Console.WriteLine("  3. Prefer concurrent collections over manual locking");
            Console.WriteLine("  4. Use ReaderWriterLockSlim for read-heavy scenarios");
            Console.WriteLine("  5. Avoid nested locks when possible");
            
            Console.WriteLine("\n✓ Performance Best Practices:");
            Console.WriteLine("  1. Measure before optimizing - don't assume");
            Console.WriteLine("  2. Consider thread overhead vs. benefit");
            Console.WriteLine("  3. Use appropriate collection types for your scenario");
            Console.WriteLine("  4. Minimize context switching with proper work distribution");
            Console.WriteLine("  5. Profile thread contention and bottlenecks");
            
            Console.WriteLine("\n⚠️ Common Pitfalls to Avoid:");
            Console.WriteLine("  1. Race conditions due to unsynchronized shared state");
            Console.WriteLine("  2. Deadlocks from inconsistent lock ordering");
            Console.WriteLine("  3. Memory leaks from unmanaged thread lifetimes");
            Console.WriteLine("  4. Excessive locking leading to serialized execution");
            Console.WriteLine("  5. Ignoring exception handling in background threads");
            
            // Demonstrate measuring thread performance
            Console.WriteLine("\n=== Performance Measurement Example ===");
            MeasureThreadPerformance();
        }

        private static void MeasureThreadPerformance()
        {
            const int workItems = 1000000;
            
            // Single-threaded baseline
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            int singleThreadResult = 0;
            for (int i = 0; i < workItems; i++)
            {
                singleThreadResult += i % 1000;
            }
            
            long singleThreadTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Single-threaded: {singleThreadTime}ms, Result: {singleThreadResult}");
            
            // Multi-threaded with proper work distribution
            stopwatch.Restart();
            
            int multiThreadResult = 0;
            int numThreads = Environment.ProcessorCount;
            int workPerThread = workItems / numThreads;
            
            Thread[] threads = new Thread[numThreads];
            int[] partialResults = new int[numThreads];
            
            for (int t = 0; t < numThreads; t++)
            {
                int threadIndex = t;
                int startIndex = threadIndex * workPerThread;
                int endIndex = (threadIndex == numThreads - 1) ? workItems : startIndex + workPerThread;
                
                threads[t] = new Thread(() =>
                {
                    int partial = 0;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        partial += i % 1000;
                    }
                    partialResults[threadIndex] = partial;
                });
            }
            
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            foreach (int partial in partialResults)
            {
                multiThreadResult += partial;
            }
            
            long multiThreadTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            
            Console.WriteLine($"Multi-threaded ({numThreads} threads): {multiThreadTime}ms, Result: {multiThreadResult}");
            Console.WriteLine($"Speedup: {(double)singleThreadTime / multiThreadTime:F1}x");
            Console.WriteLine($"Results match: {singleThreadResult == multiThreadResult}");
        }
    }

    public class ManagedResource : IDisposable
    {
        private string _name;
        private bool _disposed = false;

        public ManagedResource(string name)
        {
            _name = name;
            Console.WriteLine($"ManagedResource '{_name}': Created");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine($"ManagedResource '{_name}': Disposed");
                _disposed = true;
            }
        }
    }
}