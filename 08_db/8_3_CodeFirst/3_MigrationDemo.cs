// This file shows commands and explains the migration process

/*
MIGRATION COMMANDS DEMONSTRATION:

1. Create initial migration:
   dotnet ef migrations add InitialCreate

2. Update database:
   dotnet ef database update

3. Add new migration (after model changes):
   dotnet ef migrations add AddProductDescription

4. View migration SQL:
   dotnet ef migrations script

5. Remove last migration (if not applied):
   dotnet ef migrations remove

6. Update to specific migration:
   dotnet ef database update InitialCreate

7. Generate SQL script for specific migration:
   dotnet ef migrations script InitialCreate AddProductDescription
*/

// Configuration file for Entity Framework tools
// File: appsettings.json
/*
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=CodeFirstStoreDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
*/

// Design-time DbContext factory for migrations
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using CodeFirst.Models;
using CodeFirst.Data;

namespace CodeFirst.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CodeFirstStoreContext>
    {
        public CodeFirstStoreContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CodeFirstStoreContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder.UseSqlServer(connectionString);

            return new CodeFirstStoreContext(optionsBuilder.Options);
        }
    }
}

// Migration helper service
namespace CodeFirst.Services
{
    public class DatabaseMigrationService
    {
        private readonly CodeFirstStoreContext _context;

        public DatabaseMigrationService(CodeFirstStoreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> EnsureDatabaseCreatedAsync()
        {
            try
            {
                // Ensure database is created
                await _context.Database.EnsureCreatedAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> MigrateDatabaseAsync()
        {
            try
            {
                // Apply pending migrations
                await _context.Database.MigrateAsync();
                Console.WriteLine("Database migration completed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during migration: {ex.Message}");
                return false;
            }
        }

        public async Task<List<string>> GetPendingMigrationsAsync()
        {
            try
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                return pendingMigrations.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting pending migrations: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<string>> GetAppliedMigrationsAsync()
        {
            try
            {
                var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
                return appliedMigrations.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting applied migrations: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task DisplayDatabaseInfoAsync()
        {
            Console.WriteLine("\n=== Database Information ===");
            
            bool canConnect = await CanConnectAsync();
            Console.WriteLine($"Can connect to database: {(canConnect ? "✓ Yes" : "✗ No")}");

            if (canConnect)
            {
                var appliedMigrations = await GetAppliedMigrationsAsync();
                var pendingMigrations = await GetPendingMigrationsAsync();

                Console.WriteLine($"Applied migrations: {appliedMigrations.Count}");
                foreach (var migration in appliedMigrations)
                {
                    Console.WriteLine($"  ✓ {migration}");
                }

                Console.WriteLine($"Pending migrations: {pendingMigrations.Count}");
                foreach (var migration in pendingMigrations)
                {
                    Console.WriteLine($"  ⏳ {migration}");
                }
            }
        }
    }
}