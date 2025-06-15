using System;
using System.Threading;

namespace ConcurrencyDemos
{
    public class RaceConditionDemo
    {
        private static int _unsafeCounter = 0;
        private static int _safeCounter = 0;
        private static readonly object _lockObject = new object();
        
        public static void RunDemo()
        {
            Console.WriteLine("=== Race Condition Demonstrations ===");
            
            DemonstrateRaceCondition();
            DemonstrateLockSolution();
            DemonstrateInterlockedOperations();
            DemonstrateMonitorUsage();
        }

        private static void DemonstrateRaceCondition()
        {
            Console.WriteLine("\n=== Race Condition Problem ===");
            
            _unsafeCounter = 0;
            const int incrementsPerThread = 100000;
            const int numberOfThreads = 5;
            
            Thread[] threads = new Thread[numberOfThreads];
            
            // Create threads that increment the counter
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadId = i + 1;
                threads[i] = new Thread(() => UnsafeIncrement(threadId, incrementsPerThread));
                threads[i].Name = $"UnsafeThread{threadId}";
            }
            
            Console.WriteLine($"Starting {numberOfThreads} threads, each incrementing {incrementsPerThread} times");
            Console.WriteLine($"Expected final value: {numberOfThreads * incrementsPerThread}");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Start all threads
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            
            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            stopwatch.Stop();
            
            Console.WriteLine($"Actual final value: {_unsafeCounter}");
            Console.WriteLine($"Lost increments: {(numberOfThreads * incrementsPerThread) - _unsafeCounter}");
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine("This demonstrates the race condition problem!");
        }

        private static void UnsafeIncrement(int threadId, int incrementCount)
        {
            for (int i = 0; i < incrementCount; i++)
            {
                // This is NOT thread-safe!
                _unsafeCounter++;
                
                // Occasionally report progress
                if (i % 20000 == 0)
                {
                    Console.WriteLine($"Thread {threadId}: {i} increments completed");
                }
            }
        }

        private static void DemonstrateLockSolution()
        {
            Console.WriteLine("\n=== Lock Statement Solution ===");
            
            _safeCounter = 0;
            const int incrementsPerThread = 100000;
            const int numberOfThreads = 5;
            
            Thread[] threads = new Thread[numberOfThreads];
            
            // Create threads that safely increment the counter
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadId = i + 1;
                threads[i] = new Thread(() => SafeIncrement(threadId, incrementsPerThread));
                threads[i].Name = $"SafeThread{threadId}";
            }
            
            Console.WriteLine($"Starting {numberOfThreads} threads with lock protection");
            Console.WriteLine($"Expected final value: {numberOfThreads * incrementsPerThread}");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Start all threads
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            
            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            stopwatch.Stop();
            
            Console.WriteLine($"Actual final value: {_safeCounter}");
            Console.WriteLine($"Lost increments: {(numberOfThreads * incrementsPerThread) - _safeCounter}");
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine("Lock statement prevents race conditions!");
        }

        private static void SafeIncrement(int threadId, int incrementCount)
        {
            for (int i = 0; i < incrementCount; i++)
            {
                // This IS thread-safe!
                lock (_lockObject)
                {
                    _safeCounter++;
                }
                
                // Report progress outside the lock
                if (i % 20000 == 0)
                {
                    Console.WriteLine($"Safe Thread {threadId}: {i} increments completed");
                }
            }
        }

        private static void DemonstrateInterlockedOperations()
        {
            Console.WriteLine("\n=== Interlocked Operations ===");
            
            int interlockedCounter = 0;
            const int incrementsPerThread = 100000;
            const int numberOfThreads = 5;
            
            Thread[] threads = new Thread[numberOfThreads];
            
            // Create threads using Interlocked operations
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadId = i + 1;
                threads[i] = new Thread(() => InterlockedIncrement(threadId, incrementsPerThread, ref interlockedCounter));
                threads[i].Name = $"InterlockedThread{threadId}";
            }
            
            Console.WriteLine($"Starting {numberOfThreads} threads with Interlocked operations");
            Console.WriteLine($"Expected final value: {numberOfThreads * incrementsPerThread}");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Start all threads
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            
            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            stopwatch.Stop();
            
            Console.WriteLine($"Actual final value: {interlockedCounter}");
            Console.WriteLine($"Lost increments: {(numberOfThreads * incrementsPerThread) - interlockedCounter}");
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine("Interlocked operations are atomic and thread-safe!");
        }

        private static void InterlockedIncrement(int threadId, int incrementCount, ref int counter)
        {
            for (int i = 0; i < incrementCount; i++)
            {
                // Atomic increment operation
                Interlocked.Increment(ref counter);
                
                // Report progress
                if (i % 20000 == 0)
                {
                    Console.WriteLine($"Interlocked Thread {threadId}: {i} increments completed");
                }
            }
        }

        private static void DemonstrateMonitorUsage()
        {
            Console.WriteLine("\n=== Monitor Class Usage ===");
            
            var account = new BankAccount(1000);
            
            // Create multiple threads that perform transactions
            Thread[] threads = new Thread[4];
            
            threads[0] = new Thread(() => PerformTransactions(account, "Customer1", new[] { 100, -50, 75 }));
            threads[1] = new Thread(() => PerformTransactions(account, "Customer2", new[] { -200, 150, -25 }));
            threads[2] = new Thread(() => PerformTransactions(account, "Customer3", new[] { 50, -100, 200 }));
            threads[3] = new Thread(() => PerformTransactions(account, "Customer4", new[] { -75, 25, -150 }));
            
            Console.WriteLine($"Starting balance: ${account.Balance}");
            
            // Start all transaction threads
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            
            // Wait for all transactions to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            Console.WriteLine($"Final balance: ${account.Balance}");
        }

        private static void PerformTransactions(BankAccount account, string customerName, int[] transactions)
        {
            foreach (int amount in transactions)
            {
                if (amount > 0)
                {
                    account.Deposit(amount);
                    Console.WriteLine($"{customerName}: Deposited ${amount}");
                }
                else
                {
                    bool success = account.Withdraw(-amount);
                    Console.WriteLine($"{customerName}: Withdraw ${-amount} - {(success ? "Success" : "Failed (insufficient funds)")}");
                }
                
                Thread.Sleep(100); // Simulate processing time
            }
        }
    }

    // Thread-safe bank account using Monitor
    public class BankAccount
    {
        private decimal _balance;
        private readonly object _lockObject = new object();

        public BankAccount(decimal initialBalance)
        {
            _balance = initialBalance;
        }

        public decimal Balance
        {
            get
            {
                lock (_lockObject)
                {
                    return _balance;
                }
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive");

            Monitor.Enter(_lockObject);
            try
            {
                _balance += amount;
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            Monitor.Enter(_lockObject);
            try
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    return true;
                }
                return false;
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }
    }
}