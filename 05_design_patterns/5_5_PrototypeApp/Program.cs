using System;

namespace DesignPatternsDemo
{
    // The prototype abstract class
    public abstract class Car 
    {
        protected int basePrice = 0, onRoadPrice = 0;
        
        public string ModelName { get; set; }
        
        public int BasePrice
        {
            set => basePrice = value;
            get => basePrice;
        }
        
        public int OnRoadPrice
        {
            set => onRoadPrice = value;
            get => onRoadPrice;
        }
        
        // Generate a random additional price for demo purposes
        public static int SetAdditionalPrice()
        {
            Random random = new Random();
            int additionalPrice = random.Next(200_000, 500_000);
            return additionalPrice;
        }
        
        // Abstract Clone method
        public abstract Car Clone();
    }
    
    // Concrete prototype class
    public class Mustang : Car 
    {
        public Mustang(string model)
        {
            ModelName = model;
            BasePrice = 200_000;
        }
        
        // Creating a shallow copy and returning it
        public override Car Clone() => this.MemberwiseClone() as Mustang;
    }
    
    // Another concrete prototype class
    public class Bentley : Car
    {
        public Bentley(string model)
        {
            ModelName = model;
            BasePrice = 300_000;
        }
        
        // Creating a shallow copy and returning it
        public override Car Clone() => this.MemberwiseClone() as Bentley;
    }
    
    // A class with deep copy requirements
    public class Vehicle : ICloneable
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public Engine Engine { get; set; }
        public DateTime ManufactureDate { get; set; }
        
        public Vehicle(string make, string model, Engine engine)
        {
            Make = make;
            Model = model;
            Engine = engine;
            ManufactureDate = DateTime.Now;
        }
        
        // Shallow copy - engine will be shared
        public Vehicle ShallowCopy()
        {
            return (Vehicle)this.MemberwiseClone();
        }
        
        // Deep copy - engine will be cloned
        public object Clone()
        {
            Vehicle clone = (Vehicle)this.MemberwiseClone();
            clone.Engine = (Engine)this.Engine.Clone();
            return clone;
        }
        
