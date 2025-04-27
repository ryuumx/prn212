using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceDemo
{
    class Program
    {
        // Use significantly larger numbers to make performance differences visible
        private const int StringOpsCount = 100000;
        private const int LinqDataSize = 10000000;
        private const int ParallelProcessingSize = 10000000;
        
        static async Task Main(string[] args)
        {
            Console.WriteLine(".NET Core Performance Demo");
            
            // Sequential vs Parallel Processing
            Console.WriteLine("\nSequential vs Parallel Processing:");
            
            var stopwatch = Stopwatch.StartNew();
            long sequentialResult = ProcessSequentially(ParallelProcessingSize);
            stopwatch.Stop();
            Console.WriteLine($"Sequential processing result: {sequentialResult}, completed in {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch = Stopwatch.StartNew();
            long parallelResult = await ProcessInParallelAsync(ParallelProcessingSize);
            stopwatch.Stop();
            Console.WriteLine($"Parallel processing result: {parallelResult}, completed in {stopwatch.ElapsedMilliseconds}ms");
            
            // LINQ performance with large dataset
            Console.WriteLine("\nLINQ Performance with 10 million integers:");
            var numbers = Enumerable.Range(1, LinqDataSize).ToArray();
            
            stopwatch = Stopwatch.StartNew();
            double average1 = CalculateAverageManually(numbers);
            stopwatch.Stop();
            Console.WriteLine($"Manual average: {average1:F2} in {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch = Stopwatch.StartNew();
            double average2 = numbers.Average();
            stopwatch.Stop();
            Console.WriteLine($"LINQ average: {average2:F2} in {stopwatch.ElapsedMilliseconds}ms");
            
            // Complex LINQ query vs manual implementation
            Console.WriteLine("\nComplex Data Processing:");
            var data = GenerateTestData(1000000);
            
            stopwatch = Stopwatch.StartNew();
            var result1 = ProcessDataManually(data);
            stopwatch.Stop();
            Console.WriteLine($"Manual implementation: {stopwatch.ElapsedMilliseconds}ms, Results: {result1.Count} items");
            
            stopwatch = Stopwatch.StartNew();
            var result2 = ProcessDataWithLinq(data);
            stopwatch.Stop();
            Console.WriteLine($"LINQ implementation: {stopwatch.ElapsedMilliseconds}ms, Results: {result2.Count()} items");
            
            // String operations comparison with large strings
            Console.WriteLine("\nString Operations Performance (100,000 concatenations):");
            
            stopwatch = Stopwatch.StartNew();
            string concatResult = ConcatenateStrings(StringOpsCount);
            stopwatch.Stop();
            Console.WriteLine($"String concatenation: {stopwatch.ElapsedMilliseconds}ms for {concatResult.Length} characters");
            
            stopwatch = Stopwatch.StartNew();
            string builderResult = ConcatenateWithStringBuilder(StringOpsCount);
            stopwatch.Stop();
            Console.WriteLine($"StringBuilder: {stopwatch.ElapsedMilliseconds}ms for {builderResult.Length} characters");
            
            // List vs Array performance
            Console.WriteLine("\nList vs Array Performance:");
            const int iterations = 100000000;
            
            stopwatch = Stopwatch.StartNew();
            long arraySum = SumWithArray(iterations);
            stopwatch.Stop();
            Console.WriteLine($"Array sum: {arraySum} in {stopwatch.ElapsedMilliseconds}ms");
            
            stopwatch = Stopwatch.StartNew();
            long listSum = SumWithList(iterations);
            stopwatch.Stop();
            Console.WriteLine($"List sum: {listSum} in {stopwatch.ElapsedMilliseconds}ms");
        }
        
        static long ProcessSequentially(int count)
        {
            long total = 0;
            for (int i = 0; i < count; i++)
            {
                total += PerformCalculation(i);
            }
            return total;
        }
        
        static async Task<long> ProcessInParallelAsync(int count)
        {
            var tasks = new List<Task<long>>();
            int batchSize = count / Environment.ProcessorCount;
            
            for (int i = 0; i < count; i += batchSize)
            {
                int start = i;
                int end = Math.Min(i + batchSize, count);
                
                tasks.Add(Task.Run(() => {
                    long sum = 0;
                    for (int j = start; j < end; j++)
                    {
                        sum += PerformCalculation(j);
                    }
                    return sum;
                }));
            }
            
            var results = await Task.WhenAll(tasks);
            return results.Sum();
        }
        
        static long PerformCalculation(int value)
        {
            // A calculation complex enough to benefit from parallelization
            long result = 0;
            for (int i = 0; i < 100; i++)
            {
                result += (value * i) % 10;
            }
            return result;
        }
        
        static double CalculateAverageManually(int[] numbers)
        {
            double sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            return sum / numbers.Length;
        }
        
        static string ConcatenateStrings(int count)
        {
            string result = string.Empty;
            for (int i = 0; i < count; i++)
            {
                result += "a";
            }
            return result;
        }
        
        static string ConcatenateWithStringBuilder(int count)
        {
            StringBuilder sb = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                sb.Append("a");
            }
            return sb.ToString();
        }
        
        static List<TestItem> GenerateTestData(int count)
        {
            var result = new List<TestItem>(count);
            var random = new Random(42); // Fixed seed for reproducibility
            
            for (int i = 0; i < count; i++)
            {
                result.Add(new TestItem
                {
                    Id = i,
                    Value = random.Next(1, 1000),
                    Category = (char)('A' + random.Next(0, 26)),
                    IsActive = random.Next(100) < 70 // 70% chance of being active
                });
            }
            
            return result;
        }
        
        static List<TestItem> ProcessDataManually(List<TestItem> data)
        {
            var result = new List<TestItem>();
            foreach (var item in data)
            {
                if (item.IsActive && item.Value > 500 && item.Category < 'N')
                {
                    result.Add(item);
                }
            }
            
            // Sort by Value descending
            result.Sort((a, b) => b.Value.CompareTo(a.Value));
            
            // Take top 1000 or all if less
            if (result.Count > 1000)
            {
                return result.GetRange(0, 1000);
            }
            return result;
        }
        
        static IEnumerable<TestItem> ProcessDataWithLinq(List<TestItem> data)
        {
            return data
                .Where(item => item.IsActive && item.Value > 500 && item.Category < 'N')
                .OrderByDescending(item => item.Value)
                .Take(1000);
        }
        
        static long SumWithArray(int size)
        {
            int[] array = new int[100];
            for (int i = 0; i < 100; i++)
            {
                array[i] = i;
            }
            
            long sum = 0;
            for (int j = 0; j < size; j++)
            {
                sum += array[j % 100];
            }
            return sum;
        }
        
        static long SumWithList(int size)
        {
            List<int> list = new List<int>(100);
            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }
            
            long sum = 0;
            for (int j = 0; j < size; j++)
            {
                sum += list[j % 100];
            }
            return sum;
        }
    }
    
    class TestItem
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public char Category { get; set; }
        public bool IsActive { get; set; }
    }
}