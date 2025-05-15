using System;

namespace DesignPatternsDemo
{
    // Abstract Product interfaces
    public interface IOrder 
    { 
        void ProcessOrder(); 
    } 
    
    public interface IPayment 
    { 
        void ProcessPayment(); 
    } 
    
    public interface IShipping 
    { 
        void ShipOrder(); 
    } 
    
    // Abstract Factory interface
    public interface IOrderFactory 
    { 
        IOrder CreateOrder(); 
        IPayment CreatePayment(); 
        IShipping CreateShipping(); 
    } 
    
    // Concrete Product classes for Domestic orders
    public class DomesticOrder : IOrder 
    { 
        public void ProcessOrder() 
        { 
            Console.WriteLine("Processing domestic order..."); 
        } 
    } 
    
    public class DomesticPayment : IPayment 
    { 
        public void ProcessPayment() 
        { 
            Console.WriteLine("Processing domestic payment..."); 
        } 
    } 
    
    public class DomesticShipping : IShipping 
    { 
        public void ShipOrder() 
        { 
            Console.WriteLine("Shipping domestic order..."); 
        } 
    }
    
    // Concrete Product classes for International orders
    public class InternationalOrder : IOrder 
    { 
        public void ProcessOrder() 
        { 
            Console.WriteLine("Processing international order..."); 
        } 
    } 
    
    public class InternationalPayment : IPayment 
    { 
        public void ProcessPayment() 
        { 
            Console.WriteLine("Processing international payment..."); 
        } 
    } 
    
    public class InternationalShipping : IShipping 
    { 
        public void ShipOrder() 
        { 
            Console.WriteLine("Shipping international order..."); 
        } 
    }
    
    // Concrete Factory classes
    public class DomesticOrderFactory : IOrderFactory 
    { 
        public IOrder CreateOrder() 
        { 
            return new DomesticOrder(); 
        } 
        
        public IPayment CreatePayment() 
        { 
            return new DomesticPayment(); 
        } 
        
        public IShipping CreateShipping() 
        { 
            return new DomesticShipping(); 
        } 
    } 
    
    public class InternationalOrderFactory : IOrderFactory 
    { 
        public IOrder CreateOrder() 
        { 
            return new InternationalOrder(); 
        } 
        
        public IPayment CreatePayment() 
        { 
            return new InternationalPayment(); 
        } 
        
        public IShipping CreateShipping() 
        { 
            return new InternationalShipping(); 
        } 
    }
    
    // Another concrete factory for expedited orders
    public class ExpeditedOrderFactory : IOrderFactory
    {
        public IOrder CreateOrder()
        {
            return new ExpeditedOrder();
        }
        
        public IPayment CreatePayment()
        {
            return new PremiumPayment();
        }
        
        public IShipping CreateShipping()
        {
            return new ExpressShipping();
        }
    }
    
    public class ExpeditedOrder : IOrder
    {
        public void ProcessOrder()
        {
            Console.WriteLine("Processing expedited order with rush handling...");
        }
    }
    
    public class PremiumPayment : IPayment
    {
        public void ProcessPayment()
        {
            Console.WriteLine("Processing premium payment with priority...");
        }
    }
    
    public class ExpressShipping : IShipping
    {
        public void ShipOrder()
        {
            Console.WriteLine("Shipping order with express delivery...");
        }
    }
    
    // Real-world GUI example
    // Abstract product interfaces
    public interface IButton
    {
        void Render();
        void HandleClick();
    }
    
    public interface ITextBox
    {
        void Render();
        void HandleInput();
    }
    
    public interface ICheckbox
    {
        void Render();
        void HandleToggle();
    }
    
    // Abstract GUI factory
    public interface IGUIFactory
    {
        IButton CreateButton();
        ITextBox CreateTextBox();
        ICheckbox CreateCheckbox();
    }
    
