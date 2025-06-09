using Microsoft.EntityFrameworkCore;
using _8_2_DatabaseFirst.Models; // Adjust namespace based on your project

namespace DatabaseFirst
{
    public class SimpleDemo
    {
        public static async Task RunSimpleDemo()
        {
            using var context = new MyStoreContext();

            Console.WriteLine("=== All Categories ===");
            var categories = await context.Categories.ToListAsync();

            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.CategoryId}, Name: {category.CategoryName}");
            }

            Console.WriteLine("\n=== All Products with Categories ===");
            var products = await context.Products
                .Include(p => p.Category)
                .ToListAsync();

            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductName} - {product.Category?.CategoryName} - ${product.UnitPrice}");
            }
        }
    }
}