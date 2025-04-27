using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PerformanceDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(".NET Core Performance Demo");
            
            // Sequential processing
            Console.WriteLine("\nSequential Processing:");
            var stopwatch = Stopwatch.StartNew();
            ProcessSequentially(1_000_000);
            stopwatch.Stop();
            Console.WriteLine($"Sequential processing completed in {stopwatch.ElapsedMilliseconds}ms");
            
            // Parallel processing
            Console.WriteLine("\nParallel Processing:");
            stopwatch = Stopwatch.StartNew();
            await ProcessInParallelAsync(1_000_000);
            stopwatch.Stop();
            Console.WriteLine($"Parallel processing completed in {stopwatch.ElapsedMilliseconds}ms");
            
            // LINQ performance
            Console.WriteLine("\nLINQ Performance:");
            var numbers = Enumerable.Range(1, 1_000_000).ToList();
            
            stopwatch = Stopwatch.StartNew();
            var sum1 = SumManually(numbers);
            stopwatch.Stop();
            Console.WriteLine($"Manual sum: {sum1} in {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch = Stopwatch.StartNew();
            var sum2 = numbers.Sum();
            stopwatch.Stop();
            Console.WriteLine($"LINQ sum: {sum2} in {stopwatch.ElapsedMilliseconds}ms");
        }
        
        static void ProcessSequentially(int count)
        {
            long total = 0;
            for (int i = 0; i < count; i++)
            {
                total += PerformCalculation(i);
            }
            Console.WriteLine($"Total: {total}");
        }
        
        static async Task ProcessInParallelAsync(int count)
        {
            var tasks = new List<Task<long>>();
            for (int i = 0; i < count; i += 10000)  // Process in chunks
            {
                int localI = i;
                tasks.Add(Task.Run(() => {
                    long sum = 0;
                    for (int j = localI; j < Math.Min(localI + 10000, count); j++)
                    {
                        sum += PerformCalculation(j);
                    }
                    return sum;
                }));
            }
            
            var results = await Task.WhenAll(tasks);
            long total = results.Sum();
            Console.WriteLine($"Total: {total}");
        }
        
        static long PerformCalculation(int value)
        {
            // Simple calculation to simulate work
            return value % 10;
        }
        
        static long SumManually(List<int> numbers)
        {
            long sum = 0;
            foreach (var number in numbers)
            {
                sum += number;
            }
            return sum;
        }
    }
}