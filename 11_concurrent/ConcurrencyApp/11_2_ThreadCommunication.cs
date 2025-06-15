using System;
using System.Threading;

namespace ConcurrencyDemos
{
    public class ThreadCommunicationDemo
    {
        private static bool _shouldStop = false;
        private static readonly object _lockObject = new object();
        private static int _sharedCounter = 0;

        public static void RunDemo()
        {
            Console.WriteLine("=== Thread Communication Demonstrations ===");
            
            DemonstrateThreadInterruption();
            DemonstrateCooperativeCancellation();
            DemonstrateThreadSleep();
            DemonstrateThreadYield();
        }

        private static void DemonstrateThreadInterruption()
        {
            Console.WriteLine("\n=== Thread Interruption Demo ===");
            
            Thread sleepingThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("Worker thread: Starting long sleep...");
                    Thread.Sleep(5000); // Sleep for 5 seconds
                    Console.WriteLine("Worker thread: Sleep completed normally");
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Worker thread: Sleep was interrupted!");
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Worker thread: Thread was aborted!");
                    Thread.ResetAbort(); // In .NET Framework (not needed in .NET Core/5+)
                }
            })
            {
                Name = "SleepingWorker"
            };
            
            sleepingThread.Start();
            
            // Let it sleep for a bit
            Thread.Sleep(1000);
            
            Console.WriteLine("Main thread: Interrupting the sleeping thread...");
            sleepingThread.Interrupt();
            
            sleepingThread.Join();
            Console.WriteLine("Interruption demo completed");
        }

        private static void DemonstrateCooperativeCancellation()
        {
            Console.WriteLine("\n=== Cooperative Cancellation Demo ===");
            
            _shouldStop = false;
            
            Thread cooperativeThread = new Thread(() =>
            {
                int workDone = 0;
                Console.WriteLine("Cooperative worker: Starting work...");
                
                while (!_shouldStop)
                {
                    // Simulate work
                    Thread.Sleep(200);
                    workDone++;
                    Console.WriteLine($"  Work item {workDone} completed");
                    
                    // Check for cancellation periodically
                    if (workDone >= 10) // Failsafe
                        break;
                }
                
                Console.WriteLine($"Cooperative worker: Stopped after {workDone} work items");
            })
            {
                Name = "CooperativeWorker"
            };
            
            cooperativeThread.Start();
            
            // Let it work for a while
            Thread.Sleep(1200);
            
            Console.WriteLine("Main thread: Requesting cooperative cancellation...");
            _shouldStop = true;
            
            cooperativeThread.Join();
            Console.WriteLine("Cooperative cancellation demo completed");
        }

        private static void DemonstrateThreadSleep()
        {
            Console.WriteLine("\n=== Thread.Sleep Demo ===");
            
            Console.WriteLine("Demonstrating different sleep durations:");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Sleep with different durations
            int[] sleepDurations = { 100, 250, 500, 1000 };
            
            foreach (int duration in sleepDurations)
            {
                var start = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Sleeping for {duration}ms...");
                Thread.Sleep(duration);
                var actual = stopwatch.ElapsedMilliseconds - start;
                Console.WriteLine($"  Actual sleep time: {actual}ms");
            }
            
            Console.WriteLine($"Total demo time: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Stop();
        }

        private static void DemonstrateThreadYield()
        {
            Console.WriteLine("\n=== Thread.Yield Demo ===");
            
            int thread1Counter = 0;
            int thread2Counter = 0;
            bool finished = false;
            
            Thread thread1 = new Thread(() =>
            {
                while (!finished)
                {
                    thread1Counter++;
                    if (thread1Counter % 100000 == 0)
                    {
                        Console.WriteLine($"Thread 1: {thread1Counter}");
                        Thread.Yield(); // Give other threads a chance
                    }
                }
            })
            {
                Name = "YieldThread1"
            };
            
            Thread thread2 = new Thread(() =>
            {
                while (!finished)
                {
                    thread2Counter++;
                    if (thread2Counter % 100000 == 0)
                    {
                        Console.WriteLine($"Thread 2: {thread2Counter}");
                        Thread.Yield(); // Give other threads a chance
                    }
                }
            })
            {
                Name = "YieldThread2"
            };
            
            Console.WriteLine("Starting two competing threads with Thread.Yield...");
            thread1.Start();
            thread2.Start();
            
            Thread.Sleep(2000); // Let them run for 2 seconds
            
            finished = true;
            
            thread1.Join();
            thread2.Join();
            
            Console.WriteLine($"Final counts - Thread 1: {thread1Counter}, Thread 2: {thread2Counter}");
            Console.WriteLine("Thread.Yield demo completed");
        }
    }
}