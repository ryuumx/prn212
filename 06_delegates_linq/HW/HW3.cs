using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegatesLinQ.Homework
{
    // Data models for the homework
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; } // 0-4.0 scale
        public string Semester { get; set; }
        public string Instructor { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    /// <summary>
    /// Homework 3: LINQ Data Processor
    /// 
    /// This is the most challenging homework requiring students to:
    /// 1. Use complex LINQ operations with multiple data sources
    /// 2. Implement custom extension methods
    /// 3. Create generic delegates and expressions
    /// 4. Handle advanced scenarios like pivot operations, statistical analysis
    /// 5. Implement caching and performance optimization
    /// 
    /// Advanced Requirements:
    /// - Custom LINQ extension methods
    /// - Expression trees and dynamic queries
    /// - Performance comparison between different approaches
    /// - Statistical calculations and reporting
    /// - Data transformation and pivot operations
    /// </summary>
    public class LinqDataProcessor
    {
        private List<Student> _students;

        public LinqDataProcessor()
        {
            _students = GenerateSampleData();
        }

        // BASIC REQUIREMENTS (using techniques from sample codes)
        
        public void BasicQueries()
        {
            // TODO: Implement basic LINQ queries similar to 6_8_LinQObject
            
            // 1. Find all students with GPA > 3.5
            // 2. Group students by major
            // 3. Calculate average GPA per major
            // 4. Find students enrolled in specific courses
            // 5. Sort students by enrollment date
            
            Console.WriteLine("=== BASIC LINQ QUERIES ===");
            // Implementation needed by students
        }

        // Challenge 1: Create custom extension methods
        public void CustomExtensionMethods()
        {
            Console.WriteLine("=== CUSTOM EXTENSION METHODS ===");
            
            // TODO: Implement extension methods
            // 1. CreateAverageGPAByMajor() extension method
            // 2. FilterByAgeRange(int min, int max) extension method  
            // 3. ToGradeReport() extension method that creates formatted output
            // 4. CalculateStatistics() extension method
            
            // Example usage (students need to implement the extensions):
            // var highPerformers = _students.FilterByAgeRange(20, 25).Where(s => s.GPA > 3.5);
            // var gradeReport = _students.ToGradeReport();
            // var stats = _students.CalculateStatistics();
        }

        // Challenge 2: Dynamic queries using Expression Trees
        public void DynamicQueries()
        {
            Console.WriteLine("=== DYNAMIC QUERIES ===");
            
            // TODO: Research Expression Trees
            // Implement a method that builds LINQ queries dynamically based on user input
            // Example: BuildDynamicFilter("GPA", ">", "3.5")
            // This requires understanding of Expression<Func<T, bool>>
            
            // Students should implement:
            // 1. Dynamic filtering based on property name and value
            // 2. Dynamic sorting by any property
            // 3. Dynamic grouping operations
        }

        // Challenge 3: Statistical Analysis with Complex Aggregations
        public void StatisticalAnalysis()
        {
            Console.WriteLine("=== STATISTICAL ANALYSIS ===");
            
            // TODO: Implement complex statistical calculations
            // 1. Calculate median GPA (requires custom logic)
            // 2. Calculate standard deviation of GPAs
            // 3. Find correlation between age and GPA
            // 4. Identify outliers using statistical methods
            // 5. Create percentile rankings
            
            // This requires research into statistical formulas and advanced LINQ usage
        }

        // Challenge 4: Data Pivot Operations
        public void PivotOperations()
        {
            Console.WriteLine("=== PIVOT OPERATIONS ===");
            
            // TODO: Research pivot table concepts
            // Create pivot tables showing:
            // 1. Students by Major vs GPA ranges (3.0-3.5, 3.5-4.0, etc.)
            // 2. Course enrollment by semester and major
            // 3. Grade distribution across instructors
            
            // This requires understanding of GroupBy with multiple keys and complex projections
        }

        // Sample data generator
        private List<Student> GenerateSampleData()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1, Name = "Alice Johnson", Age = 20, Major = "Computer Science", 
                    GPA = 3.8, EnrollmentDate = new DateTime(2022, 9, 1),
                    Email = "alice.j@university.edu",
                    Address = new Address { City = "Seattle", State = "WA", ZipCode = "98101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS101", Name = "Intro to Programming", Credits = 3, Grade = 3.7, Semester = "Fall 2022", Instructor = "Dr. Smith" },
                        new Course { Code = "MATH201", Name = "Calculus II", Credits = 4, Grade = 3.9, Semester = "Fall 2022", Instructor = "Prof. Johnson" }
                    }
                },
                new Student
                {
                    Id = 2, Name = "Bob Wilson", Age = 22, Major = "Mathematics", 
                    GPA = 3.2, EnrollmentDate = new DateTime(2021, 9, 1),
                    Email = "bob.w@university.edu",
                    Address = new Address { City = "Portland", State = "OR", ZipCode = "97201" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "MATH301", Name = "Linear Algebra", Credits = 3, Grade = 3.3, Semester = "Spring 2023", Instructor = "Dr. Brown" },
                        new Course { Code = "STAT101", Name = "Statistics", Credits = 3, Grade = 3.1, Semester = "Spring 2023", Instructor = "Prof. Davis" }
                    }
                },
                // Add more sample students...
                new Student
                {
                    Id = 3, Name = "Carol Davis", Age = 19, Major = "Computer Science", 
                    GPA = 3.9, EnrollmentDate = new DateTime(2023, 9, 1),
                    Email = "carol.d@university.edu",
                    Address = new Address { City = "San Francisco", State = "CA", ZipCode = "94101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS102", Name = "Data Structures", Credits = 4, Grade = 4.0, Semester = "Fall 2023", Instructor = "Dr. Smith" },
                        new Course { Code = "CS201", Name = "Algorithms", Credits = 3, Grade = 3.8, Semester = "Fall 2023", Instructor = "Prof. Lee" }
                    }
                }
            };
        }
    }

    // TODO: Implement these extension methods
    public static class StudentExtensions
    {
        // Challenge: Implement custom extension methods
        // public static IEnumerable<Student> FilterByAgeRange(this IEnumerable<Student> students, int minAge, int maxAge)
        // public static Dictionary<string, double> AverageGPAByMajor(this IEnumerable<Student> students)
        // public static string ToGradeReport(this Student student)
        // public static StudentStatistics CalculateStatistics(this IEnumerable<Student> students)
    }

    // TODO: Define this class for statistical operations
    public class StudentStatistics
    {
        // Properties for mean, median, mode, standard deviation, etc.
    }

    public class LinqDataProcessor
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 3: LINQ DATA PROCESSOR ===");
            Console.WriteLine();
            
            Console.WriteLine("BASIC REQUIREMENTS:");
            Console.WriteLine("1. Implement basic LINQ queries (filtering, grouping, sorting)");
            Console.WriteLine("2. Use SelectMany for course data");
            Console.WriteLine("3. Calculate averages and aggregations");
            Console.WriteLine();
            
            Console.WriteLine("ADVANCED REQUIREMENTS:");
            Console.WriteLine("1. Create custom LINQ extension methods");
            Console.WriteLine("2. Implement dynamic queries using Expression Trees");
            Console.WriteLine("3. Perform statistical analysis (median, std dev, correlation)");
            Console.WriteLine("4. Create pivot table operations");
            Console.WriteLine();

            LinqDataProcessor processor = new LinqDataProcessor();
            
            // Students should implement all these methods
            processor.BasicQueries();
            processor.CustomExtensionMethods();
            processor.DynamicQueries();
            processor.StatisticalAnalysis();
            processor.PivotOperations();

            Console.ReadKey();
        }
    }
}