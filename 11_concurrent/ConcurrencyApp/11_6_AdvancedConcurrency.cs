using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrencyDemos
{
    public class AdvancedConcurrencyDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Advanced Concurrency Patterns ===");
            
            DemonstrateProducerConsumer();
            DemonstrateThreadSafeCollections();
            DemonstrateReaderWriterLock();
            DemonstrateCancellationToken();
        }

        private static void DemonstrateProducerConsumer()
        {
            Console.WriteLine("\n=== Producer-Consumer Pattern ===");
            
            var queue = new ConcurrentQueue<WorkItem>();
            var cts = new CancellationTokenSource();
            
            // Producer thread
            Thread producer = new Thread(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    var workItem = new WorkItem { Id = i, Data = $"Task {i}", CreatedAt = DateTime.Now };
                    queue.Enqueue(workItem);
                    Console.WriteLine($"Producer: Added {workItem.Data}");
                    Thread.Sleep(300);
                }
                Console.WriteLine("Producer: Finished adding items");
            })
            { Name = "Producer" };
            
            // Consumer threads
            Thread[] consumers = new Thread[2];
            for (int i = 0; i < consumers.Length; i++)
            {
                int consumerId = i + 1;
                consumers[i] = new Thread(() =>
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        if (queue.TryDequeue(out WorkItem workItem))
                        {
                            Console.WriteLine($"Consumer {consumerId}: Processing {workItem.Data}");
                            Thread.Sleep(500); // Simulate work
                            Console.WriteLine($"Consumer {consumerId}: Completed {workItem.Data}");
                        }
                        else
                        {
                            Thread.Sleep(100); // No work available, wait a bit
                        }
                    }
                    Console.WriteLine($"Consumer {consumerId}: Shutting down");
                })
                { Name = $"Consumer{consumerId}" };
            }
            
            // Start all threads
            producer.Start();
            foreach (Thread consumer in consumers)
            {
                consumer.Start();
            }
            
            // Wait for producer to finish
            producer.Join();
            
            // Wait a bit for consumers to process remaining items
            Thread.Sleep(2000);
            
            // Signal consumers to stop
            cts.Cancel();
            
            // Wait for consumers to finish
            foreach (Thread consumer in consumers)
            {
                consumer.Join();
            }
            
            Console.WriteLine($"Remaining items in queue: {queue.Count}");
        }

        private static void DemonstrateThreadSafeCollections()
        {
            Console.WriteLine("\n=== Thread-Safe Collections Demo ===");
            
            // ConcurrentDictionary
            var concurrentDict = new ConcurrentDictionary<string, int>();
            
            // Multiple threads updating the dictionary
            Thread[] dictThreads = new Thread[3];
            for (int i = 0; i < dictThreads.Length; i++)
            {
                int threadId = i + 1;
                dictThreads[i] = new Thread(() =>
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        string key = $"Thread{threadId}_Item{j}";
                        concurrentDict.AddOrUpdate(key, 1, (k, v) => v + 1);
                        Console.WriteLine($"Thread {threadId}: Added/Updated {key}");
                        Thread.Sleep(100);
                    }
                });
            }
            
            foreach (Thread thread in dictThreads)
            {
                thread.Start();
            }
            
            foreach (Thread thread in dictThreads)
            {
                thread.Join();
            }
            
            Console.WriteLine($"ConcurrentDictionary final count: {concurrentDict.Count}");
            foreach (var kvp in concurrentDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            
            // ConcurrentBag
            var concurrentBag = new ConcurrentBag<string>();
            
            Thread[] bagThreads = new Thread[2];
            for (int i = 0; i < bagThreads.Length; i++)
            {
                int threadId = i + 1;
                bagThreads[i] = new Thread(() =>
                {
                    // Add items
                    for (int j = 1; j <= 3; j++)
                    {
                        string item = $"BagItem_T{threadId}_{j}";
                        concurrentBag.Add(item);
                        Console.WriteLine($"Thread {threadId}: Added {item} to bag");
                    }
                    
                    // Try to take items
                    for (int j = 1; j <= 2; j++)
                    {
                        if (concurrentBag.TryTake(out string item))
                        {
                            Console.WriteLine($"Thread {threadId}: Took {item} from bag");
                        }
                    }
                });
            }
            
            foreach (Thread thread in bagThreads)
            {
                thread.Start();
            }
            
            foreach (Thread thread in bagThreads)
            {
                thread.Join();
            }
            
            Console.WriteLine($"ConcurrentBag final count: {concurrentBag.Count}");
        }

        private static void DemonstrateReaderWriterLock()
        {
            Console.WriteLine("\n=== ReaderWriterLock Demo ===");
            
            var sharedResource = new SharedResource();
            
            // Create reader threads
            Thread[] readers = new Thread[3];
            for (int i = 0; i < readers.Length; i++)
            {
                int readerId = i + 1;
                readers[i] = new Thread(() =>
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        string value = sharedResource.Read();
                        Console.WriteLine($"Reader {readerId}: Read '{value}'");
                        Thread.Sleep(200);
                    }
                })
                { Name = $"Reader{readerId}" };
            }
            
            // Create writer threads
            Thread[] writers = new Thread[2];
            for (int i = 0; i < writers.Length; i++)
            {
                int writerId = i + 1;
                writers[i] = new Thread(() =>
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        string newValue = $"Writer{writerId}_Update{j}";
                        sharedResource.Write(newValue);
                        Console.WriteLine($"Writer {writerId}: Wrote '{newValue}'");
                        Thread.Sleep(400);
                    }
                })
                { Name = $"Writer{writerId}" };
            }
            
            // Start all threads
            foreach (Thread reader in readers)
            {
                reader.Start();
            }
            
            foreach (Thread writer in writers)
            {
                writer.Start();
            }
            
            // Wait for all threads to complete
            foreach (Thread reader in readers)
            {
                reader.Join();
            }
            
            foreach (Thread writer in writers)
            {
                writer.Join();
            }
            
            Console.WriteLine($"Final value: '{sharedResource.Read()}'");
        }

        private static void DemonstrateCancellationToken()
        {
            Console.WriteLine("\n=== CancellationToken Demo ===");
            
            var cts = new CancellationTokenSource();
            
            // Long-running task that respects cancellation
            Thread longRunningTask = new Thread(() =>
            {
                try
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        // Check for cancellation
                        cts.Token.ThrowIfCancellationRequested();
                        
                        Console.WriteLine($"Long task: Step {i}/20");
                        Thread.Sleep(300);
                    }
                    Console.WriteLine("Long task: Completed normally");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Long task: Cancelled gracefully");
                }
            })
            { Name = "LongRunningTask" };
            
            longRunningTask.Start();
            
            // Cancel after 2 seconds
            Thread.Sleep(2000);
            Console.WriteLine("Main thread: Requesting cancellation...");
            cts.Cancel();
            
            longRunningTask.Join();
            Console.WriteLine("CancellationToken demo completed");
        }
    }

    public class WorkItem
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SharedResource
    {
        private string _data = "Initial Value";
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public string Read()
        {
            _lock.EnterReadLock();
            try
            {
                Thread.Sleep(100); // Simulate read operation
                return _data;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Write(string newValue)
        {
            _lock.EnterWriteLock();
            try
            {
                Thread.Sleep(200); // Simulate write operation
                _data = newValue;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}