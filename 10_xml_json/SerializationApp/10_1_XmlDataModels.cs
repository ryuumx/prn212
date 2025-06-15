using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SerializationDemos
{
    [XmlRoot("ContactList")]
    public class ContactList
    {
        [XmlElement("Contact")]
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }

    public class Contact
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlElement("ContactName")]
        public string ContactName { get; set; }

        [XmlElement("Company")]
        public string Company { get; set; }

        [XmlElement("Phone")]
        public string Phone { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlIgnore]
        public DateTime LastModified { get; set; } = DateTime.Now;

        // Constructor for XML serialization
        public Contact() { }

        public Contact(string id, string name, string company, string phone, string email = "")
        {
            Id = id;
            ContactName = name;
            Company = company;
            Phone = phone;
            Email = email;
        }

        public override string ToString()
        {
            return $"{ContactName} ({Company}) - {Phone}";
        }
    }

    // Additional model for advanced demos
    public class Product
    {
        [XmlAttribute("ProductId")]
        public int ProductId { get; set; }

        [XmlElement("ProductName")]
        public string ProductName { get; set; }

        [XmlElement("Category")]
        public string Category { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }

        [XmlElement("InStock")]
        public bool InStock { get; set; }

        [XmlElement("LastUpdated")]
        public DateTime LastUpdated { get; set; }

        public Product() { }

        public Product(int id, string name, string category, decimal price, bool inStock = true)
        {
            ProductId = id;
            ProductName = name;
            Category = category;
            Price = price;
            InStock = inStock;
            LastUpdated = DateTime.Now;
        }
    }

    [XmlRoot("ProductCatalog")]
    public class ProductCatalog
    {
        [XmlAttribute("Version")]
        public string Version { get; set; } = "1.0";

        [XmlElement("Product")]
        public List<Product> Products { get; set; } = new List<Product>();

        [XmlElement("GeneratedDate")]
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
    }
}