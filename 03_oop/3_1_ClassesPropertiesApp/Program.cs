using System;

namespace ClassesAndProperties
{
    // Basic class with encapsulation using properties
    public class Customer
    {
        // Private fields
        private int id;
        private string name;
        private string email;
        
        // Full property with backing field
        public int CustomerId 
        { 
            get { return id; }
            set { id = value; }
        }
        
        // Auto-implemented property with default value
        public string CustomerName { get; set; } = "New customer";
        
        // Auto-implemented property
        public string Email { get; set; }
        
        // Read-only auto property
        public DateTime CreatedDate { get; } = DateTime.Now;
        
        // Property with validation
        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value < 0 || value > 120)
                    throw new ArgumentException("Age must be between 0 and 120");
                age = value;
            }
        }
        
        // Init-only property (C# 9.0+)
        public string CustomerCode { get; init; }
        
        // Default constructor
        public Customer()
        {
            // Initialize fields/properties
            CustomerCode = GenerateCode();
        }
        
        // Parameterized constructor
        public Customer(int id, string name, string email)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            CustomerCode = GenerateCode();
        }
        
        // Method
        public void PrintInfo()
        {
            Console.WriteLine($"ID: {id}, Name: {name}, Email: {email}");
            Console.WriteLine($"Created on: {CreatedDate}");
        }
        
        // Private helper method
        private string GenerateCode()
        {
            return "C-" + DateTime.Now.Ticks.ToString().Substring(10);
        }
        
        // Expression-bodied method
        public string GetFullDetails() => $"Customer {id}: {name} ({email})";
    }
    
    class Program
    {
        static void Main()
        {
            // Create an object using default constructor
            var customer1 = new Customer();
            customer1.CustomerId = 1001;
            customer1.CustomerName = "John Doe";
            customer1.Email = "john@example.com";
            customer1.Age = 30;
            customer1.PrintInfo();
            
            // Using object initializer syntax
            var customer2 = new Customer 
            { 
                CustomerId = 1002, 
                CustomerName = "Jane Smith", 
                Email = "jane@example.com",
                Age = 28
            };
            
            // Cannot change init-only property after initialization
            // customer2.CustomerCode = "NEW-CODE"; // This would cause a compile error
            
            Console.WriteLine(customer2.GetFullDetails());
            
            // Using parameterized constructor
            var customer3 = new Customer(1003, "Bob Johnson", "bob@example.com");
            Console.WriteLine($"Customer code: {customer3.CustomerCode}");
        }
    }
}