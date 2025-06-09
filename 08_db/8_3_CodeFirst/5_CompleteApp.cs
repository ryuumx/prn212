using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;
using CodeFirst.Data;
using CodeFirst.Services;

namespace CodeFirst
{
    public class CompleteApplication
    {
        private static CodeFirstStoreContext _context = null!;
        private static AdvancedQueryService _queryService = null!;
        private static DatabaseMigrationService _migrationService = null!;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== Code First EF Core Demo Application ===");
            Console.WriteLine("Advanced Entity Framework Core with LINQ");

            // Initialize services
            await InitializeServicesAsync();

            // Check database status
            await CheckDatabaseStatusAsync();

            // Run main application loop
            await RunApplicationAsync();

            // Cleanup
            await _context.DisposeAsync();
        }

        private static async Task InitializeServicesAsync()
        {
            _context = new CodeFirstStoreContext();
            _queryService = new AdvancedQueryService(_context);
            _migrationService = new DatabaseMigrationService(_context);

            Console.WriteLine("✓ Services initialized");
        }

        private static async Task CheckDatabaseStatusAsync()
        {
            Console.WriteLine("\n=== Database Status Check ===");

            try
            {
                bool canConnect = await _migrationService.CanConnectAsync();
                if (!canConnect)
                {
                    Console.WriteLine("Cannot connect to database. Attempting to create/migrate...");
                    await _migrationService.MigrateDatabaseAsync();
                }

                await _migrationService.DisplayDatabaseInfoAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                Console.WriteLine("Please check your connection string and database server.");
                return;
            }
        }

        private static async Task RunApplicationAsync()
        {
            while (true)
            {
                try
                {
                    DisplayMainMenu();
                    string choice = Console.ReadLine() ?? "";

                    switch (choice)
                    {
                        case "1":
                            await ShowAllProductsAsync();
                            break;
                        case "2":
                            await SearchProductsAsync();
                            break;
                        case "3":
                            await ShowProductsByPriceRangeAsync();
                            break;
                        case "4":
                            await ShowCategoryStatisticsAsync();
                            break;
                        case "5":
                            await ShowInventoryStatisticsAsync();
                            break;
                        case "6":
                            await ShowRecentProductsAsync();
                            break;
                        case "7":
                            await ShowProductsPaginatedAsync();
                            break;
                        case "8":
                            await ManageDataAsync();
                            break;
                        case "9":
                            await ShowAdvancedQueriesMenuAsync();
                            break;
                        case "0":
                            Console.WriteLine("Thank you for using the Code First Demo!");
                            return;
                        default:
                            Console.WriteLine("Invalid option! Please try again.");
                            break;
                    }

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Please try again or contact support.");
                }
            }
        }

