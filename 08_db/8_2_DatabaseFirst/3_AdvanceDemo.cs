using Microsoft.EntityFrameworkCore;
using DatabaseFirst;
using _8_2_DatabaseFirst.Models; // Adjust namespace based on your project

// Custom exception for data access
public class DataAccessException : Exception
{
    public DataAccessException(string message) : base(message) { }
    public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
}

// Repository interface for testability
public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<bool> AddCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int id);
    Task<List<Category>> SearchCategoriesAsync(string searchTerm);
}

// Repository implementation with advanced error handling
public class CategoryRepository : ICategoryRepository, IDisposable
{
    private readonly MyStoreContext _context;

    public CategoryRepository(MyStoreContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        try
        {
            return await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Failed to retrieve categories", ex);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        try
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Failed to retrieve category with ID {id}", ex);
        }
    }

    public async Task<bool> AddCategoryAsync(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        if (string.IsNullOrWhiteSpace(category.CategoryName))
            throw new ArgumentException("Category name cannot be empty");

        try
        {
            // Check for duplicate category names
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == category.CategoryName.ToLower());
            
            if (existingCategory != null)
            {
                throw new DataAccessException("A category with this name already exists");
            }

            _context.Categories.Add(category);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (DataAccessException)
        {
            throw; // Re-throw our custom exceptions
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Failed to add category", ex);
        }
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        try
        {
            var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
            if (existingCategory == null)
            {
                throw new DataAccessException("Category not found");
            }

            // Update properties
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Failed to update category", ex);
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            
            if (category == null)
            {
                return false; // Category doesn't exist
            }

            // Check if category has products
            if (category.Products.Any())
            {
                throw new DataAccessException("Cannot delete category that contains products");
            }

            _context.Categories.Remove(category);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Failed to delete category with ID {id}", ex);
        }
    }

    public async Task<List<Category>> SearchCategoriesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllCategoriesAsync();

        try
        {
            return await _context.Categories
                .Where(c => c.CategoryName.Contains(searchTerm) || 
                           c.Description.Contains(searchTerm))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Failed to search categories", ex);
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

// Service layer with business logic
public class CategoryBusinessService
{
    private readonly ICategoryRepository _repository;

    public CategoryBusinessService(ICategoryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _repository.GetAllCategoriesAsync();
    }

    public async Task<Category?> GetCategoryWithDetailsAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid category ID");

        return await _repository.GetCategoryByIdAsync(id);
    }

    public async Task<bool> CreateCategoryAsync(string name, string description)
    {
        // Business validation
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required");

        if (name.Length > 50)
            throw new ArgumentException("Category name cannot exceed 50 characters");

        var category = new Category
        {
            CategoryName = name.Trim(),
            Description = description?.Trim()
        };

        return await _repository.AddCategoryAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(int id, string name, string description)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid category ID");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required");

        var category = new Category
        {
            CategoryId = id,
            CategoryName = name.Trim(),
            Description = description?.Trim()
        };

        return await _repository.UpdateCategoryAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid category ID");

        return await _repository.DeleteCategoryAsync(id);
    }

    public async Task<List<Category>> SearchCategoriesAsync(string searchTerm)
    {
        return await _repository.SearchCategoriesAsync(searchTerm?.Trim());
    }
}

// Enhanced console application
public class EnhancedProgram
{
    private static CategoryBusinessService _categoryService;

    public static async Task Main(string[] args)
    {
        // Setup dependency injection manually (in real apps, use DI container)
        using var context = new MyStoreContext();
        var repository = new CategoryRepository(context);
        _categoryService = new CategoryBusinessService(repository);

        Console.WriteLine("Enhanced Category Management System");
        Console.WriteLine("Database First Approach with EF Core");

        await RunApplicationAsync();
    }