    // Windows UI components
    public class WindowsButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("Rendering a Windows-style button");
        }
        
        public void HandleClick()
        {
            Console.WriteLine("Handling click event for Windows button");
        }
    }
    
    public class WindowsTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("Rendering a Windows-style text box");
        }
        
        public void HandleInput()
        {
            Console.WriteLine("Handling input for Windows text box");
        }
    }
    
    public class WindowsCheckbox : ICheckbox
    {
        public void Render()
        {
            Console.WriteLine("Rendering a Windows-style checkbox");
        }
        
        public void HandleToggle()
        {
            Console.WriteLine("Handling toggle for Windows checkbox");
        }
    }
    
    // macOS UI components
    public class MacOSButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("Rendering a macOS-style button");
        }
        
        public void HandleClick()
        {
            Console.WriteLine("Handling click event for macOS button");
        }
    }
    
    public class MacOSTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("Rendering a macOS-style text box");
        }
        
        public void HandleInput()
        {
            Console.WriteLine("Handling input for macOS text box");
        }
    }
    
    public class MacOSCheckbox : ICheckbox
    {
        public void Render()
        {
            Console.WriteLine("Rendering a macOS-style checkbox");
        }
        
        public void HandleToggle()
        {
            Console.WriteLine("Handling toggle for macOS checkbox");
        }
    }
    
    // Concrete GUI factories
    public class WindowsGUIFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new WindowsButton();
        }
        
        public ITextBox CreateTextBox()
        {
            return new WindowsTextBox();
        }
        
        public ICheckbox CreateCheckbox()
        {
            return new WindowsCheckbox();
        }
    }
    
    public class MacOSGUIFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new MacOSButton();
        }
        
        public ITextBox CreateTextBox()
        {
            return new MacOSTextBox();
        }
        
        public ICheckbox CreateCheckbox()
        {
            return new MacOSCheckbox();
        }
    }
    
    // Application using the UI components
    public class Application
    {
        private readonly IButton _button;
        private readonly ITextBox _textBox;
        private readonly ICheckbox _checkbox;
        
        public Application(IGUIFactory factory)
        {
            _button = factory.CreateButton();
            _textBox = factory.CreateTextBox();
            _checkbox = factory.CreateCheckbox();
        }
        
        public void RenderUI()
        {
            Console.WriteLine("Rendering application UI:");
            _button.Render();
            _textBox.Render();
            _checkbox.Render();
        }
        
        public void SimulateUserInteraction()
        {
            Console.WriteLine("\nSimulating user interaction:");
            _button.HandleClick();
            _textBox.HandleInput();
            _checkbox.HandleToggle();
        }
    }
    
    class AbstractFactoryPatternDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Abstract Factory Pattern Demo ***\n");
            
            // Order processing example
            Console.WriteLine("=== Order Processing Example ===");
            
            // Create a domestic order
            Console.WriteLine("\nProcessing a domestic order:");
            IOrderFactory domesticFactory = new DomesticOrderFactory();
            ProcessOrder(domesticFactory);
            
            // Create an international order
            Console.WriteLine("\nProcessing an international order:");
            IOrderFactory internationalFactory = new InternationalOrderFactory();
            ProcessOrder(internationalFactory);
            
            // Create an expedited order
            Console.WriteLine("\nProcessing an expedited order:");
            IOrderFactory expeditedFactory = new ExpeditedOrderFactory();
            ProcessOrder(expeditedFactory);
            
            // GUI example
            Console.WriteLine("\n=== Cross-Platform GUI Example ===");
            
            // Determine which factory to use based on operating system
            string os = Environment.OSVersion.Platform == PlatformID.Win32NT ? "Windows" : "macOS";
            Console.WriteLine($"Detected operating system: {os}");
            
            IGUIFactory guiFactory;
            
            if (os == "Windows")
            {
                guiFactory = new WindowsGUIFactory();
            }
            else
            {
                guiFactory = new MacOSGUIFactory();
            }
            
            // Create and use application with appropriate UI components
            Application app = new Application(guiFactory);
            app.RenderUI();
            app.SimulateUserInteraction();
            
            // Demonstrate switching to a different UI style
            Console.WriteLine("\nSwitching to a different UI style:");
            IGUIFactory alternativeFactory = (os == "Windows") 
                ? new MacOSGUIFactory() 
                : new WindowsGUIFactory();
            
            Application alternativeApp = new Application(alternativeFactory);
            alternativeApp.RenderUI();
            alternativeApp.SimulateUserInteraction();
            
            Console.ReadLine();
        }
        
        // Client code that works with factories and products through abstract interfaces
        static void ProcessOrder(IOrderFactory factory)
        {
            IOrder order = factory.CreateOrder();
            IPayment payment = factory.CreatePayment();
            IShipping shipping = factory.CreateShipping();
            
            // Use the created objects
            order.ProcessOrder();
            payment.ProcessPayment();
            shipping.ShipOrder();
        }
    }
}