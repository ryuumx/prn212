using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CodeFirst.Models;

namespace CodeFirst.Data
{
    public class CodeFirstStoreContext : DbContext
    {
        public CodeFirstStoreContext() { }

        public CodeFirstStoreContext(DbContextOptions<CodeFirstStoreContext> options) : base(options) { }

        // DbSet properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Read connection string from appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);

                // Enable sensitive data logging for development
                #if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(Console.WriteLine);
                #endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category entity using Fluent API
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Index for better performance
                entity.HasIndex(e => e.CategoryName)
                    .IsUnique()
                    .HasDatabaseName("IX_Categories_CategoryName");

                // Configure relationship
                entity.HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0.00m);

                entity.Property(e => e.UnitsInStock)
                    .HasDefaultValue((short)0);

                entity.Property(e => e.ReorderLevel)
                    .HasDefaultValue((short)0);

                entity.Property(e => e.Discontinued)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                // Indexes
                entity.HasIndex(e => e.ProductName)
                    .HasDatabaseName("IX_Products_ProductName");

                entity.HasIndex(e => e.CategoryID)
                    .HasDatabaseName("IX_Products_CategoryID");

                // Check constraints
                entity.HasCheckConstraint("CK_Products_UnitPrice", "[UnitPrice] >= 0");
                entity.HasCheckConstraint("CK_Products_UnitsInStock", "[UnitsInStock] >= 0");
                entity.HasCheckConstraint("CK_Products_ReorderLevel", "[ReorderLevel] >= 0");
            });

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierID);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20);

                entity.Property(e => e.Address)
                    .HasMaxLength(200);

                entity.Property(e => e.City)
                    .HasMaxLength(50);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20);

                entity.Property(e => e.Country)
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                // Index for company name
                entity.HasIndex(e => e.CompanyName)
                    .IsUnique()
                    .HasDatabaseName("IX_Suppliers_CompanyName");
            });

            // Configure many-to-many relationship through junction table
            modelBuilder.Entity<ProductSupplier>(entity =>
            {
                entity.HasKey(e => e.ProductSupplierID);

                entity.Property(e => e.SupplierPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0.00m);

                entity.Property(e => e.IsPreferredSupplier)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                // Configure relationships
                entity.HasOne(ps => ps.Product)
                    .WithMany()
                    .HasForeignKey(ps => ps.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ps => ps.Supplier)
                    .WithMany()
                    .HasForeignKey(ps => ps.SupplierID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Composite index to prevent duplicates
                entity.HasIndex(e => new { e.ProductID, e.SupplierID })
                    .IsUnique()
                    .HasDatabaseName("IX_ProductSuppliers_ProductID_SupplierID");

                // Check constraint
                entity.HasCheckConstraint("CK_ProductSuppliers_SupplierPrice", "[SupplierPrice] >= 0");
            });

            // Seed some initial data: not using for stability
            //SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "Electronics",
                    Description = "Electronic devices and accessories",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "Books",
                    Description = "Books and publications",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Category
                {
                    CategoryID = 3,
                    CategoryName = "Clothing",
                    Description = "Apparel and fashion items",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                }
            );

            // Seed Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    SupplierID = 1,
                    CompanyName = "Tech Solutions Inc.",
                    ContactName = "John Smith",
                    Email = "john@techsolutions.com",
                    Phone = "+1-555-0123",
                    Address = "123 Tech Street",
                    City = "San Francisco",
                    PostalCode = "94105",
                    Country = "USA",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new Supplier
                {
                    SupplierID = 2,
                    CompanyName = "Book World Publishers",
                    ContactName = "Emily Johnson",
                    Email = "emily@bookworld.com",
                    Phone = "+1-555-0124",
                    Address = "456 Literature Ave",
                    City = "New York",
                    PostalCode = "10001",
                    Country = "USA",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductID = 1,
                    ProductName = "Laptop Computer",
                    Description = "High-performance laptop for professionals",
                    UnitPrice = 1299.99m,
                    UnitsInStock = 25,
                    ReorderLevel = 5,
                    Discontinued = false,
                    CategoryID = 1,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    ProductID = 2,
                    ProductName = "Programming Guide",
                    Description = "Complete guide to C# programming",
                    UnitPrice = 49.99m,
                    UnitsInStock = 100,
                    ReorderLevel = 10,
                    Discontinued = false,
                    CategoryID = 2,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    ProductID = 3,
                    ProductName = "Business Shirt",
                    Description = "Professional business shirt",
                    UnitPrice = 79.99m,
                    UnitsInStock = 50,
                    ReorderLevel = 15,
                    Discontinued = false,
                    CategoryID = 3,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}