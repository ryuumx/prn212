using System;
using System.Collections.Generic;

namespace DesignPatternsDemo
{
    // The Product interface
    public interface IAnimal
    {
        void AboutMe();
    }
    
    // Concrete Products
    public class Lion : IAnimal
    {
        public void AboutMe() => Console.WriteLine("This is Lion.");
    }
    
    public class Tiger : IAnimal
    {
        public void AboutMe() => Console.WriteLine("This is Tiger.");
    }
    
    public class Elephant : IAnimal
    {
        public void AboutMe() => Console.WriteLine("This is Elephant.");
    }
    
    // The Creator abstract class
    public abstract class AnimalFactory
    {
        /*
         * Factory method lets a class defer instantiation to subclasses.
         * The following method will create a Tiger or a Lion,
         * but at this point it does not know whether it will get a Lion or a Tiger.
         * It will be decided by the subclasses i.e. LionFactory or TigerFactory.
         * So, the following method is acting like a factory (of creation).
         */
        public abstract IAnimal CreateAnimal();
        
        // The Creator may also provide a default implementation
        public void ExplainFactory()
        {
            Console.WriteLine($"I am a {this.GetType().Name} and I create {CreateAnimal().GetType().Name}s");
        }
    }
    
    // Concrete Creators
    public class LionFactory : AnimalFactory
    {
        // Creating a Lion
        public override IAnimal CreateAnimal() => new Lion();
    }
    
    public class TigerFactory : AnimalFactory
    {
        // Creating a Tiger
        public override IAnimal CreateAnimal() => new Tiger();
    }
    
    public class ElephantFactory : AnimalFactory
    {
        // Creating an Elephant
        public override IAnimal CreateAnimal() => new Elephant();
    }
    
    // Real-world example: Document factory
    public interface IDocument
    {
        void Open();
        void Save();
        void Close();
    }
    
    public class TextDocument : IDocument
    {
        public void Open() => Console.WriteLine("Opening text document...");
        public void Save() => Console.WriteLine("Saving text document...");
        public void Close() => Console.WriteLine("Closing text document...");
    }
    
    public class SpreadsheetDocument : IDocument
    {
        public void Open() => Console.WriteLine("Opening spreadsheet document...");
        public void Save() => Console.WriteLine("Saving spreadsheet document...");
        public void Close() => Console.WriteLine("Closing spreadsheet document...");
    }
    
    public class PresentationDocument : IDocument
    {
        public void Open() => Console.WriteLine("Opening presentation document...");
        public void Save() => Console.WriteLine("Saving presentation document...");
        public void Close() => Console.WriteLine("Closing presentation document...");
    }
    
    public abstract class DocumentCreator
    {
        // Template method that uses the factory method
        public void OpenAndEditDocument()
        {
            IDocument document = CreateDocument();
            document.Open();
            Console.WriteLine("Editing document content...");
            document.Save();
        }
        
        // Factory method
        public abstract IDocument CreateDocument();
    }
    
    public class TextDocumentCreator : DocumentCreator
    {
        public override IDocument CreateDocument() => new TextDocument();
    }
    
    public class SpreadsheetDocumentCreator : DocumentCreator
    {
        public override IDocument CreateDocument() => new SpreadsheetDocument();
    }
    
    public class PresentationDocumentCreator : DocumentCreator
    {
        public override IDocument CreateDocument() => new PresentationDocument();
    }
    
    class FactoryMethodPatternDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Factory Method Pattern Demo ***\n");
            
            // Basic Factory Method example
            Console.WriteLine("=== Basic Factory Method Example ===");
            
            // Create a list of animal factories
            List<AnimalFactory> animalFactories = new List<AnimalFactory>
            {
                new TigerFactory(),
                new LionFactory()
            };
            
            // Use each factory to create animals
            foreach (var factory in animalFactories)
            {
                factory.ExplainFactory();
                IAnimal animal = factory.CreateAnimal();
                animal.AboutMe();
                Console.WriteLine();
            }
            
            // Document creator example
            Console.WriteLine("\n=== Document Creator Example ===");
            
            // Client doesn't know which type of document will be created
            DocumentCreator[] creators = new DocumentCreator[]
            {
                new TextDocumentCreator(),
                new SpreadsheetDocumentCreator(),
                new PresentationDocumentCreator()
            };
            
            // Client works with creators through abstract interface
            foreach (var creator in creators)
            {
                Console.WriteLine($"\nUsing {creator.GetType().Name}:");
                creator.OpenAndEditDocument();
            }
            
            // Dynamic factory selection based on file extension
            Console.WriteLine("\n=== Dynamic Factory Selection ===");
            string[] filesToOpen = { "report.txt", "budget.xlsx", "presentation.pptx" };
            
            foreach (var file in filesToOpen)
            {
                DocumentCreator creator = GetDocumentCreatorForFile(file);
                Console.WriteLine($"\nOpening file: {file}");
                creator.OpenAndEditDocument();
            }
            
            Console.ReadLine();
        }
        
        // Helper method to select appropriate factory based on file extension
        private static DocumentCreator GetDocumentCreatorForFile(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLower();
            
            switch (extension)
            {
                case ".txt":
                    return new TextDocumentCreator();
                case ".xlsx":
                    return new SpreadsheetDocumentCreator();
                case ".pptx":
                    return new PresentationDocumentCreator();
                default:
                    throw new ArgumentException($"Unsupported file extension: {extension}");
            }
        }
    }
}