    private static async Task RunApplicationAsync()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\n=== Enhanced Category Management ===");
                Console.WriteLine("1. List all categories");
                Console.WriteLine("2. Search categories");
                Console.WriteLine("3. View category details");
                Console.WriteLine("4. Add new category");
                Console.WriteLine("5. Update category");
                Console.WriteLine("6. Delete category");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ListAllCategoriesAsync();
                        break;
                    case "2":
                        await SearchCategoriesAsync();
                        break;
                    case "3":
                        await ViewCategoryDetailsAsync();
                        break;
                    case "4":
                        await AddNewCategoryAsync();
                        break;
                    case "5":
                        await UpdateCategoryAsync();
                        break;
                    case "6":
                        await DeleteCategoryAsync();
                        break;
                    case "0":
                        Console.WriteLine("Thank you for using Category Management System!");
                        return;
                    default:
                        Console.WriteLine("Invalid option! Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Please try again or contact support if the problem persists.");
            }
        }
    }

    private static async Task ListAllCategoriesAsync()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            Console.WriteLine($"\n=== Found {categories.Count} Categories ===");
            
            if (categories.Any())
            {
                foreach (var cat in categories)
                {
                    Console.WriteLine($"ID: {cat.CategoryId,-3} | Name: {cat.CategoryName,-20} | Description: {cat.Description}");
                }
            }
            else
            {
                Console.WriteLine("No categories found.");
            }
        }
        catch (DataAccessException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }

    private static async Task SearchCategoriesAsync()
    {
        Console.Write("Enter search term: ");
        string searchTerm = Console.ReadLine();

        try
        {
            var categories = await _categoryService.SearchCategoriesAsync(searchTerm);
            Console.WriteLine($"\n=== Search Results ({categories.Count} found) ===");
            
            foreach (var cat in categories)
            {
                Console.WriteLine($"ID: {cat.CategoryId,-3} | Name: {cat.CategoryName,-20} | Description: {cat.Description}");
            }
        }
        catch (DataAccessException ex)
        {
            Console.WriteLine($"Search failed: {ex.Message}");
        }
    }

    private static async Task ViewCategoryDetailsAsync()
    {
        Console.Write("Enter category ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                var category = await _categoryService.GetCategoryWithDetailsAsync(id);
                if (category != null)
                {
                    Console.WriteLine($"\n=== Category Details ===");
                    Console.WriteLine($"ID: {category.CategoryId}");
                    Console.WriteLine($"Name: {category.CategoryName}");
                    Console.WriteLine($"Description: {category.Description ?? "No description"}");
                    Console.WriteLine($"Number of Products: {category.Products?.Count ?? 0}");
                    
                    if (category.Products?.Any() == true)
                    {
                        Console.WriteLine("\nProducts in this category:");
                        foreach (var product in category.Products.Take(5)) // Show first 5 products
                        {
                            Console.WriteLine($"  - {product.ProductName} (${product.UnitPrice:C})");
                        }
                        if (category.Products.Count > 5)
                        {
                            Console.WriteLine($"  ... and {category.Products.Count - 5} more products");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Category not found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving category: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format!");
        }
    }

    private static async Task AddNewCategoryAsync()
    {
        Console.Write("Enter category name: ");
        string name = Console.ReadLine();
        
        Console.Write("Enter description (optional): ");
        string description = Console.ReadLine();

        try
        {
            bool success = await _categoryService.CreateCategoryAsync(name, description);
            if (success)
            {
                Console.WriteLine("✓ Category added successfully!");
            }
            else
            {
                Console.WriteLine("✗ Failed to add category!");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (DataAccessException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
    }

    private static async Task UpdateCategoryAsync()
    {
        Console.Write("Enter category ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                // First, show current category
                var currentCategory = await _categoryService.GetCategoryWithDetailsAsync(id);
                if (currentCategory == null)
                {
                    Console.WriteLine("Category not found!");
                    return;
                }

                Console.WriteLine($"\nCurrent category: {currentCategory.CategoryName}");
                Console.WriteLine($"Current description: {currentCategory.Description ?? "None"}");

                Console.Write("Enter new name (or press Enter to keep current): ");
                string newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    newName = currentCategory.CategoryName;
                }

                Console.Write("Enter new description (or press Enter to keep current): ");
                string newDescription = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newDescription))
                {
                    newDescription = currentCategory.Description;
                }

                bool success = await _categoryService.UpdateCategoryAsync(id, newName, newDescription);
                if (success)
                {
                    Console.WriteLine("✓ Category updated successfully!");
                }
                else
                {
                    Console.WriteLine("✗ Failed to update category!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format!");
        }
    }

    private static async Task DeleteCategoryAsync()
    {
        Console.Write("Enter category ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                // Show category details before deletion
                var category = await _categoryService.GetCategoryWithDetailsAsync(id);
                if (category == null)
                {
                    Console.WriteLine("Category not found!");
                    return;
                }

                Console.WriteLine($"\nCategory to delete: {category.CategoryName}");
                Console.WriteLine($"Description: {category.Description ?? "None"}");
                Console.WriteLine($"Products in category: {category.Products?.Count ?? 0}");

                Console.Write("Are you sure you want to delete this category? (y/N): ");
                string confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y" || confirmation?.ToLower() == "yes")
                {
                    bool success = await _categoryService.DeleteCategoryAsync(id);
                    if (success)
                    {
                        Console.WriteLine("✓ Category deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("✗ Failed to delete category!");
                    }
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Cannot delete category: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format!");
        }
    }
}