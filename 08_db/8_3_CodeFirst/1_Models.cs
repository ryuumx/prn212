using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

// Code First approach - we define our models first
namespace CodeFirst.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
        public decimal UnitPrice { get; set; }

        [Range(0, short.MaxValue)]
        public short UnitsInStock { get; set; }

        [Range(0, short.MaxValue)]
        public short ReorderLevel { get; set; }

        public bool Discontinued { get; set; } = false;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        // Foreign Key
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        // Navigation property
        public virtual Category Category { get; set; } = null!;
    }

    // Additional entity to show more relationships
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ContactName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        [Phone]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        public bool IsActive { get; set; } = true;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        // Navigation property - one supplier can supply many products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

    // Junction table for many-to-many relationship
    [Table("ProductSuppliers")]
    public class ProductSupplier
    {
        [Key]
        public int ProductSupplierID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SupplierPrice { get; set; }

        public bool IsPreferredSupplier { get; set; } = false;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual Product Product { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
    }

}