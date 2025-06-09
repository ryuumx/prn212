// Traditional.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace  TraditionalCompare
{
    public class TraditionalCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class TraditionalCategoryService
    {
        private string connectionString = "Server=(local);Database=MyStore;Trusted_Connection=true;TrustServerCertificate=true;";
        
        public List<TraditionalCategory> GetAllCategories()
        {
            var categories = new List<TraditionalCategory>();
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT CategoryID, CategoryName, Description FROM Categories", connection);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new TraditionalCategory
                        {
                            // Use indexer with column names - CORRECT WAY
                            CategoryID = (int)reader["CategoryID"],
                            CategoryName = (string)reader["CategoryName"],
                            Description = reader["Description"] == DBNull.Value ? null : (string)reader["Description"]
                        });
                    }
                }
            }
            return categories;
        }

        public bool AddCategory(TraditionalCategory category)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description)", 
                        connection);
                    
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
                return false;
            }
        }

        public bool UpdateCategory(TraditionalCategory category)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description WHERE CategoryID = @CategoryID", 
                        connection);
                    
                    command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                return false;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("DELETE FROM Categories WHERE CategoryID = @CategoryID", connection);
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        }
    }

    // Demo program to show traditional approach complexity
    public class TraditionalDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Traditional ADO.NET Approach Demo ===");
            Console.WriteLine("Notice how much code is needed for simple operations!\n");

            var service = new TraditionalCategoryService();

            try
            {
                // Get all categories
                Console.WriteLine("1. Getting all categories:");
                var categories = service.GetAllCategories();
                foreach (var cat in categories)
                {
                    Console.WriteLine($"   ID: {cat.CategoryID}, Name: {cat.CategoryName}");
                }

                // Add a new category
                Console.WriteLine("\n2. Adding a new category:");
                var newCategory = new TraditionalCategory
                {
                    CategoryName = "Test Category",
                    Description = "This is a test category created by traditional ADO.NET"
                };

                bool added = service.AddCategory(newCategory);
                Console.WriteLine($"   Category added: {(added ? "Success" : "Failed")}");

                // Show updated list
                if (added)
                {
                    Console.WriteLine("\n3. Updated category list:");
                    categories = service.GetAllCategories();
                    foreach (var cat in categories)
                    {
                        Console.WriteLine($"   ID: {cat.CategoryID}, Name: {cat.CategoryName}");
                    }
                }

                Console.WriteLine("\n4. Problems with Traditional Approach:");
                Console.WriteLine("   - Lots of boilerplate code");
                Console.WriteLine("   - Manual connection management");
                Console.WriteLine("   - Manual parameter handling");
                Console.WriteLine("   - Manual data type conversion");
                Console.WriteLine("   - No compile-time checking of SQL");
                Console.WriteLine("   - Error-prone string concatenation");
                Console.WriteLine("   - Repetitive exception handling");
                Console.WriteLine("   - Manual null checking");
                Console.WriteLine("   - Type casting required");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Demo failed: {ex.Message}");
                Console.WriteLine("Make sure your database is running and connection string is correct.");
            }
        }
    }
}