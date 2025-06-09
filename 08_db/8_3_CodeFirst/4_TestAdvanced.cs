// TestAdvancedQueries.cs
using CodeFirst.Data;
using CodeFirst.Services;

public class TestAdvancedQueries
{
    public static async Task RunAsync()
    {
        using var context = new CodeFirstStoreContext();
        var queryService = new AdvancedQueryService(context);
        
        Console.WriteLine("=== Advanced Query Tests ===");
        
        // Test 1: Products above price
        Console.WriteLine("\n1. Products above $50:");
        var expensiveProducts = await queryService.GetProductsAbovePriceAsync(50m);
        foreach (var product in expensiveProducts)
        {
            Console.WriteLine($"   - {product.ProductName}: ${product.UnitPrice}");
        }
        
        // Test 2: Category statistics
        Console.WriteLine("\n2. Category Statistics:");
        var stats = await queryService.GetProductCountByCategoryAsync();
        foreach (dynamic stat in stats)
        {
            Console.WriteLine($"   - {stat.CategoryName}: {stat.ProductCount} products, avg ${stat.AveragePrice:F2}");
        }
        
        // Test 3: Search
        Console.WriteLine("\n3. Search for 'book':");
        var searchResults = await queryService.SearchProductsAsync("book");
        foreach (var product in searchResults)
        {
            Console.WriteLine($"   - {product.ProductName} ({product.Category.CategoryName})");
        }
        
        // Test 4: Pagination
        Console.WriteLine("\n4. Paginated results (page 1, size 2):");
        var (products, totalCount) = await queryService.GetProductsPagedAsync(1, 2);
        Console.WriteLine($"   Showing 2 of {totalCount} products:");
        foreach (var product in products)
        {
            Console.WriteLine($"   - {product.ProductName}");
        }
    }
}