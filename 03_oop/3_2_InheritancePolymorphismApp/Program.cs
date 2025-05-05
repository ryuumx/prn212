// Corrected version for InheritanceAndPolymorphism.cs

using System;
using System.Collections.Generic;

namespace InheritanceAndPolymorphism
{
    // Base class - must be marked as abstract
    public abstract class Shape
    {
        // Properties
        public int Height { get; set; }
        public int Width { get; set; }
        
        // Virtual method
        public virtual void Draw()
        {
            Console.WriteLine("Performing base class drawing tasks");
        }
        
        // Non-virtual method
        public void Print()
        {
            Console.WriteLine("Printing shape details");
        }
        
        // Abstract method
        public abstract void Display();
    }
    
    // Derived class
    public class Circle : Shape
    {
        // Additional property
        public double Radius { get; set; }
        
        // Override base class method
        public override void Draw()
        {
            Console.WriteLine("Drawing a circle");
        }
        
        // Implement abstract method
        public override void Display()
        {
            Console.WriteLine("Display Circle");
        }
    }
    
    // Another derived class
    public class Rectangle : Shape
    {
        // Override base class method
        public override void Draw()
        {
            Console.WriteLine("Drawing a rectangle");
        }
        
        // Implement abstract method
        public override void Display()
        {
            Console.WriteLine("Display Rectangle");
        }
        
        // Method hiding (not overriding)
        public new void Print()
        {
            Console.WriteLine("Printing rectangle-specific details");
        }
    }
    
    // Sealed class
    public sealed class Square : Rectangle
    {
        public void SetSize(int size)
        {
            Height = Width = size;
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Create objects
            Circle circle = new Circle { Radius = 5 };
            Rectangle rectangle = new Rectangle { Height = 10, Width = 20 };
            
            // Polymorphism demonstration
            List<Shape> shapes = new List<Shape>
            {
                circle,
                rectangle,
                new Square { Height = 15, Width = 15 }
            };
            
            // Polymorphic behavior
            foreach (Shape shapeObj in shapes)  // Changed variable name from 'shape' to 'shapeObj'
            {
                // Virtual method call
                shapeObj.Draw();
                
                // Abstract method call
                shapeObj.Display();
                
                // Non-virtual method call
                shapeObj.Print();
                
                Console.WriteLine();
            }
            
            // Method hiding demonstration
            Rectangle rect = new Rectangle();
            rect.Print(); // Calls Rectangle.Print()
            
            Shape baseShape = rect;  // Changed variable name from 'shape' to 'baseShape'
            baseShape.Print(); // Calls Shape.Print()
        }
    }
}