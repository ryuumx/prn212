using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace SerializationDemos
{
    public class XmlDataProviderDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== XML Data Provider Style Operations ===");
            
            // Create XML data file similar to WPF XmlDataProvider
            CreateSampleXmlData();
            
            // Demonstrate XML querying and manipulation
            DemonstrateXmlQuerying();
            DemonstrateXmlModification();
        }

        private static void CreateSampleXmlData()
        {
            Console.WriteLine("\n=== Creating XML Data (Similar to WPF XmlDataProvider) ===");
            
            string xmlContent = @"<?xml version='1.0' encoding='utf-8'?>
<ContactList>
    <Contact Id='001'>
        <ContactName>Maria Anders</ContactName>
        <Company>Alfreds Futterkiste</Company>
        <Phone>030-0074321</Phone>
        <Department>Sales</Department>
    </Contact>
    <Contact Id='002'>
        <ContactName>Thomas Hardy</ContactName>
        <Company>Around The Horn</Company>
        <Phone>(171) 555-7788</Phone>
        <Department>Marketing</Department>
    </Contact>
    <Contact Id='003'>
        <ContactName>Elizabeth Lincoln</ContactName>
        <Company>Bottom-Dollar Markets</Company>
        <Phone>(604) 555-4729</Phone>
        <Department>Sales</Department>
    </Contact>
    <Contact Id='004'>
        <ContactName>John Smith</ContactName>
        <Company>Tech Solutions Inc</Company>
        <Phone>555-0123</Phone>
        <Department>IT</Department>
    </Contact>
</ContactList>";

            File.WriteAllText("contacts_data.xml", xmlContent);
            Console.WriteLine("Created contacts_data.xml for querying demonstrations");
        }

        private static void DemonstrateXmlQuerying()
        {
            Console.WriteLine("\n=== XML Querying with XPath ===");
            
            XmlDocument doc = new XmlDocument();
            doc.Load("contacts_data.xml");
            
            // Query all contacts
            XmlNodeList allContacts = doc.SelectNodes("//Contact");
            Console.WriteLine($"Total contacts found: {allContacts.Count}");
            
            // Query specific contacts by department
            XmlNodeList salesContacts = doc.SelectNodes("//Contact[Department='Sales']");
            Console.WriteLine($"\nSales department contacts ({salesContacts.Count}):");
            foreach (XmlNode contact in salesContacts)
            {
                string name = contact.SelectSingleNode("ContactName")?.InnerText;
                string company = contact.SelectSingleNode("Company")?.InnerText;
                Console.WriteLine($"  - {name} at {company}");
            }
            
            // Query by attribute
            XmlNode specificContact = doc.SelectSingleNode("//Contact[@Id='002']");
            if (specificContact != null)
            {
                Console.WriteLine($"\nContact with ID '002':");
                Console.WriteLine($"  Name: {specificContact.SelectSingleNode("ContactName")?.InnerText}");
                Console.WriteLine($"  Phone: {specificContact.SelectSingleNode("Phone")?.InnerText}");
            }
            
            // Advanced XPath queries
            XmlNodeList companiesWithThe = doc.SelectNodes("//Contact[contains(Company, 'The')]");
            Console.WriteLine($"\nCompanies containing 'The' ({companiesWithThe.Count}):");
            foreach (XmlNode contact in companiesWithThe)
            {
                string company = contact.SelectSingleNode("Company")?.InnerText;
                Console.WriteLine($"  - {company}");
            }
        }

        private static void DemonstrateXmlModification()
        {
            Console.WriteLine("\n=== XML Modification Operations ===");
            
            XmlDocument doc = new XmlDocument();
            doc.Load("contacts_data.xml");
            
            // Add a new contact
            XmlNode root = doc.DocumentElement;
            XmlElement newContact = doc.CreateElement("Contact");
            newContact.SetAttribute("Id", "005");
            
            XmlElement name = doc.CreateElement("ContactName");
            name.InnerText = "Alice Johnson";
            newContact.AppendChild(name);
            
            XmlElement company = doc.CreateElement("Company");
            company.InnerText = "Innovation Labs";
            newContact.AppendChild(company);
            
            XmlElement phone = doc.CreateElement("Phone");
            phone.InnerText = "555-9999";
            newContact.AppendChild(phone);
            
            XmlElement department = doc.CreateElement("Department");
            department.InnerText = "R&D";
            newContact.AppendChild(department);
            
            root.AppendChild(newContact);
            
            Console.WriteLine("Added new contact: Alice Johnson");
            
            // Modify existing contact
            XmlNode existingContact = doc.SelectSingleNode("//Contact[@Id='003']");
            if (existingContact != null)
            {
                XmlNode phoneNode = existingContact.SelectSingleNode("Phone");
                if (phoneNode != null)
                {
                    string oldPhone = phoneNode.InnerText;
                    phoneNode.InnerText = "(604) 555-NEW1";
                    Console.WriteLine($"Updated phone for Elizabeth Lincoln: {oldPhone} -> {phoneNode.InnerText}");
                }
            }
            
            // Save modified XML
            doc.Save("contacts_modified.xml");
            Console.WriteLine("Saved modifications to contacts_modified.xml");
            
            // Display final result
            Console.WriteLine("\nFinal XML content:");
            using (StringWriter sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
            {
                doc.WriteTo(writer);
                writer.Flush();
                Console.WriteLine(sw.ToString());
            }
        }
    }
}