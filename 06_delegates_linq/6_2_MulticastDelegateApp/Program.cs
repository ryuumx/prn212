using System;

namespace MulticastDevegate
{
    // Delegates for multicast demonstration
    public delegate void NotificationHandler(string message);
    public delegate int MathCalculation(int x, int y);

    public class MulticastDelegates
    {
        // Notification methods
        public static void SendEmail(string message)
        {
            Console.WriteLine($"📧 EMAIL: {message}");
        }

        public static void SendSMS(string message)
        {
            Console.WriteLine($"📱 SMS: {message}");
        }

        public static void LogToFile(string message)
        {
            Console.WriteLine($"📄 FILE LOG: [{DateTime.Now:HH:mm:ss}] {message}");
        }

        public static void ShowNotification(string message)
        {
            Console.WriteLine($"🔔 NOTIFICATION: {message}");
        }

        // Math methods for return value demonstration
        public static int AddNumbers(int x, int y)
        {
            int result = x + y;
            Console.WriteLine($"Add: {x} + {y} = {result}");
            return result;
        }

        public static int MultiplyNumbers(int x, int y)
        {
            int result = x * y;
            Console.WriteLine($"Multiply: {x} * {y} = {result}");
            return result;
        }

        public static int SubtractNumbers(int x, int y)
        {
            int result = x - y;
            Console.WriteLine($"Subtract: {x} - {y} = {result}");
            return result;
        }

        // Method to demonstrate Delegate.Combine vs += operator
        public static void DelegateCombineDemo()
        {
            Console.WriteLine("=== DELEGATE.COMBINE() VS += OPERATOR ===");
            
            NotificationHandler handler1 = SendEmail;
            NotificationHandler handler2 = SendSMS;
            
            // Using Delegate.Combine
            NotificationHandler combined1 = (NotificationHandler)Delegate.Combine(handler1, handler2);
            Console.WriteLine("Combined using Delegate.Combine:");
            combined1?.Invoke("Message via Combine");
            Console.WriteLine();

            // Using += operator (more common)
            NotificationHandler combined2 = SendEmail;
            combined2 += SendSMS;
            Console.WriteLine("Combined using += operator:");
            combined2?.Invoke("Message via += operator");
            Console.WriteLine();
        }

        // Method to show invocation list
        public static void ShowInvocationList(NotificationHandler handler)
        {
            if (handler != null)
            {
                Delegate[] invocationList = handler.GetInvocationList();
                Console.WriteLine($"Invocation list contains {invocationList.Length} methods:");
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {invocationList[i].Method.Name}");
                }
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("=== MULTICAST DELEGATES DEMO ===");

            // 1. Creating multicast delegate step by step
            Console.WriteLine("1. Building multicast delegate step by step:");
            NotificationHandler notifier = SendEmail;
            notifier += SendSMS;
            notifier += LogToFile;
            notifier += ShowNotification;

            ShowInvocationList(notifier);
            Console.WriteLine();

            // Invoke all methods in the multicast delegate
            Console.WriteLine("Invoking multicast delegate:");
            notifier?.Invoke("System maintenance scheduled for tonight");
            Console.WriteLine();

            // 2. Removing methods from multicast delegate
            Console.WriteLine("2. Removing SMS from the chain:");
            notifier -= SendSMS;
            ShowInvocationList(notifier);
            Console.WriteLine();

            Console.WriteLine("Invoking after removal:");
            notifier?.Invoke("SMS removed from notification chain");
            Console.WriteLine();

            // 3. Multicast delegates with return values (only last return value is kept)
            Console.WriteLine("3. Multicast delegates with return values:");
            MathCalculation mathChain = AddNumbers;
            mathChain += MultiplyNumbers;
            mathChain += SubtractNumbers;

            Console.WriteLine("Calling multicast delegate with return values:");
            int finalResult = mathChain(10, 5);
            Console.WriteLine($"Final result (only last method's return): {finalResult}");
            Console.WriteLine();

            // 4. Demonstrate Delegate.Combine vs += operator
            DelegateCombineDemo();

            // 5. Null delegate handling
            Console.WriteLine("4. Null delegate handling:");
            NotificationHandler nullHandler = null;
            nullHandler += SendEmail;  // Adding to null creates new delegate
            nullHandler?.Invoke("Message after adding to null");
            
            nullHandler -= SendEmail;  // Removing last method makes it null again
            Console.WriteLine($"After removing last method, handler is null: {nullHandler == null}");
            Console.WriteLine();

            // 6. Exception handling in multicast delegates
            Console.WriteLine("5. Exception handling in multicast delegates:");
            NotificationHandler riskyHandler = SendEmail;
            riskyHandler += ThrowException;
            riskyHandler += SendSMS;  // This won't execute if exception occurs

            try
            {
                riskyHandler?.Invoke("This will cause an exception");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
                Console.WriteLine("Note: Methods after the exception are not called");
            }

            Console.ReadKey();
        }

        // Method that throws exception for demonstration
        public static void ThrowException(string message)
        {
            Console.WriteLine($"💥 About to throw exception for: {message}");
            throw new InvalidOperationException("Simulated exception in delegate chain");
        }
    }
}