using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BoxingUnboxingPerformance
{
    /// <summary>
    /// Practical demonstration of boxing/unboxing performance and memory impact
    /// </summary>
    class Program
    {
        // Size of collections for testing
        private const int CollectionSize = 10_000_000;
        private const int MemoryTestSize = 1_000_000; // Smaller size for memory tests
        
        static void Main(string[] args)
        {
            Console.WriteLine("===== Boxing/Unboxing Performance Demo =====");
            
            // Memory impact demonstration
            DemonstrateMemoryImpact();
            
            // Performance comparison
            DemonstratePerformanceImpact();
            
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
        
        static void DemonstrateMemoryImpact()
        {
            Console.WriteLine("\n1. MEMORY IMPACT DEMONSTRATION");
            
            // Clear memory before starting
            ClearMemory();
            
            Console.WriteLine("Creating arrays to store integers...");
            
            // Measure memory usage before creating any collections
            long memoryBefore = GetMemoryUsage();
            Console.WriteLine($"Memory before: {memoryBefore:N0} KB");
            
            // Create an array of regular integers
            Console.WriteLine("\nCreating array of regular int values...");
            int[] regularArray = new int[MemoryTestSize];
            for (int i = 0; i < MemoryTestSize; i++)
            {
                regularArray[i] = i;
            }
            
            // Measure memory after creating regular array
            long memoryAfterRegular = GetMemoryUsage();
            Console.WriteLine($"Memory after int[]: {memoryAfterRegular:N0} KB");
            Console.WriteLine($"Memory used by int[]: {memoryAfterRegular - memoryBefore:N0} KB");
            
            // Create an array of boxed integers
            Console.WriteLine("\nCreating array of boxed int values (as objects)...");
            object[] boxedArray = new object[MemoryTestSize];
            for (int i = 0; i < MemoryTestSize; i++)
            {
                boxedArray[i] = i; // Boxing happens here
            }
            
            // Measure memory after creating boxed array
            long memoryAfterBoxed = GetMemoryUsage();
            Console.WriteLine($"Memory after object[]: {memoryAfterBoxed:N0} KB");
            Console.WriteLine($"Memory used by object[]: {memoryAfterBoxed - memoryAfterRegular:N0} KB");
            
            // Calculate and display ratio
            double ratio = (double)(memoryAfterBoxed - memoryAfterRegular) / (memoryAfterRegular - memoryBefore);
            Console.WriteLine($"\nBoxed integers use approximately {ratio:F1}x more memory than regular integers");
            
            // Demonstrate memory release
            Console.WriteLine("\nReleasing arrays to free memory...");
            regularArray = null;
            boxedArray = null;
            ClearMemory();
            Console.WriteLine($"Memory after cleanup: {GetMemoryUsage():N0} KB");
            
            // Compare ArrayList vs List<T>
            Console.WriteLine("\n2. COLLECTIONS COMPARISON (ArrayList vs List<T>)");
            
            // Measure memory before creating collections
            ClearMemory();
            memoryBefore = GetMemoryUsage();
            Console.WriteLine($"Memory before collections: {memoryBefore:N0} KB");
            
            // Create ArrayList (causes boxing)
            Console.WriteLine("\nCreating ArrayList (non-generic, causes boxing)...");
            var arrayList = new ArrayList(MemoryTestSize);
            for (int i = 0; i < MemoryTestSize; i++)
            {
                arrayList.Add(i); // Boxing occurs here
            }
            
            // Measure memory after ArrayList
            long memoryAfterArrayList = GetMemoryUsage();
            Console.WriteLine($"Memory after ArrayList: {memoryAfterArrayList:N0} KB");
            Console.WriteLine($"Memory used by ArrayList: {memoryAfterArrayList - memoryBefore:N0} KB");
            
            // Create List<T> (no boxing)
            Console.WriteLine("\nCreating List<int> (generic, no boxing)...");
            var list = new List<int>(MemoryTestSize);
            for (int i = 0; i < MemoryTestSize; i++)
            {
                list.Add(i); // No boxing
            }
            
            // Measure memory after List<T>
            long memoryAfterList = GetMemoryUsage();
            Console.WriteLine($"Memory after List<int>: {memoryAfterList:N0} KB");
            Console.WriteLine($"Memory used by List<int>: {memoryAfterList - memoryAfterArrayList:N0} KB");
            
            // Calculate collection efficiency
            double collectionRatio = (double)(memoryAfterArrayList - memoryBefore) / (memoryAfterList - memoryAfterArrayList);
            Console.WriteLine($"\nArrayList uses approximately {collectionRatio:F1}x more memory than List<int>");
            
            // Clean up
            arrayList = null;
            list = null;
            ClearMemory();
        }
        
        static void DemonstratePerformanceImpact()
        {
            Console.WriteLine("\n3. PERFORMANCE IMPACT DEMONSTRATION");
            
            // ArrayList (causes boxing) - measure add + iterate
            Console.WriteLine("\nTesting ArrayList (with boxing/unboxing)...");
            var stopwatch1 = Stopwatch.StartNew();
            
            var arrayList = new ArrayList(CollectionSize);
            
            // Measure Add operations
            for (int i = 0; i < CollectionSize; i++)
            {
                arrayList.Add(i);  // Boxing occurs here
            }
            
            stopwatch1.Stop();
            long addTimeArrayList = stopwatch1.ElapsedMilliseconds;
            Console.WriteLine($"Time to add {CollectionSize:N0} items to ArrayList: {addTimeArrayList} ms");
            
            // Measure iteration/sum
            stopwatch1.Restart();
            int sum1 = 0;
            foreach (object item in arrayList)
            {
                sum1 += (int)item;  // Unboxing occurs here
            }
            stopwatch1.Stop();
            long iterateTimeArrayList = stopwatch1.ElapsedMilliseconds;
            Console.WriteLine($"Time to iterate and sum ArrayList: {iterateTimeArrayList} ms");
            Console.WriteLine($"Total ArrayList time: {addTimeArrayList + iterateTimeArrayList} ms");
            
            // List<T> (no boxing)
            Console.WriteLine("\nTesting List<int> (no boxing/unboxing)...");
            var stopwatch2 = Stopwatch.StartNew();
            
            var list = new List<int>(CollectionSize);
            
            // Measure Add operations
            for (int i = 0; i < CollectionSize; i++)
            {
                list.Add(i);  // No boxing
            }
            
            stopwatch2.Stop();
            long addTimeList = stopwatch2.ElapsedMilliseconds;
            Console.WriteLine($"Time to add {CollectionSize:N0} items to List<int>: {addTimeList} ms");
            
            // Measure iteration/sum
            stopwatch2.Restart();
            int sum2 = 0;
            foreach (int item in list)
            {
                sum2 += item;  // No unboxing
            }
            stopwatch2.Stop();
            long iterateTimeList = stopwatch2.ElapsedMilliseconds;
            Console.WriteLine($"Time to iterate and sum List<int>: {iterateTimeList} ms");
            Console.WriteLine($"Total List<int> time: {addTimeList + iterateTimeList} ms");
            
            // Calculate performance differences
            double addRatio = (double)addTimeArrayList / addTimeList;
            double iterateRatio = (double)iterateTimeArrayList / iterateTimeList;
            double totalRatio = (double)(addTimeArrayList + iterateTimeArrayList) / (addTimeList + iterateTimeList);
            
            Console.WriteLine("\nPerformance Comparison:");
            Console.WriteLine($"- Adding: ArrayList is {addRatio:F1}x slower than List<int>");
            Console.WriteLine($"- Iterating: ArrayList is {iterateRatio:F1}x slower than List<int>");
            Console.WriteLine($"- Overall: ArrayList is {totalRatio:F1}x slower than List<int>");
        }
        
        // Helper method to get current memory usage in KB
        static long GetMemoryUsage()
        {
            // Return memory usage in KB
            return GC.GetTotalMemory(false) / 1024;
        }
        
        // Helper method to clear memory
        static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}