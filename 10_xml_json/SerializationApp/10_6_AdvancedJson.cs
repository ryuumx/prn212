using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SerializationDemos
{
    // Custom converter example
    public class DateOnlyConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            return DateTime.ParseExact(dateString, "yyyy-MM-dd", null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }

    // Example with custom attributes and converters
    public class Event
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("date")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; }

        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("metadata")]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        public Event() { }

        public Event(int id, string title, DateTime date, bool isPublic = true)
        {
            Id = id;
            Title = title;
            Date = date;
            IsPublic = isPublic;
        }
    }

    public class AdvancedJsonDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Advanced JSON Features ===");
            
            DemonstrateCustomConverters();
            DemonstrateExtensionData();
            DemonstratePolymorphism();
            DemonstrateStreamingJson();
        }

        private static void DemonstrateCustomConverters()
        {
            Console.WriteLine("\n=== Custom JSON Converters ===");
            
            var eventObj = new Event(1, "Tech Conference 2024", new DateTime(2024, 6, 15), true);
            eventObj.Tags.AddRange(new[] { "technology", "conference", "networking" });
            eventObj.Metadata["location"] = "San Francisco";
            eventObj.Metadata["capacity"] = 500;
            eventObj.Metadata["price"] = 299.99;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new DateOnlyConverter() }
            };

            string json = JsonSerializer.Serialize(eventObj, options);
            Console.WriteLine("Event with custom date converter:");
            Console.WriteLine(json);

            // Deserialize back
            Event deserializedEvent = JsonSerializer.Deserialize<Event>(json, options);
            Console.WriteLine($"\nDeserialized event: {deserializedEvent.Title} on {deserializedEvent.Date:yyyy-MM-dd}");
            Console.WriteLine($"Tags: {string.Join(", ", deserializedEvent.Tags)}");
            
            File.WriteAllText("event_custom_converter.json", json);
        }

        private static void DemonstrateExtensionData()
        {
            Console.WriteLine("\n=== Extension Data Handling ===");
            
            // JSON with extra properties that don't map to our model
            string jsonWithExtras = @"{
                ""id"": 2,
                ""title"": ""Workshop: JSON in .NET"",
                ""date"": ""2024-07-20"",
                ""isPublic"": false,
                ""tags"": [""workshop"", ""json"", ""dotnet""],
                ""extraField1"": ""This will be captured in ExtensionData"",
                ""extraField2"": 42,
                ""complexExtra"": {
                    ""nested"": ""value"",
                    ""number"": 123
                }
            }";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new DateOnlyConverter() }
            };

            Event eventWithExtras = JsonSerializer.Deserialize<Event>(jsonWithExtras, options);
            
            Console.WriteLine($"Event: {eventWithExtras.Title}");
            Console.WriteLine($"Extension data captured:");
            
            if (eventWithExtras.ExtensionData != null)
            {
                foreach (var kvp in eventWithExtras.ExtensionData)
                {
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                }
            }

            // Serialize back - extension data will be preserved
            string serializedBack = JsonSerializer.Serialize(eventWithExtras, options);
            Console.WriteLine("\nSerialized back with extension data preserved:");
            Console.WriteLine(serializedBack);
        }

        private static void DemonstratePolymorphism()
        {
            Console.WriteLine("\n=== JSON Polymorphism Handling ===");
            
            // Create different types of notifications
            var notifications = new List<object>
            {
                new EmailNotification { Id = 1, Message = "Welcome!", Recipient = "user@email.com" },
                new SmsNotification { Id = 2, Message = "Code: 123456", PhoneNumber = "+1234567890" },
                new PushNotification { Id = 3, Message = "New message received", DeviceId = "device123" }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Console.WriteLine("Serializing different notification types:");
            foreach (var notification in notifications)
            {
                string json = JsonSerializer.Serialize(notification, notification.GetType(), options);
                Console.WriteLine($"\n{notification.GetType().Name}:");
                Console.WriteLine(json);
            }
        }

        private static void DemonstrateStreamingJson()
        {
            Console.WriteLine("\n=== Streaming JSON (Large Data) ===");
            
            // Simulate processing large JSON data
            string largeJsonFile = "large_customers.json";
            
            // Create large JSON file
            CreateLargeJsonFile(largeJsonFile);
            
            // Read using streaming
            using (FileStream fileStream = File.OpenRead(largeJsonFile))
            {
                JsonDocument document = JsonDocument.Parse(fileStream);
                JsonElement root = document.RootElement;
                
                if (root.TryGetProperty("customers", out JsonElement customersArray))
                {
                    Console.WriteLine($"Processing {customersArray.GetArrayLength()} customers from large file");
                    
                    int count = 0;
                    foreach (JsonElement customerElement in customersArray.EnumerateArray())
                    {
                        if (customerElement.TryGetProperty("customerName", out JsonElement nameElement))
                        {
                            count++;
                            if (count <= 3) // Show first 3
                            {
                                Console.WriteLine($"  Customer {count}: {nameElement.GetString()}");
                            }
                        }
                    }
                    
                    Console.WriteLine($"  ... and {count - 3} more customers");
                }
            }
            
            // Clean up large file
            if (File.Exists(largeJsonFile))
            {
                File.Delete(largeJsonFile);
                Console.WriteLine("Cleaned up large file");
            }
        }

        private static void CreateLargeJsonFile(string fileName)
        {
            var customers = new List<Customer>();
            var random = new Random();
            
            for (int i = 1; i <= 1000; i++)
            {
                var customer = new Customer(i, $"Customer {i}", $"customer{i}@email.com");
                
                // Add some random orders
                for (int j = 0; j < random.Next(1, 4); j++)
                {
                    var order = new Order(i * 1000 + j, random.Next(50, 1000), (OrderStatus)random.Next(0, 5));
                    customer.Orders.Add(order);
                }
                
                customers.Add(customer);
            }

            var data = new { customers };
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, json);
        }
    }

    // Supporting classes for polymorphism demo
    public abstract class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }

    public class EmailNotification : Notification
    {
        public string Recipient { get; set; }
    }

    public class SmsNotification : Notification
    {
        public string PhoneNumber { get; set; }
    }

    public class PushNotification : Notification
    {
        public string DeviceId { get; set; }
    }
}