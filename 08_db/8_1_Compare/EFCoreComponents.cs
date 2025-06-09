// Demonstrating main EF Core components
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCompare {

    // 1. Entity Class (POCO)
    public class EFCategory
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<EFProduct> Products { get; set; }
    }

    public class EFProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal UnitPrice { get; set; }
        
        // Navigation property
        public virtual EFCategory Category { get; set; }
    }

    // 2. DbContext Class
    public class MyStoreContext : DbContext
    {
        public DbSet<EFCategory> Categories { get; set; }
        public DbSet<EFProduct> Products { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=MyStore;Trusted_Connection=true;");
        }
    }

    // 3. Simple Usage
    public class SimpleEFDemo
    {
        public static void ShowBasicUsage()
        {
            using var context = new MyStoreContext();

            // Look how simple this is compared to traditional approach!
            var categories = context.Categories.ToList();

            Console.WriteLine("Categories found:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"ID: {cat.CategoryID}, Name: {cat.CategoryName}");
            }
        }
    }
}