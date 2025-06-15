using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SerializationDemos
{
    public class Customer
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("orders")]
        public List<Order> Orders { get; set; } = new List<Order>();

        [JsonIgnore]
        public string InternalNotes { get; set; }

        public Customer() { }

        public Customer(int id, string name, string email, bool isActive = true)
        {
            CustomerId = id;
            CustomerName = name;
            Email = email;
            RegistrationDate = DateTime.Now;
            IsActive = isActive;
        }
    }

    public class Order
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("status")]
        public OrderStatus Status { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order() { }

        public Order(int orderId, decimal totalAmount, OrderStatus status = OrderStatus.Pending)
        {
            OrderId = orderId;
            OrderDate = DateTime.Now;
            TotalAmount = totalAmount;
            Status = status;
        }
    }

    public class OrderItem
    {
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal => Quantity * UnitPrice;

        public OrderItem() { }

        public OrderItem(string productName, int quantity, decimal unitPrice)
        {
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    // API Response wrapper
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiResponse() { }

        public ApiResponse(bool success, string message, T data = default(T))
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}