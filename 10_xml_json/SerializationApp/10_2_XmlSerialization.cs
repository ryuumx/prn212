using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SerializationDemos
{
    public class XmlSerializationDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== XML Serialization Demonstrations ===");
            
            // Demo 1: Basic XML Serialization
            DemonstrateBasicXmlSerialization();
            
            // Demo 2: Complex Object Serialization
            DemonstrateComplexXmlSerialization();
            
            // Demo 3: XML Deserialization
            DemonstrateXmlDeserialization();
            
            // Demo 4: XML Validation and Error Handling
            DemonstrateXmlErrorHandling();
        }

        private static void DemonstrateBasicXmlSerialization()
        {
            Console.WriteLine("\n=== Basic XML Serialization ===");
            
            // Create sample data
            var contactList = new ContactList();
            contactList.Contacts.AddRange(new[]
            {
                new Contact("001", "Maria Anders", "Alfreds Futterkiste", "030-0074321", "maria@alfreds.com"),
                new Contact("002", "Thomas Hardy", "Around The Horn", "(171) 555-7788", "thomas@aroundthehorn.com"),
                new Contact("003", "Elizabeth Lincoln", "Bottom-Dollar Markets", "(604) 555-4729", "elizabeth@bdm.com"),
                new Contact("004", "John Smith", "Tech Solutions Inc", "555-0123", "john@techsolutions.com")
            });

            string xmlFile = "contacts.xml";
            
            // Serialize to XML
            XmlSerializer serializer = new XmlSerializer(typeof(ContactList));
            
            using (FileStream stream = new FileStream(xmlFile, FileMode.Create))
            {
                serializer.Serialize(stream, contactList);
            }
            
            Console.WriteLine($"Serialized {contactList.Contacts.Count} contacts to {xmlFile}");
            
            // Display the generated XML
            string xmlContent = File.ReadAllText(xmlFile);
            Console.WriteLine("Generated XML content:");
            Console.WriteLine(xmlContent);
        }

        private static void DemonstrateComplexXmlSerialization()
        {
            Console.WriteLine("\n=== Complex Object XML Serialization ===");
            
            var catalog = new ProductCatalog();
            catalog.Products.AddRange(new[]
            {
                new Product(1, "Laptop Pro", "Electronics", 1299.99m, true),
                new Product(2, "Wireless Mouse", "Electronics", 29.99m, true),
                new Product(3, "Office Chair", "Furniture", 199.50m, false),
                new Product(4, "Programming Book", "Books", 49.95m, true),
                new Product(5, "Coffee Mug", "Office Supplies", 12.99m, true)
            });

            string xmlFile = "product_catalog.xml";
            
            // Serialize with custom settings
            XmlSerializer serializer = new XmlSerializer(typeof(ProductCatalog));
            
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\n"
            };

            using (XmlWriter writer = XmlWriter.Create(xmlFile, settings))
            {
                serializer.Serialize(writer, catalog);
            }
            
            Console.WriteLine($"Serialized product catalog with {catalog.Products.Count} products");
            
            // Show formatted XML
            string xmlContent = File.ReadAllText(xmlFile);
            Console.WriteLine("Generated XML (formatted):");
            Console.WriteLine(xmlContent);
        }

        private static void DemonstrateXmlDeserialization()
        {
            Console.WriteLine("\n=== XML Deserialization ===");
            
            string contactsFile = "contacts.xml";
            string catalogFile = "product_catalog.xml";
            
            // Deserialize contacts
            if (File.Exists(contactsFile))
            {
                XmlSerializer contactSerializer = new XmlSerializer(typeof(ContactList));
                
                using (FileStream stream = new FileStream(contactsFile, FileMode.Open))
                {
                    ContactList deserializedContacts = (ContactList)contactSerializer.Deserialize(stream);
                    
                    Console.WriteLine($"Deserialized {deserializedContacts.Contacts.Count} contacts:");
                    foreach (var contact in deserializedContacts.Contacts)
                    {
                        Console.WriteLine($"  - {contact}");
                        if (!string.IsNullOrEmpty(contact.Email))
                        {
                            Console.WriteLine($"    Email: {contact.Email}");
                        }
                    }
                }
            }
            
            // Deserialize product catalog
            if (File.Exists(catalogFile))
            {
                XmlSerializer catalogSerializer = new XmlSerializer(typeof(ProductCatalog));
                
                using (FileStream stream = new FileStream(catalogFile, FileMode.Open))
                {
                    ProductCatalog deserializedCatalog = (ProductCatalog)catalogSerializer.Deserialize(stream);
                    
                    Console.WriteLine($"\nDeserialized product catalog (Version: {deserializedCatalog.Version}):");
                    Console.WriteLine($"Generated: {deserializedCatalog.GeneratedDate}");
                    
                    foreach (var product in deserializedCatalog.Products)
                    {
                        Console.WriteLine($"  - {product.ProductName}: ${product.Price} " +
                                        $"({product.Category}) - {(product.InStock ? "In Stock" : "Out of Stock")}");
                    }
                }
            }
        }

        private static void DemonstrateXmlErrorHandling()
        {
            Console.WriteLine("\n=== XML Error Handling ===");
            
            // Create invalid XML to demonstrate error handling
            string invalidXml = @"<?xml version='1.0'?>
                <ContactList>
                    <Contact Id='001'>
                        <ContactName>John Doe</ContactName>
                        <Company>Tech Corp</Company>
                        <!-- Missing closing tag for Phone -->
                        <Phone>555-0123
                    </Contact>
                </ContactList>";
            
            string invalidXmlFile = "invalid_contacts.xml";
            File.WriteAllText(invalidXmlFile, invalidXml);
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ContactList));
                using (FileStream stream = new FileStream(invalidXmlFile, FileMode.Open))
                {
                    ContactList contacts = (ContactList)serializer.Deserialize(stream);
                    Console.WriteLine("This shouldn't print - XML should be invalid");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"XML Serialization Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"XML Parse Error: {ex.Message}");
                Console.WriteLine($"Line: {ex.LineNumber}, Position: {ex.LinePosition}");
            }
            
            // Clean up
            if (File.Exists(invalidXmlFile))
            {
                File.Delete(invalidXmlFile);
            }
        }
    }
}