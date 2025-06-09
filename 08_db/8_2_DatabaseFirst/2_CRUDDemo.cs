using Microsoft.EntityFrameworkCore;
using _8_2_DatabaseFirst.Models; // Adjust namespace based on your project

namespace DatabaseFirst {
    public class CategoryService
    {
        private readonly MyStoreContext _context;

        public CategoryService()
        {
            _context = new MyStoreContext();
        }

        // READ operations
        public List<Category> GetAllCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting categories: {ex.Message}");
                return new List<Category>();
            }
        }

        public Category? GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        // Demonstrate Include for navigation properties
        public List<Category> GetCategoriesWithProducts()
        {
            return _context.Categories
                .Include(c => c.Products)
                .ToList();
        }

        // CREATE operation
        public bool AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                int result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
                return false;
            }
        }

        // UPDATE operation
        public bool UpdateCategory(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                int result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                return false;
            }
        }

        // DELETE operation
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                var category = _context.Categories.Find(categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    int result = _context.SaveChanges();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    // Console application to test our service
    public class DatabaseFirstCRUDDemo
    {
        public static async Task RunCRUDDemo()
        {
            var categoryService = new CategoryService();

            while (true)
            {
                Console.WriteLine("\n=== Category Management System ===");
                Console.WriteLine("1. List all categories");
                Console.WriteLine("2. Add new category");
                Console.WriteLine("3. Update category");
                Console.WriteLine("4. Delete category");
                Console.WriteLine("5. Show categories with products");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListCategories(categoryService);
                        break;
                    case "2":
                        AddNewCategory(categoryService);
                        break;
                    case "3":
                        UpdateCategory(categoryService);
                        break;
                    case "4":
                        DeleteCategory(categoryService);
                        break;
                    case "5":
                        ShowCategoriesWithProducts(categoryService);
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        static void ListCategories(CategoryService service)
        {
            var categories = service.GetAllCategories();
            Console.WriteLine("\n=== All Categories ===");
            foreach (var cat in categories)
            {
                Console.WriteLine($"ID: {cat.CategoryId}, Name: {cat.CategoryName}, Description: {cat.Description}");
            }
        }

        static void AddNewCategory(CategoryService service)
        {
            Console.Write("Enter category name: ");
            string name = Console.ReadLine();
            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            var newCategory = new Category
            {
                CategoryName = name,
                Description = description
            };

            if (service.AddCategory(newCategory))
            {
                Console.WriteLine("Category added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add category!");
            }
        }

        static void UpdateCategory(CategoryService service)
        {
            Console.Write("Enter category ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var category = service.GetCategoryById(id);
                if (category != null)
                {
                    Console.WriteLine($"Current name: {category.CategoryName}");
                    Console.Write("Enter new name (or press Enter to keep current): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        category.CategoryName = newName;
                    }

                    Console.WriteLine($"Current description: {category.Description}");
                    Console.Write("Enter new description (or press Enter to keep current): ");
                    string newDesc = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newDesc))
                    {
                        category.Description = newDesc;
                    }

                    if (service.UpdateCategory(category))
                    {
                        Console.WriteLine("Category updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update category!");
                    }
                }
                else
                {
                    Console.WriteLine("Category not found!");
                }
            }
        }

        static void DeleteCategory(CategoryService service)
        {
            Console.Write("Enter category ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (service.DeleteCategory(id))
                {
                    Console.WriteLine("Category deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to delete category or category not found!");
                }
            }
        }

        static void ShowCategoriesWithProducts(CategoryService service)
        {
            var categories = service.GetCategoriesWithProducts();
            Console.WriteLine("\n=== Categories with Products ===");
            foreach (var cat in categories)
            {
                Console.WriteLine($"\nCategory: {cat.CategoryName}");
                if (cat.Products.Any())
                {
                    foreach (var product in cat.Products)
                    {
                        Console.WriteLine($"  - {product.ProductName} (${product.UnitPrice})");
                    }
                }
                else
                {
                    Console.WriteLine("  No products in this category");
                }
            }
        }
    }
}