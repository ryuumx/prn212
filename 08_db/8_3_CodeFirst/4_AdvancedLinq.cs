// File: AdvancedLinqQueries.cs
using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;
using CodeFirst.Data;

namespace CodeFirst.Services
{
    public class AdvancedQueryService
    {
        private readonly CodeFirstStoreContext _context;

        public AdvancedQueryService(CodeFirstStoreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // 1. Basic queries with filtering and sorting
        public async Task<List<Product>> GetProductsAbovePriceAsync(decimal minPrice)
        {
            return await _context.Products
                .Where(p => p.UnitPrice > minPrice && !p.Discontinued)
                .OrderBy(p => p.ProductName)
                .Include(p => p.Category)
                .ToListAsync();
        }

        // 2. Grouping and aggregation
        public async Task<List<object>> GetProductCountByCategoryAsync()
        {
            return await _context.Products
                .Where(p => !p.Discontinued)
                .GroupBy(p => p.Category.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count(),
                    AveragePrice = g.Average(p => p.UnitPrice),
                    TotalValue = g.Sum(p => p.UnitPrice * p.UnitsInStock)
                })
                .OrderByDescending(x => x.ProductCount)
                .Cast<object>()
                .ToListAsync();
        }

        // 3. Complex joins and navigation properties
        public async Task<List<object>> GetDetailedProductInfoAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.UnitsInStock > 0)
                .Select(p => new
                {
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    Price = p.UnitPrice,
                    StockValue = p.UnitPrice * p.UnitsInStock,
                    StockStatus = p.UnitsInStock <= p.ReorderLevel ? "Low Stock" : "In Stock",
                    DaysOld = EF.Functions.DateDiffDay(p.CreatedDate, DateTime.Now)
                })
                .OrderBy(p => p.CategoryName)
                .ThenBy(p => p.ProductName)
                .Cast<object>()
                .ToListAsync();
        }

        // 4. Subqueries and EXISTS operations
        public async Task<List<Category>> GetCategoriesWithExpensiveProductsAsync(decimal priceThreshold)
        {
            return await _context.Categories
                .Where(c => c.Products.Any(p => p.UnitPrice > priceThreshold && !p.Discontinued))
                .Include(c => c.Products.Where(p => p.UnitPrice > priceThreshold && !p.Discontinued))
                .ToListAsync();
        }

        // 5. Statistical queries
        public async Task<object> GetInventoryStatisticsAsync()
        {
            var stats = await _context.Products
                .Where(p => !p.Discontinued)
                .GroupBy(p => 1) // Group all products together
                .Select(g => new
                {
                    TotalProducts = g.Count(),
                    AveragePrice = g.Average(p => p.UnitPrice),
                    MinPrice = g.Min(p => p.UnitPrice),
                    MaxPrice = g.Max(p => p.UnitPrice),
                    TotalInventoryValue = g.Sum(p => p.UnitPrice * p.UnitsInStock),
                    LowStockCount = g.Count(p => p.UnitsInStock <= p.ReorderLevel)
                })
                .FirstOrDefaultAsync();

            return stats;
        }

        // 6. Date-based queries
        public async Task<List<Product>> GetProductsCreatedInLastDaysAsync(int days)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);
            
            return await _context.Products
                .Where(p => p.CreatedDate >= cutoffDate)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        // 7. Text search with multiple conditions
        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Product>();

            searchTerm = searchTerm.ToLower();

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => 
                    p.ProductName.ToLower().Contains(searchTerm) ||
                    p.Description.ToLower().Contains(searchTerm) ||
                    p.Category.CategoryName.ToLower().Contains(searchTerm))
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        // 8. Pagination
        public async Task<(List<Product> Products, int TotalCount)> GetProductsPagedAsync(
            int page, int pageSize, string sortBy = "ProductName")
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => !p.Discontinued);

            // Dynamic sorting
            query = sortBy.ToLower() switch
            {
                "name" or "productname" => query.OrderBy(p => p.ProductName),
                "price" => query.OrderBy(p => p.UnitPrice),
                "category" => query.OrderBy(p => p.Category.CategoryName),
                "stock" => query.OrderBy(p => p.UnitsInStock),
                "created" => query.OrderByDescending(p => p.CreatedDate),
                _ => query.OrderBy(p => p.ProductName)
            };

            var totalCount = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        // 9. Raw SQL queries (when LINQ is not enough)
        public async Task<List<object>> GetTopSellingCategoriesAsync()
        {
            // Example of raw SQL query
            var sql = @"
                SELECT 
                    c.CategoryName,
                    COUNT(p.ProductID) as ProductCount,
                    AVG(p.UnitPrice) as AveragePrice,
                    SUM(p.UnitsInStock * p.UnitPrice) as TotalValue
                FROM Categories c
                LEFT JOIN Products p ON c.CategoryID = p.CategoryID 
                WHERE p.Discontinued = 0 OR p.Discontinued IS NULL
                GROUP BY c.CategoryID, c.CategoryName
                ORDER BY TotalValue DESC";

            // Note: This is just an example. In real scenarios, prefer LINQ when possible
            var results = new List<object>();
            
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            await _context.Database.OpenConnectionAsync();
            
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new
                {
                    CategoryName = reader["CategoryName"].ToString() ?? "",
                    ProductCount = Convert.ToInt32(reader["ProductCount"]),
                    AveragePrice = reader["AveragePrice"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["AveragePrice"]),
                    TotalValue = reader["TotalValue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalValue"])
                });
            }

            return results;
        }

        // 10. Performance optimized queries
        public async Task<bool> HasProductsInCategoryAsync(int categoryId)
        {
            // Use Any() for existence checks - more efficient than Count() > 0
            return await _context.Products
                .AnyAsync(p => p.CategoryID == categoryId && !p.Discontinued);
        }

        public async Task<List<string>> GetProductNamesOnlyAsync()
        {
            // Select only what you need to improve performance
            return await _context.Products
                .Where(p => !p.Discontinued)
                .Select(p => p.ProductName)
                .OrderBy(name => name)
                .ToListAsync();
        }
    }

    // Query result models for complex queries
    public class ProductSummary
    {
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal StockValue { get; set; }
        public string StockStatus { get; set; } = string.Empty;
        public int DaysOld { get; set; }
    }

    public class CategoryStatistics
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class InventoryStatistics
    {
        public int TotalProducts { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int LowStockCount { get; set; }
    }
}