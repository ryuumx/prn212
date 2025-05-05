using System;

namespace InterfacesAndAbstractions
{
    // Interface definition
    public interface IVehicle
    {
        // Properties
        string RegistrationNumber { get; set; }
        
        // Methods
        void Start();
        void Stop();
        
        // Default interface method (C# 8.0+)
        void DisplayInfo()
        {
            Console.WriteLine($"Vehicle: {RegistrationNumber}");
        }
    }
    
    // Another interface
    public interface IElectric
    {
        int BatteryLevel { get; }
        void Charge();
    }
    
    // Abstract class
    public abstract class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        
        // Constructor
        protected Vehicle(string make, string model, int year)
        {
            Make = make;
            Model = model;
            Year = year;
        }
        
        // Abstract method
        public abstract void DisplayDetails();
        
        // Virtual method with implementation
        public virtual decimal CalculateValue()
        {
            return Year < 2020 ? 10000 : 20000;
        }
    }
    
    // Concrete class implementing interface
    public class Car : Vehicle, IVehicle
    {
        public string RegistrationNumber { get; set; }
        public int NumberOfDoors { get; set; }
        
        public Car(string make, string model, int year) : base(make, model, year)
        {
        }
        
        public void Start()
        {
            Console.WriteLine($"Car {RegistrationNumber} engine started");
        }
        
        public void Stop()
        {
            Console.WriteLine($"Car {RegistrationNumber} engine stopped");
        }
        
        public override void DisplayDetails()
        {
            Console.WriteLine($"Car: {Year} {Make} {Model}, Doors: {NumberOfDoors}");
        }
        
        // Override virtual method
        public override decimal CalculateValue()
        {
            decimal baseValue = base.CalculateValue();
            return NumberOfDoors > 2 ? baseValue * 1.1m : baseValue;
        }
    }
    
    // Multiple interface implementation
    public class ElectricCar : Vehicle, IVehicle, IElectric
    {
        public string RegistrationNumber { get; set; }
        public int BatteryLevel { get; private set; }
        
        public ElectricCar(string make, string model, int year) : base(make, model, year)
        {
            BatteryLevel = 50; // 50% initial charge
        }
        
        public void Start()
        {
            if (BatteryLevel > 0)
                Console.WriteLine($"Electric car {RegistrationNumber} started silently");
            else
                Console.WriteLine("Cannot start: Battery depleted");
        }
        
        public void Stop()
        {
            Console.WriteLine($"Electric car {RegistrationNumber} stopped");
        }
        
        public void Charge()
        {
            BatteryLevel = 100;
            Console.WriteLine("Battery fully charged");
        }
        
        public override void DisplayDetails()
        {
            Console.WriteLine($"Electric Car: {Year} {Make} {Model}, Battery: {BatteryLevel}%");
        }
    }
    
    // Explicit interface implementation
    public class Truck : IVehicle
    {
        private string registrationNumber;
        
        // Explicit implementation
        string IVehicle.RegistrationNumber
        {
            get { return registrationNumber; }
            set { registrationNumber = value; }
        }
        
        // Public property (not part of the interface)
        public string TruckID { get; set; }
        
        void IVehicle.Start()
        {
            Console.WriteLine($"Truck {registrationNumber} engine started");
        }
        
        void IVehicle.Stop()
        {
            Console.WriteLine($"Truck {registrationNumber} engine stopped");
        }
        
        // Must be called through interface reference
        void IVehicle.DisplayInfo()
        {
            Console.WriteLine($"Truck: {registrationNumber}, ID: {TruckID}");
        }
        
        // Public method
        public void StartEngine()
        {
            Console.WriteLine("Starting truck engine...");
            ((IVehicle)this).Start();
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Car example
            Car car = new Car("Toyota", "Corolla", 2022)
            {
                RegistrationNumber = "ABC123",
                NumberOfDoors = 4
            };
            
            car.Start();
            car.DisplayDetails();
            Console.WriteLine($"Car value: ${car.CalculateValue()}");
            
            // Interface as a type
            IVehicle vehicle = car;
            vehicle.DisplayInfo(); // Calls default interface method
            
            // Electric car example
            ElectricCar electricCar = new ElectricCar("Tesla", "Model 3", 2023)
            {
                RegistrationNumber = "EV456"
            };
            
            electricCar.Start();
            electricCar.Charge();
            electricCar.DisplayDetails();
            
            // Explicit interface implementation
            Truck truck = new Truck { TruckID = "T-9000" };
            // truck.RegistrationNumber = "XYZ789"; // Won't compile - not accessible
            
            // Must cast to interface to access members
            IVehicle truckVehicle = truck;
            truckVehicle.RegistrationNumber = "XYZ789";
            truckVehicle.DisplayInfo();
            
            // Using public method that internally uses interface
            truck.StartEngine();
        }
    }
}