using System;

namespace DesignPatternsDemo
{
    // PROBLEM EXAMPLE - Without Design Pattern
    // ----------------------------------------
    class ProblemExample
    {
        public static void DemonstrateProblem()
        {
            Console.WriteLine("=== Problem Without Design Pattern ===");
            
            // Direct instantiation of concrete classes
            PaymentProcessor processor;
            
            // Client code needs to know details about payment types
            string paymentType = "CreditCard";
            
            // Client needs to change when new payment methods are added
            if (paymentType == "CreditCard")
            {
                processor = new CreditCardProcessor();
            }
            else if (paymentType == "PayPal")
            {
                processor = new PayPalProcessor();
            }
            else
            {
                throw new ArgumentException("Unsupported payment method");
            }
            
            processor.ProcessPayment(100.0m);
            
            // Adding a new payment method requires changing existing code
            // This violates the Open/Closed Principle
        }
    }
    
    abstract class PaymentProcessor
    {
        public abstract void ProcessPayment(decimal amount);
    }
    
    class CreditCardProcessor : PaymentProcessor
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing ${amount} credit card payment");
        }
    }
    
    class PayPalProcessor : PaymentProcessor
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing ${amount} PayPal payment");
        }
    }
    
    // SOLUTION EXAMPLE - With Factory Pattern
    // ---------------------------------------
    class SolutionExample
    {
        public static void DemonstrateSolution()
        {
            Console.WriteLine("\n=== Solution With Factory Pattern ===");
            
            // Client works with factory interface instead of concrete classes
            IPaymentProcessorFactory factory;
            
            string paymentType = "CreditCard";
            
            // Factory is determined once, client doesn't need to know details
            if (paymentType == "CreditCard")
            {
                factory = new CreditCardProcessorFactory();
            }
            else if (paymentType == "PayPal")
            {
                factory = new PayPalProcessorFactory();
            }
            else
            {
                throw new ArgumentException("Unsupported payment method");
            }
            
            // Client works with abstract types, not concrete implementations
            PaymentProcessor processor = factory.CreateProcessor();
            processor.ProcessPayment(100.0m);
            
            Console.WriteLine("\n=== Adding New Payment Method ===");
            // Adding new payment method is easier now - we just add new factory
            // No change to existing client code needed
            var bankTransferFactory = new BankTransferProcessorFactory();
            var bankProcessor = bankTransferFactory.CreateProcessor();
            bankProcessor.ProcessPayment(200.0m);
        }
    }
    
    interface IPaymentProcessorFactory
    {
        PaymentProcessor CreateProcessor();
    }
    
    class CreditCardProcessorFactory : IPaymentProcessorFactory
    {
        public PaymentProcessor CreateProcessor()
        {
            return new CreditCardProcessor();
        }
    }
    
    class PayPalProcessorFactory : IPaymentProcessorFactory
    {
        public PaymentProcessor CreateProcessor()
        {
            return new PayPalProcessor();
        }
    }
    
    class BankTransferProcessorFactory : IPaymentProcessorFactory
    {
        public PaymentProcessor CreateProcessor()
        {
            return new BankTransferProcessor();
        }
    }
    
    class BankTransferProcessor : PaymentProcessor
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing ${amount} bank transfer payment");
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Design Patterns Introduction Demo\n");
            
            // Show problem without patterns
            ProblemExample.DemonstrateProblem();
            
            // Show solution with patterns
            SolutionExample.DemonstrateSolution();
            
            //Console.WriteLine("\nKey benefits demonstrated:");
            //Console.WriteLine("1. Reduces direct dependencies between components");
            //Console.WriteLine("2. Makes code more maintainable and extensible");
            //Console.WriteLine("3. Enforces SOLID principles (especially Open/Closed)");
            //Console.WriteLine("4. Provides common vocabulary for developers");
            
            Console.ReadLine();
        }
    }
}