        private static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Code First EF Core Demo - Main Menu ===");
            Console.WriteLine("1. 📋 Show all products");
            Console.WriteLine("2. 🔍 Search products");
            Console.WriteLine("3. 💰 Products by price range");
            Console.WriteLine("4. 📊 Category statistics");
            Console.WriteLine("5. 📈 Inventory statistics");
            Console.WriteLine("6. 🆕 Recent products");
            Console.WriteLine("7. 📄 Products (paginated)");
            Console.WriteLine("8. ⚙️  Manage data (CRUD)");
            Console.WriteLine("9. 🔬 Advanced queries");
Console.WriteLine("0. 🚪 Exit");
           Console.WriteLine();
           Console.Write("Choose an option: ");
       }

       private static async Task ShowAllProductsAsync()
       {
           Console.Clear();
           Console.WriteLine("=== All Products ===");

           var products = await _context.Products
               .Include(p => p.Category)
               .OrderBy(p => p.Category.CategoryName)
               .ThenBy(p => p.ProductName)
               .ToListAsync();

           if (products.Any())
           {
               Console.WriteLine($"{"ID",-5} {"Product Name",-25} {"Category",-15} {"Price",-10} {"Stock",-8} {"Status",-12}");
               Console.WriteLine(new string('-', 80));

               foreach (var product in products)
               {
                   var status = product.Discontinued ? "Discontinued" : 
                                product.UnitsInStock <= product.ReorderLevel ? "Low Stock" : "In Stock";
                   
                   Console.WriteLine($"{product.ProductID,-5} {product.ProductName,-25} " +
                                   $"{product.Category.CategoryName,-15} ${product.UnitPrice,-9:F2} " +
                                   $"{product.UnitsInStock,-8} {status,-12}");
               }
               Console.WriteLine($"\nTotal products: {products.Count}");
           }
           else
           {
               Console.WriteLine("No products found.");
           }
       }

       private static async Task SearchProductsAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Search Products ===");
           Console.Write("Enter search term (name, description, or category): ");
           string searchTerm = Console.ReadLine() ?? "";

           if (string.IsNullOrWhiteSpace(searchTerm))
           {
               Console.WriteLine("Please enter a search term.");
               return;
           }

           var products = await _queryService.SearchProductsAsync(searchTerm);

           Console.WriteLine($"\n=== Search Results for '{searchTerm}' ===");
           if (products.Any())
           {
               foreach (var product in products)
               {
                   Console.WriteLine($"📦 {product.ProductName}");
                   Console.WriteLine($"   Category: {product.Category.CategoryName}");
                   Console.WriteLine($"   Price: ${product.UnitPrice:F2}");
                   Console.WriteLine($"   Stock: {product.UnitsInStock} units");
                   if (!string.IsNullOrEmpty(product.Description))
                   {
                       Console.WriteLine($"   Description: {product.Description}");
                   }
                   Console.WriteLine();
               }
               Console.WriteLine($"Found {products.Count} products matching '{searchTerm}'");
           }
           else
           {
               Console.WriteLine("No products found matching your search.");
           }
       }

       private static async Task ShowProductsByPriceRangeAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Products by Price Range ===");
           Console.Write("Enter minimum price: $");
           
           if (decimal.TryParse(Console.ReadLine(), out decimal minPrice))
           {
               var products = await _queryService.GetProductsAbovePriceAsync(minPrice);

               if (products.Any())
               {
                   Console.WriteLine($"\n=== Products above ${minPrice:F2} ===");
                   foreach (var product in products)
                   {
                       Console.WriteLine($"💰 {product.ProductName} - ${product.UnitPrice:F2}");
                       Console.WriteLine($"   Category: {product.Category.CategoryName}");
                       Console.WriteLine($"   Stock: {product.UnitsInStock} units");
                       Console.WriteLine();
                   }
                   Console.WriteLine($"Found {products.Count} products above ${minPrice:F2}");
               }
               else
               {
                   Console.WriteLine($"No products found above ${minPrice:F2}");
               }
           }
           else
           {
               Console.WriteLine("Invalid price format!");
           }
       }

       private static async Task ShowCategoryStatisticsAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Category Statistics ===");

           var stats = await _queryService.GetProductCountByCategoryAsync();

           if (stats.Any())
           {
               Console.WriteLine($"{"Category",-20} {"Products",-10} {"Avg Price",-12} {"Total Value",-15}");
               Console.WriteLine(new string('-', 60));

               foreach (dynamic stat in stats)
               {
                   Console.WriteLine($"{stat.CategoryName,-20} {stat.ProductCount,-10} " +
                                   $"${stat.AveragePrice,-11:F2} ${stat.TotalValue,-14:F2}");
               }
           }
           else
           {
               Console.WriteLine("No category data available.");
           }
       }

       private static async Task ShowInventoryStatisticsAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Inventory Statistics ===");

           var stats = await _queryService.GetInventoryStatisticsAsync();

           if (stats != null)
           {
               dynamic s = stats;
               Console.WriteLine($"📊 Total Products: {s.TotalProducts}");
               Console.WriteLine($"💰 Average Price: ${s.AveragePrice:F2}");
               Console.WriteLine($"📉 Minimum Price: ${s.MinPrice:F2}");
               Console.WriteLine($"📈 Maximum Price: ${s.MaxPrice:F2}");
               Console.WriteLine($"💎 Total Inventory Value: ${s.TotalInventoryValue:F2}");
               Console.WriteLine($"⚠️  Low Stock Items: {s.LowStockCount}");
           }
           else
           {
               Console.WriteLine("No inventory data available.");
           }
       }

       private static async Task ShowRecentProductsAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Recent Products ===");
           Console.Write("Enter number of days (default 30): ");
           
           string input = Console.ReadLine() ?? "";
           int days = int.TryParse(input, out int d) ? d : 30;

           var products = await _queryService.GetProductsCreatedInLastDaysAsync(days);

           if (products.Any())
           {
               Console.WriteLine($"\n=== Products created in the last {days} days ===");
               foreach (var product in products)
               {
                   var daysAgo = (DateTime.Now - product.CreatedDate).Days;
                   Console.WriteLine($"🆕 {product.ProductName}");
                   Console.WriteLine($"   Category: {product.Category.CategoryName}");
                   Console.WriteLine($"   Created: {product.CreatedDate:yyyy-MM-dd} ({daysAgo} days ago)");
                   Console.WriteLine($"   Price: ${product.UnitPrice:F2}");
                   Console.WriteLine();
               }
               Console.WriteLine($"Found {products.Count} recent products");
           }
           else
           {
               Console.WriteLine($"No products created in the last {days} days.");
           }
       }

       private static async Task ShowProductsPaginatedAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Products (Paginated) ===");
           
           int page = 1;
           int pageSize = 5;
           string sortBy = "ProductName";

           while (true)
           {
               var (products, totalCount) = await _queryService.GetProductsPagedAsync(page, pageSize, sortBy);
               int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

               Console.Clear();
               Console.WriteLine($"=== Products (Page {page} of {totalPages}) ===");
               Console.WriteLine($"Sort by: {sortBy} | Total products: {totalCount}");
               Console.WriteLine();

               if (products.Any())
               {
                   foreach (var product in products)
                   {
                       Console.WriteLine($"📦 {product.ProductName}");
                       Console.WriteLine($"   Category: {product.Category.CategoryName}");
                       Console.WriteLine($"   Price: ${product.UnitPrice:F2} | Stock: {product.UnitsInStock}");
                       Console.WriteLine();
                   }
               }
               else
               {
                   Console.WriteLine("No products found.");
               }

               Console.WriteLine($"Page {page} of {totalPages}");
               Console.WriteLine("Commands: [N]ext, [P]revious, [S]ort, [Q]uit");
               Console.Write("Enter command: ");

               string command = Console.ReadLine()?.ToUpper() ?? "";
               
               switch (command)
               {
                   case "N":
                       if (page < totalPages) page++;
                       break;
                   case "P":
                       if (page > 1) page--;
                       break;
                   case "S":
                       Console.WriteLine("Sort options: name, price, category, stock, created");
                       Console.Write("Enter sort field: ");
                       string newSort = Console.ReadLine() ?? "";
                       if (!string.IsNullOrWhiteSpace(newSort))
                           sortBy = newSort;
                       break;
                   case "Q":
                       return;
               }
           }
       }

       private static async Task ManageDataAsync()
       {
           while (true)
           {
               Console.Clear();
               Console.WriteLine("=== Data Management (CRUD Operations) ===");
               Console.WriteLine("1. ➕ Add new product");
               Console.WriteLine("2. ✏️  Update product");
               Console.WriteLine("3. ❌ Delete product");
               Console.WriteLine("4. 📁 Manage categories");
               Console.WriteLine("0. ⬅️  Back to main menu");
               Console.WriteLine();
               Console.Write("Choose an option: ");

               string choice = Console.ReadLine() ?? "";

               switch (choice)
               {
                   case "1":
                       await AddNewProductAsync();
                       break;
                   case "2":
                       await UpdateProductAsync();
                       break;
                   case "3":
                       await DeleteProductAsync();
                       break;
                   case "4":
                       await ManageCategoriesAsync();
                       break;
                   case "0":
                       return;
                   default:
                       Console.WriteLine("Invalid option!");
                       Console.ReadKey();
                       break;
               }
           }
       }

       private static async Task AddNewProductAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Add New Product ===");

           try
           {
               // Show available categories
               var categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
               Console.WriteLine("Available categories:");
               foreach (var cat in categories)
               {
                   Console.WriteLine($"  {cat.CategoryID}. {cat.CategoryName}");
               }

               Console.WriteLine();
               Console.Write("Product name: ");
               string name = Console.ReadLine() ?? "";

               Console.Write("Description: ");
               string description = Console.ReadLine() ?? "";

               Console.Write("Price: $");
               if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
               {
                   Console.WriteLine("Invalid price!");
                   return;
               }

               Console.Write("Units in stock: ");
               if (!short.TryParse(Console.ReadLine(), out short stock) || stock < 0)
               {
                   Console.WriteLine("Invalid stock quantity!");
                   return;
               }

               Console.Write("Reorder level: ");
               if (!short.TryParse(Console.ReadLine(), out short reorderLevel) || reorderLevel < 0)
               {
                   Console.WriteLine("Invalid reorder level!");
                   return;
               }

               Console.Write("Category ID: ");
               if (!int.TryParse(Console.ReadLine(), out int categoryId))
               {
                   Console.WriteLine("Invalid category ID!");
                   return;
               }

               // Validate category exists
               var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryID == categoryId && c.IsActive);
               if (!categoryExists)
               {
                   Console.WriteLine("Category not found or inactive!");
                   return;
               }

               var product = new Product
               {
                   ProductName = name,
                   Description = string.IsNullOrWhiteSpace(description) ? null : description,
                   UnitPrice = price,
                   UnitsInStock = stock,
                   ReorderLevel = reorderLevel,
                   CategoryID = categoryId,
                   CreatedDate = DateTime.Now,
                   Discontinued = false
               };

               _context.Products.Add(product);
               await _context.SaveChangesAsync();

               Console.WriteLine("✓ Product added successfully!");
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error adding product: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task UpdateProductAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Update Product ===");

           Console.Write("Enter product ID to update: ");
           if (!int.TryParse(Console.ReadLine(), out int productId))
           {
               Console.WriteLine("Invalid product ID!");
               Console.ReadKey();
               return;
           }

           try
           {
               var product = await _context.Products
                   .Include(p => p.Category)
                   .FirstOrDefaultAsync(p => p.ProductID == productId);

               if (product == null)
               {
                   Console.WriteLine("Product not found!");
                   Console.ReadKey();
                   return;
               }

               Console.WriteLine($"\nCurrent product: {product.ProductName}");
               Console.WriteLine($"Category: {product.Category.CategoryName}");
               Console.WriteLine($"Price: ${product.UnitPrice:F2}");
               Console.WriteLine($"Stock: {product.UnitsInStock}");
               Console.WriteLine();

               Console.Write($"New name (current: {product.ProductName}): ");
               string newName = Console.ReadLine() ?? "";
               if (!string.IsNullOrWhiteSpace(newName))
                   product.ProductName = newName;

               Console.Write($"New price (current: ${product.UnitPrice:F2}): $");
               string priceInput = Console.ReadLine() ?? "";
               if (decimal.TryParse(priceInput, out decimal newPrice) && newPrice >= 0)
                   product.UnitPrice = newPrice;

               Console.Write($"New stock (current: {product.UnitsInStock}): ");
               string stockInput = Console.ReadLine() ?? "";
               if (short.TryParse(stockInput, out short newStock) && newStock >= 0)
                   product.UnitsInStock = newStock;

               await _context.SaveChangesAsync();
               Console.WriteLine("✓ Product updated successfully!");
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error updating product: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task DeleteProductAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Delete Product ===");

           Console.Write("Enter product ID to delete: ");
           if (!int.TryParse(Console.ReadLine(), out int productId))
           {
               Console.WriteLine("Invalid product ID!");
               Console.ReadKey();
               return;
           }

           try
           {
               var product = await _context.Products
                   .Include(p => p.Category)
                   .FirstOrDefaultAsync(p => p.ProductID == productId);

               if (product == null)
               {
                   Console.WriteLine("Product not found!");
                   Console.ReadKey();
                   return;
               }

               Console.WriteLine($"\nProduct to delete:");
               Console.WriteLine($"Name: {product.ProductName}");
               Console.WriteLine($"Category: {product.Category.CategoryName}");
               Console.WriteLine($"Price: ${product.UnitPrice:F2}");

               Console.Write("\nAre you sure you want to delete this product? (y/N): ");
               string confirmation = Console.ReadLine() ?? "";

               if (confirmation.ToLower() == "y" || confirmation.ToLower() == "yes")
               {
                   _context.Products.Remove(product);
                   await _context.SaveChangesAsync();
                   Console.WriteLine("✓ Product deleted successfully!");
               }
               else
               {
                   Console.WriteLine("Deletion cancelled.");
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error deleting product: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task ManageCategoriesAsync()
       {
           while (true)
           {
               Console.Clear();
               Console.WriteLine("=== Category Management ===");

               var categories = await _context.Categories
                   .Include(c => c.Products)
                   .OrderBy(c => c.CategoryName)
                   .ToListAsync();

               Console.WriteLine("Current categories:");
               foreach (var cat in categories)
               {
                   var productCount = cat.Products.Count(p => !p.Discontinued);
                   var status = cat.IsActive ? "Active" : "Inactive";
                   Console.WriteLine($"  {cat.CategoryID}. {cat.CategoryName} ({productCount} products) - {status}");
               }

               Console.WriteLine();
               Console.WriteLine("1. ➕ Add category");
               Console.WriteLine("2. ✏️  Update category");
               Console.WriteLine("3. 🔄 Toggle category status");
               Console.WriteLine("0. ⬅️  Back");
               Console.WriteLine();
               Console.Write("Choose an option: ");

               string choice = Console.ReadLine() ?? "";

               switch (choice)
               {
                   case "1":
                       await AddCategoryAsync();
                       break;
                   case "2":
                       await UpdateCategoryAsync();
                       break;
                   case "3":
                       await ToggleCategoryStatusAsync();
                       break;
                   case "0":
                       return;
                   default:
                       Console.WriteLine("Invalid option!");
                       Console.ReadKey();
                       break;
               }
           }
       }

       private static async Task AddCategoryAsync()
       {
           Console.Write("Category name: ");
           string name = Console.ReadLine() ?? "";
           
           if (string.IsNullOrWhiteSpace(name))
           {
               Console.WriteLine("Category name is required!");
               Console.ReadKey();
               return;
           }

           Console.Write("Description: ");
           string description = Console.ReadLine() ?? "";

           try
           {
               var category = new Category
               {
                   CategoryName = name,
                   Description = string.IsNullOrWhiteSpace(description) ? null : description,
                   IsActive = true,
                   CreatedDate = DateTime.Now
               };

               _context.Categories.Add(category);
               await _context.SaveChangesAsync();

               Console.WriteLine("✓ Category added successfully!");
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error adding category: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task UpdateCategoryAsync()
       {
           Console.Write("Enter category ID to update: ");
           if (!int.TryParse(Console.ReadLine(), out int categoryId))
           {
               Console.WriteLine("Invalid category ID!");
               Console.ReadKey();
               return;
           }

           try
           {
               var category = await _context.Categories.FindAsync(categoryId);
               if (category == null)
               {
                   Console.WriteLine("Category not found!");
                   Console.ReadKey();
                   return;
               }

               Console.WriteLine($"\nCurrent category: {category.CategoryName}");
               Console.Write("New name (press Enter to keep current): ");
               string newName = Console.ReadLine() ?? "";
               if (!string.IsNullOrWhiteSpace(newName))
                   category.CategoryName = newName;

               Console.Write("New description (press Enter to keep current): ");
               string newDescription = Console.ReadLine() ?? "";
               if (!string.IsNullOrWhiteSpace(newDescription))
                   category.Description = newDescription;

               await _context.SaveChangesAsync();
               Console.WriteLine("✓ Category updated successfully!");
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error updating category: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task ToggleCategoryStatusAsync()
       {
           Console.Write("Enter category ID to toggle status: ");
           if (!int.TryParse(Console.ReadLine(), out int categoryId))
           {
               Console.WriteLine("Invalid category ID!");
               Console.ReadKey();
               return;
           }

           try
           {
               var category = await _context.Categories
                   .Include(c => c.Products)
                   .FirstOrDefaultAsync(c => c.CategoryID == categoryId);

               if (category == null)
               {
                   Console.WriteLine("Category not found!");
                   Console.ReadKey();
                   return;
               }

               string currentStatus = category.IsActive ? "Active" : "Inactive";
               string newStatus = category.IsActive ? "Inactive" : "Active";

               Console.WriteLine($"\nCategory: {category.CategoryName}");
               Console.WriteLine($"Current status: {currentStatus}");
               Console.WriteLine($"Products in category: {category.Products.Count}");

               Console.Write($"Change status to {newStatus}? (y/N): ");
               string confirmation = Console.ReadLine() ?? "";

               if (confirmation.ToLower() == "y" || confirmation.ToLower() == "yes")
               {
                   category.IsActive = !category.IsActive;
                   await _context.SaveChangesAsync();
                   Console.WriteLine($"✓ Category status changed to {newStatus}!");
               }
               else
               {
                   Console.WriteLine("Status change cancelled.");
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error toggling category status: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task ShowAdvancedQueriesMenuAsync()
       {
           while (true)
           {
               Console.Clear();
               Console.WriteLine("=== Advanced Query Demonstrations ===");
               Console.WriteLine("1. 📊 Categories with expensive products");
               Console.WriteLine("2. 🔝 Top selling categories (raw SQL demo)");
               Console.WriteLine("3. 📈 Detailed product information");
               Console.WriteLine("4. ⚡ Performance optimized queries");
               Console.WriteLine("0. ⬅️  Back to main menu");
               Console.WriteLine();
               Console.Write("Choose an option: ");

               string choice = Console.ReadLine() ?? "";

               switch (choice)
               {
                   case "1":
                       await ShowCategoriesWithExpensiveProductsAsync();
                       break;
                   case "2":
                       await ShowTopSellingCategoriesAsync();
                       break;
                   case "3":
                       await ShowDetailedProductInfoAsync();
                       break;
                   case "4":
                       await ShowPerformanceOptimizedQueriesAsync();
                       break;
                   case "0":
                       return;
                   default:
                       Console.WriteLine("Invalid option!");
                       Console.ReadKey();
                       break;
               }
           }
       }

       private static async Task ShowCategoriesWithExpensiveProductsAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Categories with Expensive Products ===");
           Console.Write("Enter minimum price threshold: $");

           if (decimal.TryParse(Console.ReadLine(), out decimal threshold))
           {
               var categories = await _queryService.GetCategoriesWithExpensiveProductsAsync(threshold);

               if (categories.Any())
               {
                   Console.WriteLine($"\n=== Categories with products above ${threshold:F2} ===");
                   foreach (var category in categories)
                   {
                       Console.WriteLine($"\n📁 {category.CategoryName}");
                       Console.WriteLine($"   Description: {category.Description ?? "No description"}");
                       Console.WriteLine($"   Expensive products in this category:");
                       
                       foreach (var product in category.Products)
                       {
                           Console.WriteLine($"     💰 {product.ProductName} - ${product.UnitPrice:F2}");
                       }
                   }
               }
               else
               {
                   Console.WriteLine($"No categories found with products above ${threshold:F2}");
               }
           }
           else
           {
               Console.WriteLine("Invalid price format!");
           }

           Console.ReadKey();
       }

       private static async Task ShowTopSellingCategoriesAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Top Selling Categories (Raw SQL Demo) ===");

           try
           {
               var results = await _queryService.GetTopSellingCategoriesAsync();

               if (results.Any())
               {
                   Console.WriteLine($"{"Category",-20} {"Products",-10} {"Avg Price",-12} {"Total Value",-15}");
                   Console.WriteLine(new string('-', 60));

                   foreach (dynamic result in results)
                   {
                       Console.WriteLine($"{result.CategoryName,-20} {result.ProductCount,-10} " +
                                       $"${result.AveragePrice,-11:F2} ${result.TotalValue,-14:F2}");
                   }
               }
               else
               {
                   Console.WriteLine("No data available.");
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error executing raw SQL query: {ex.Message}");
           }

           Console.ReadKey();
       }

       private static async Task ShowDetailedProductInfoAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Detailed Product Information ===");

           var products = await _queryService.GetDetailedProductInfoAsync();

           if (products.Any())
           {
               foreach (dynamic product in products)
               {
                   Console.WriteLine($"📦 {product.ProductName}");
                   Console.WriteLine($"   Category: {product.CategoryName}");
                   Console.WriteLine($"   Price: ${product.Price:F2}");
                   Console.WriteLine($"   Stock Value: ${product.StockValue:F2}");
                   Console.WriteLine($"   Status: {product.StockStatus}");
                   Console.WriteLine($"   Age: {product.DaysOld} days");
                   Console.WriteLine();
               }
               Console.WriteLine($"Total products displayed: {products.Count}");
           }
           else
           {
               Console.WriteLine("No product data available.");
           }

           Console.ReadKey();
       }

       private static async Task ShowPerformanceOptimizedQueriesAsync()
       {
           Console.Clear();
           Console.WriteLine("=== Performance Optimized Queries Demo ===");

           try
           {
               // Demonstrate existence check
               Console.WriteLine("Testing category existence checks...");
               var sw = System.Diagnostics.Stopwatch.StartNew();
               
               for (int i = 1; i <= 5; i++)
               {
                   bool hasProducts = await _queryService.HasProductsInCategoryAsync(i);
                   Console.WriteLine($"Category {i} has products: {hasProducts}");
               }
               
               sw.Stop();
               Console.WriteLine($"Existence checks completed in: {sw.ElapsedMilliseconds}ms");

               Console.WriteLine();

               // Demonstrate projection query
               Console.WriteLine("Getting product names only (projection)...");
               sw.Restart();
               
               var productNames = await _queryService.GetProductNamesOnlyAsync();
               
               sw.Stop();
               Console.WriteLine($"Retrieved {productNames.Count} product names in: {sw.ElapsedMilliseconds}ms");
               
               Console.WriteLine("\nFirst 10 product names:");
               foreach (var name in productNames.Take(10))
               {
                   Console.WriteLine($"  • {name}");
               }

               if (productNames.Count > 10)
               {
                   Console.WriteLine($"  ... and {productNames.Count - 10} more");
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Error in performance demo: {ex.Message}");
           }

           Console.ReadKey();
       }
   }
}