        public override string ToString()
        {
            return $"{Make} {Model} with {Engine}, manufactured on {ManufactureDate.ToShortDateString()}";
        }
    }
    
    public class Engine : ICloneable
    {
        public string Type { get; set; }
        public int Horsepower { get; set; }
        public int SerialNumber { get; private set; }
        
        public Engine(string type, int horsepower)
        {
            Type = type;
            Horsepower = horsepower;
            SerialNumber = new Random().Next(10000, 99999);
        }
        
        public object Clone()
        {
            // Create a new instance with a new serial number
            return new Engine(this.Type, this.Horsepower);
        }
        
        public override string ToString()
        {
            return $"{Type} engine ({Horsepower}hp), SN: {SerialNumber}";
        }
    }
    
    // Prototype Registry implementation
    public class VehicleRegistry
    {
        private System.Collections.Generic.Dictionary<string, Vehicle> _vehicles = 
            new System.Collections.Generic.Dictionary<string, Vehicle>();
        
        public VehicleRegistry()
        {
            // Initialize with some base prototypes
            _vehicles["EconomyCar"] = new Vehicle("Toyota", "Corolla", new Engine("Inline-4", 132));
            _vehicles["SportsCar"] = new Vehicle("Mazda", "MX-5", new Engine("Inline-4", 181));
            _vehicles["Luxury"] = new Vehicle("Mercedes", "S-Class", new Engine("V8", 463));
            _vehicles["SUV"] = new Vehicle("Jeep", "Grand Cherokee", new Engine("V6", 293));
        }
        
        public Vehicle GetVehicle(string key)
        {
            if (!_vehicles.ContainsKey(key))
            {
                throw new ArgumentException($"Prototype with key '{key}' doesn't exist.");
            }
            
            // Return a clone, not the original
            return (Vehicle)_vehicles[key].Clone();
        }
        
        public void AddVehicle(string key, Vehicle vehicle)
        {
            _vehicles[key] = vehicle;
        }
        
        public Vehicle GetShallowCopy(string key)
        {
            if (!_vehicles.ContainsKey(key))
            {
                throw new ArgumentException($"Prototype with key '{key}' doesn't exist.");
            }
            
            // Return a shallow copy
            return _vehicles[key].ShallowCopy();
        }
    }
    
    class PrototypePatternDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Prototype Pattern Demo ***\n");
            
            // Basic Car prototype example
            Console.WriteLine("=== Car Prototype Example ===");
            
            // Base or Original Copy
            Car mustang = new Mustang("Mustang EcoBoost");
            Car bentley = new Bentley("Continental GT Mulliner");
            
            Console.WriteLine("Before clone, base prices:");
            Console.WriteLine($"Car is: {mustang.ModelName}, and it's base price is Rs. {mustang.BasePrice}");
            Console.WriteLine($"Car is: {bentley.ModelName}, and it's base price is Rs. {bentley.BasePrice}");
            
            // Clone the cars and modify them
            Car clonedMustang = mustang.Clone();
            // Working on cloned copy
            clonedMustang.OnRoadPrice = clonedMustang.BasePrice + Car.SetAdditionalPrice();
            Console.WriteLine($"Car is: {clonedMustang.ModelName}, and it's price is Rs. {clonedMustang.OnRoadPrice}");
            
            Car clonedBentley = bentley.Clone();
            // Working on cloned copy
            clonedBentley.OnRoadPrice = clonedBentley.BasePrice + Car.SetAdditionalPrice();
            Console.WriteLine($"Car is: {clonedBentley.ModelName}, and it's price is Rs. {clonedBentley.OnRoadPrice}");
            
            // Deep vs Shallow Copy example
            Console.WriteLine("\n=== Deep vs Shallow Copy Example ===");
            
            Engine v8Engine = new Engine("V8", 400);
            Vehicle originalCar = new Vehicle("Ford", "Mustang", v8Engine);
            Console.WriteLine($"Original vehicle: {originalCar}");
            
            // Shallow copy - shares the Engine reference
            Vehicle shallowCopyCar = originalCar.ShallowCopy();
            
            // Deep copy - clones the Engine too
            Vehicle deepCopyCar = (Vehicle)originalCar.Clone();
            
            // Modify the original engine
            originalCar.Engine.Horsepower = 450;
            
            Console.WriteLine("\nAfter modifying original engine horsepower to 450:");
            Console.WriteLine($"Original car: {originalCar}");
            Console.WriteLine($"Shallow copy: {shallowCopyCar}");  // Will reflect engine changes
            Console.WriteLine($"Deep copy: {deepCopyCar}");        // Won't reflect engine changes
            
            // Prototype Registry example
            Console.WriteLine("\n=== Prototype Registry Example ===");
            
            VehicleRegistry registry = new VehicleRegistry();
            
            // Get vehicles from registry (which creates clones)
            Vehicle economyCar = registry.GetVehicle("EconomyCar");
            Vehicle sportsCar = registry.GetVehicle("SportsCar");
            Vehicle luxuryCar = registry.GetVehicle("Luxury");
            
            Console.WriteLine("Vehicles from registry:");
            Console.WriteLine($"Economy: {economyCar}");
            Console.WriteLine($"Sports: {sportsCar}");
            Console.WriteLine($"Luxury: {luxuryCar}");
            
            // Adding a custom prototype
            Engine electricEngine = new Engine("Electric", 300);
            Vehicle electricCar = new Vehicle("Tesla", "Model 3", electricEngine);
            registry.AddVehicle("Electric", electricCar);
            
            // Get a clone of the new prototype
            Vehicle clonedElectricCar = registry.GetVehicle("Electric");
            Console.WriteLine($"\nCloned electric car: {clonedElectricCar}");
            
            // Demonstrate customizing a cloned prototype
            clonedElectricCar.Model = "Model Y";
            clonedElectricCar.Engine.Horsepower = 350;
            Console.WriteLine($"Customized electric car: {clonedElectricCar}");
            
            // Get another clone from registry (original prototype unchanged)
            Vehicle anotherElectricCar = registry.GetVehicle("Electric");
            Console.WriteLine($"Another electric car: {anotherElectricCar}");
            
            Console.ReadLine();
        }
    }
}