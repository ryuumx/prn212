using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SerializationDemos
{
    public class JsonSerializationDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== JSON Serialization Demonstrations ===");
            
            // Demo 1: Basic JSON Serialization
            DemonstrateBasicJsonSerialization();
            
            // Demo 2: JSON Serialization Options
            DemonstrateJsonSerializationOptions();
            
            // Demo 3: Complex Object Serialization
            DemonstrateComplexJsonSerialization();
            
            // Demo 4: JSON Deserialization
            DemonstrateJsonDeserialization();
            
            // Demo 5: Error Handling and Validation
            DemonstrateJsonErrorHandling();
        }

        private static void DemonstrateBasicJsonSerialization()
        {
            Console.WriteLine("\n=== Basic JSON Serialization ===");
            
            var customer = new Customer(1, "John Doe", "john.doe@email.com", true);
            customer.InternalNotes = "This should not appear in JSON";
            
            // Basic serialization
            string jsonString = JsonSerializer.Serialize(customer);
            Console.WriteLine("Basic JSON output:");
            Console.WriteLine(jsonString);
            
            // Pretty-printed serialization
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            string prettyJson = JsonSerializer.Serialize(customer, options);
            Console.WriteLine("\nPretty-printed JSON:");
            Console.WriteLine(prettyJson);
            
            // Save to file
            File.WriteAllText("customer_basic.json", prettyJson);
            Console.WriteLine("\nSaved to customer_basic.json");
        }

        private static void DemonstrateJsonSerializationOptions()
        {
            Console.WriteLine("\n=== JSON Serialization Options ===");
            
            var customer = new Customer(2, "Jane Smith", "jane.smith@email.com");
            
            // Different naming policies
            var camelCaseOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            string camelCaseJson = JsonSerializer.Serialize(customer, camelCaseOptions);
            Console.WriteLine("CamelCase naming policy:");
            Console.WriteLine(camelCaseJson);
            
            // Custom serialization options
            var customOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            };
            
            // Add some data for demonstration
            customer.Orders.Add(new Order(1001, 299.99m, OrderStatus.Processing));
            
            string customJson = JsonSerializer.Serialize(customer, customOptions);
            Console.WriteLine("\nCustom options (camelCase, ignore nulls, enum as string):");
            Console.WriteLine(customJson);
            
            File.WriteAllText("customer_custom_options.json", customJson);
        }

        private static void DemonstrateComplexJsonSerialization()
        {
            Console.WriteLine("\n=== Complex Object JSON Serialization ===");
            
            // Create complex customer data
            var customer = new Customer(3, "Alice Johnson", "alice@techcorp.com");
            
            // Add orders with items
            var order1 = new Order(2001, 0, OrderStatus.Delivered);
            order1.Items.AddRange(new[]
            {
                new OrderItem("Laptop Pro", 1, 1299.99m),
                new OrderItem("Wireless Mouse", 2, 29.99m),
                new OrderItem("USB Cable", 3, 9.99m)
            });
            order1.TotalAmount = order1.Items.Sum(item => item.Subtotal);
            
            var order2 = new Order(2002, 0, OrderStatus.Shipped);
            order2.Items.AddRange(new[]
            {
                new OrderItem("Programming Book", 2, 49.95m),
                new OrderItem("Coffee Mug", 1, 12.99m)
            });
            order2.TotalAmount = order2.Items.Sum(item => item.Subtotal);
            
            customer.Orders.AddRange(new[] { order1, order2 });
            
            // Serialize with all options
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };
            
            string complexJson = JsonSerializer.Serialize(customer, options);
            Console.WriteLine("Complex customer with orders and items:");
            Console.WriteLine(complexJson);
            
            File.WriteAllText("customer_complex.json", complexJson);
            
            // Create API response wrapper
            var apiResponse = new ApiResponse<Customer>(true, "Customer retrieved successfully", customer);
            string apiJson = JsonSerializer.Serialize(apiResponse, options);
            
            Console.WriteLine("\nAPI Response format:");
            Console.WriteLine(apiJson);
            
            File.WriteAllText("api_response.json", apiJson);
        }

        private static void DemonstrateJsonDeserialization()
        {
            Console.WriteLine("\n=== JSON Deserialization ===");
            
            // Deserialize from files created earlier
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };
            
            // Deserialize basic customer
            if (File.Exists("customer_basic.json"))
            {
                string basicJson = File.ReadAllText("customer_basic.json");
                Customer basicCustomer = JsonSerializer.Deserialize<Customer>(basicJson, options);
                
                Console.WriteLine("Deserialized basic customer:");
                Console.WriteLine($"  ID: {basicCustomer.CustomerId}");
                Console.WriteLine($"  Name: {basicCustomer.CustomerName}");
                Console.WriteLine($"  Email: {basicCustomer.Email}");
                Console.WriteLine($"  Registration: {basicCustomer.RegistrationDate}");
                Console.WriteLine($"  Active: {basicCustomer.IsActive}");
            }
            
            // Deserialize complex customer
            if (File.Exists("customer_complex.json"))
            {
                string complexJson = File.ReadAllText("customer_complex.json");
                Customer complexCustomer = JsonSerializer.Deserialize<Customer>(complexJson, options);
                
                Console.WriteLine($"\nDeserialized complex customer: {complexCustomer.CustomerName}");
                Console.WriteLine($"  Orders: {complexCustomer.Orders.Count}");
                
                foreach (var order in complexCustomer.Orders)
                {
                    Console.WriteLine($"  Order {order.OrderId}: ${order.TotalAmount} ({order.Status})");
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"    - {item.ProductName}: {item.Quantity} x ${item.UnitPrice} = ${item.Subtotal}");
                    }
                }
            }
            
            // Deserialize API response
            if (File.Exists("api_response.json"))
            {
                string apiJson = File.ReadAllText("api_response.json");
                ApiResponse<Customer> apiResponse = JsonSerializer.Deserialize<ApiResponse<Customer>>(apiJson, options);
                
                Console.WriteLine($"\nDeserialized API Response:");
                Console.WriteLine($"  Success: {apiResponse.Success}");
                Console.WriteLine($"  Message: {apiResponse.Message}");
                Console.WriteLine($"  Timestamp: {apiResponse.Timestamp}");
                Console.WriteLine($"  Customer: {apiResponse.Data?.CustomerName}");
            }
        }

        private static void DemonstrateJsonErrorHandling()
        {
            Console.WriteLine("\n=== JSON Error Handling ===");
            
            // Test with invalid JSON
            string invalidJson = @"{
                ""customerId"": ""not_a_number"",
                ""customerName"": ""Test User"",
                ""email"": ""test@email.com"",
                ""registrationDate"": ""invalid_date"",
                ""isActive"": ""not_a_boolean""
            }";
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            try
            {
                Customer customer = JsonSerializer.Deserialize<Customer>(invalidJson, options);
                Console.WriteLine("This shouldn't print - JSON should be invalid");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                Console.WriteLine($"Path: {ex.Path}");
                Console.WriteLine($"Line: {ex.LineNumber}, Position: {ex.BytePositionInLine}");
            }
            
            // Test with missing required properties
            string incompleteJson = @"{
                ""customerName"": ""Incomplete User""
            }";
            
            try
            {
                Customer customer = JsonSerializer.Deserialize<Customer>(incompleteJson, options);
                Console.WriteLine($"\nDeserialized incomplete JSON:");
                Console.WriteLine($"  ID: {customer.CustomerId} (default value)");
                Console.WriteLine($"  Name: {customer.CustomerName}");
                Console.WriteLine($"  Email: {customer.Email ?? "null"}");
                Console.WriteLine($"  Registration: {customer.RegistrationDate} (default value)");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error with incomplete JSON: {ex.Message}");
            }
            
            // Test with extra properties
            string extraPropertiesJson = @"{
                ""customerId"": 999,
                ""customerName"": ""Extra Props User"",
                ""email"": ""extra@email.com"",
                ""unknownProperty"": ""this will be ignored"",
                ""anotherExtra"": 42
            }";
            
            try
            {
                Customer customer = JsonSerializer.Deserialize<Customer>(extraPropertiesJson, options);
                Console.WriteLine($"\nDeserialized JSON with extra properties:");
                Console.WriteLine($"  Successfully ignored unknown properties");
                Console.WriteLine($"  Customer: {customer.CustomerName}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error with extra properties: {ex.Message}");
            }
        }
    }
}