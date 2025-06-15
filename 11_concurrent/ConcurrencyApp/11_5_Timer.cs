using System;
using System.Threading;

namespace ConcurrencyDemos
{
    public class TimerDemo
    {
        private static int _timerCallCount = 0;
        private static Timer _periodicTimer;
        private static Timer _oneShotTimer;

        public static void RunDemo()
        {
            Console.WriteLine("=== Timer Demonstrations ===");
            
            DemonstrateBasicTimer();
            DemonstratePeriodicTimer();
            DemonstrateTimerWithState();
            DemonstrateTimerControl();
        }

        private static void DemonstrateBasicTimer()
        {
            Console.WriteLine("\n=== Basic Timer Demo ===");
            
            Console.WriteLine("Creating a one-shot timer (fires once after 2 seconds)...");
            
            _oneShotTimer = new Timer(OneShotTimerCallback, "OneShot", 2000, Timeout.Infinite);
            
            // Wait for the timer to fire
            Thread.Sleep(3000);
            
            _oneShotTimer?.Dispose();
            Console.WriteLine("One-shot timer disposed");
        }

        private static void OneShotTimerCallback(object state)
        {
            string timerName = (string)state;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {timerName} timer fired on thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Is background thread: {Thread.CurrentThread.IsBackground}");
            Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
        }

        private static void DemonstratePeriodicTimer()
        {
            Console.WriteLine("\n=== Periodic Timer Demo ===");
            
            _timerCallCount = 0;
            
            Console.WriteLine("Creating a periodic timer (fires every 500ms after 1 second delay)...");
            
            _periodicTimer = new Timer(PeriodicTimerCallback, null, 1000, 500);
            
            // Let it run for several intervals
            Thread.Sleep(4000);
            
            Console.WriteLine("Stopping periodic timer...");
            _periodicTimer?.Dispose();
            Console.WriteLine($"Periodic timer stopped after {_timerCallCount} calls");
        }

        private static void PeriodicTimerCallback(object state)
        {
            _timerCallCount++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Periodic timer call #{_timerCallCount} on thread {Thread.CurrentThread.ManagedThreadId}");
            
            // Simulate some work
            Thread.Sleep(100);
            
            if (_timerCallCount >= 6)
            {
                Console.WriteLine("Timer has fired enough times, stopping...");
                _periodicTimer?.Change(Timeout.Infinite, Timeout.Infinite); // Stop the timer
            }
        }

        private static void DemonstrateTimerWithState()
        {
            Console.WriteLine("\n=== Timer with State Demo ===");
            
            var timerState = new TimerState
            {
                Name = "StatefulTimer",
                MaxExecutions = 3,
                CurrentExecution = 0
            };
            
            Timer statefulTimer = new Timer(StatefulTimerCallback, timerState, 500, 1000);
            
            // Wait for all executions to complete
            while (timerState.CurrentExecution < timerState.MaxExecutions)
            {
                Thread.Sleep(200);
            }
            
            statefulTimer.Dispose();
            Console.WriteLine("Stateful timer demo completed");
        }

        private static void StatefulTimerCallback(object state)
        {
            if (state is TimerState timerState)
            {
                timerState.CurrentExecution++;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {timerState.Name} execution {timerState.CurrentExecution}/{timerState.MaxExecutions}");
                
                // Simulate work that varies based on execution number
                int workDuration = timerState.CurrentExecution * 200;
                Thread.Sleep(workDuration);
                
                Console.WriteLine($"  Work completed in {workDuration}ms");
            }
        }

        private static void DemonstrateTimerControl()
        {
            Console.WriteLine("\n=== Timer Control Demo ===");
            
            var controlledTimer = new Timer(ControlledTimerCallback, "Controlled", Timeout.Infinite, Timeout.Infinite);
            
            Console.WriteLine("Timer created but not started...");
            Thread.Sleep(1000);
            
            Console.WriteLine("Starting timer with 500ms intervals...");
            controlledTimer.Change(0, 500); // Start immediately, repeat every 500ms
            Thread.Sleep(2000);
            
            Console.WriteLine("Pausing timer...");
            controlledTimer.Change(Timeout.Infinite, Timeout.Infinite); // Pause
            Thread.Sleep(1000);
            
            Console.WriteLine("Resuming timer with 300ms intervals...");
            controlledTimer.Change(100, 300); // Resume after 100ms, repeat every 300ms
            Thread.Sleep(1500);
            
            Console.WriteLine("Stopping timer...");
            controlledTimer.Dispose();
            
            // Wait a bit to ensure no more callbacks
            Thread.Sleep(500);
            Console.WriteLine("Timer control demo completed");
        }

        private static void ControlledTimerCallback(object state)
        {
            string timerName = (string)state;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {timerName} timer tick");
        }
    }

    public class TimerState
    {
        public string Name { get; set; }
        public int MaxExecutions { get; set; }
        public int CurrentExecution { get; set; }
    }
}