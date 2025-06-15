using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace SerializationDemos
{
    // Wrapper class for XML serialization (anonymous types don't work with XML)
    [XmlRoot("CustomerList")]
    public class CustomerListWrapper
    {
        [XmlElement("Customer")]
        public List<Customer> Customers { get; set; } = new List<Customer>();
        
        public CustomerListWrapper() { }
    }

    public class SerializationComparison
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Serialization Comparison and Best Practices ===");
            
            DemonstratePerformanceComparison();
            DemonstrateSizeComparison();
            DemonstrateVersionTolerance();
            DemonstrateBestPractices();
        }

        private static void DemonstratePerformanceComparison()
        {
            Console.WriteLine("\n=== Performance Comparison ===");
            
            // Create test data
            var customers = CreateTestCustomers(1000);
            
            Stopwatch sw = new Stopwatch();
            
            // XML Serialization timing
            sw.Start();
            string xmlResult = SerializeToXml(customers);
            sw.Stop();
            long xmlSerializationTime = sw.ElapsedMilliseconds;
            
            sw.Restart();
            var xmlDeserialized = DeserializeFromXml(xmlResult);
            sw.Stop();
            long xmlDeserializationTime = sw.ElapsedMilliseconds;
            
            // JSON Serialization timing
            sw.Restart();
            string jsonResult = SerializeToJson(customers);
            sw.Stop();
            long jsonSerializationTime = sw.ElapsedMilliseconds;
            
            sw.Restart();
            var jsonDeserialized = DeserializeFromJson(jsonResult);
            sw.Stop();
            long jsonDeserializationTime = sw.ElapsedMilliseconds;
            
            Console.WriteLine("Performance Results (1000 customers):");
            Console.WriteLine($"XML Serialization:   {xmlSerializationTime} ms");
            Console.WriteLine($"XML Deserialization: {xmlDeserializationTime} ms");
            Console.WriteLine($"JSON Serialization:  {jsonSerializationTime} ms");
            Console.WriteLine($"JSON Deserialization: {jsonDeserializationTime} ms");
            
            // Avoid division by zero
            if (jsonSerializationTime > 0)
                Console.WriteLine($"\nJSON is ~{(double)xmlSerializationTime / jsonSerializationTime:F1}x faster for serialization");
            else
                Console.WriteLine($"\nJSON serialization: <1ms, XML: {xmlSerializationTime}ms");
                
            if (jsonDeserializationTime > 0)
                Console.WriteLine($"JSON is ~{(double)xmlDeserializationTime / jsonDeserializationTime:F1}x faster for deserialization");
            else
                Console.WriteLine($"JSON deserialization: <1ms, XML: {xmlDeserializationTime}ms");
                
            Console.WriteLine($"\nSuccessfully processed {xmlDeserialized.Customers.Count} customers via XML");
            Console.WriteLine($"Successfully processed {jsonDeserialized.Customers.Count} customers via JSON");
        }

        private static void DemonstrateSizeComparison()
        {
            Console.WriteLine("\n=== Size Comparison ===");
            
            var customers = CreateTestCustomers(100);
            
            string xmlData = SerializeToXml(customers);
            string jsonData = SerializeToJson(customers);
            
            // Write to files for accurate size measurement
            File.WriteAllText("size_test.xml", xmlData);
            File.WriteAllText("size_test.json", jsonData);
            
            long xmlSize = new FileInfo("size_test.xml").Length;
            long jsonSize = new FileInfo("size_test.json").Length;
            
            Console.WriteLine($"XML Size:  {xmlSize:N0} bytes");
            Console.WriteLine($"JSON Size: {jsonSize:N0} bytes");
            Console.WriteLine($"JSON is {(double)xmlSize / jsonSize:F1}x smaller than XML");
            
            // Cleanup
            File.Delete("size_test.xml");
            File.Delete("size_test.json");
        }

        private static void DemonstrateVersionTolerance()
        {
            Console.WriteLine("\n=== Version Tolerance ===");
            
            // Create JSON with missing and extra fields
            string jsonV1 = @"{
                ""customerId"": 1,
                ""customerName"": ""John Doe""
            }";
            
            string jsonV2 = @"{
                ""customerId"": 2,
                ""customerName"": ""Jane Smith"",
                ""email"": ""jane@email.com"",
                ""registrationDate"": ""2024-01-15T10:30:00"",
                ""isActive"": true,
                ""futureField"": ""This field doesn't exist in our model yet""
            }";
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            try
            {
                Customer customerV1 = JsonSerializer.Deserialize<Customer>(jsonV1, options);
                Customer customerV2 = JsonSerializer.Deserialize<Customer>(jsonV2, options);
                
                Console.WriteLine("Successfully handled version differences:");
                Console.WriteLine($"V1 Customer: {customerV1.CustomerName} (missing fields filled with defaults)");
                Console.WriteLine($"V2 Customer: {customerV2.CustomerName} (extra fields ignored)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Version tolerance error: {ex.Message}");
            }
        }

        private static void DemonstrateBestPractices()
        {
            Console.WriteLine("\n=== Best Practices Demonstration ===");
            
            Console.WriteLine("1. Use appropriate data types:");
            Console.WriteLine("   ✓ DateTime for dates, not strings");
            Console.WriteLine("   ✓ Enums for fixed sets of values");
            Console.WriteLine("   ✓ Nullable types for optional values");
            
            Console.WriteLine("\n2. Handle errors gracefully:");
            Console.WriteLine("   ✓ Use try-catch for deserialization");
            Console.WriteLine("   ✓ Validate data after deserialization");
            Console.WriteLine("   ✓ Provide meaningful error messages");
            
            Console.WriteLine("\n3. Performance considerations:");
            Console.WriteLine("   ✓ Reuse JsonSerializerOptions instances");
            Console.WriteLine("   ✓ Use streaming for large data sets");
            Console.WriteLine("   ✓ Consider async methods for I/O operations");
            
            Console.WriteLine("\n4. Security considerations:");
            Console.WriteLine("   ✓ Validate input data");
            Console.WriteLine("   ✓ Don't serialize sensitive information");
            Console.WriteLine("   ✓ Use [JsonIgnore] for internal properties");
            
            // Demonstrate reusing options
            var reusableOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            
            var customer = new Customer(1, "Best Practices User", "bp@email.com");
            
            // Good: Reuse options
            string json1 = JsonSerializer.Serialize(customer, reusableOptions);
            string json2 = JsonSerializer.Serialize(customer, reusableOptions);
            
            Console.WriteLine("\n✓ Demonstrated reusing JsonSerializerOptions for better performance");
        }

        private static List<Customer> CreateTestCustomers(int count)
        {
            var customers = new List<Customer>();
            var random = new Random();
            
            for (int i = 1; i <= count; i++)
            {
                var customer = new Customer(i, $"Customer {i}", $"customer{i}@test.com");
                
                // Add random orders
                for (int j = 0; j < random.Next(0, 4); j++)
                {
                    var order = new Order(i * 100 + j, random.Next(50, 500));
                    order.Items.Add(new OrderItem($"Product {j + 1}", random.Next(1, 5), random.Next(10, 100)));
                    customer.Orders.Add(order);
                }
                
                customers.Add(customer);
            }
            
            return customers;
        }

        private static string SerializeToXml(List<Customer> customers)
        {
            var wrapper = new CustomerListWrapper { Customers = customers };
            var serializer = new XmlSerializer(typeof(CustomerListWrapper));
            
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, wrapper);
                return writer.ToString();
            }
        }

        private static CustomerListWrapper DeserializeFromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(CustomerListWrapper));
            
            using (var reader = new StringReader(xml))
            {
                return (CustomerListWrapper)serializer.Deserialize(reader);
            }
        }

        private static string SerializeToJson(List<Customer> customers)
        {
            var wrapper = new CustomerListWrapper { Customers = customers };
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            return JsonSerializer.Serialize(wrapper, options);
        }

        private static CustomerListWrapper DeserializeFromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            return JsonSerializer.Deserialize<CustomerListWrapper>(json, options);
        }
    }
}