using Microsoft.EntityFrameworkCore;
using CodeFirst.Data;
using CodeFirst.Models;

public class TestCodeFirst
{
    public static async Task RunAsync()
    {
        using var context = new CodeFirstStoreContext();
        
        Console.WriteLine("=== Code First Demo ===");
        
        // Test seeded data
        var categories = await context.Categories.ToListAsync();
        Console.WriteLine($"Categories found: {categories.Count}");
        
        foreach (var cat in categories)
        {
            Console.WriteLine($"- {cat.CategoryName}: {cat.Description}");
        }
        
        var products = await context.Products.Include(p => p.Category).ToListAsync();
        Console.WriteLine($"\nProducts found: {products.Count}");
        
        foreach (var product in products)
        {
            Console.WriteLine($"- {product.ProductName} ({product.Category.CategoryName}): ${product.UnitPrice}");
        }
    }
}