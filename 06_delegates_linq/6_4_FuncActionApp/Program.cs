using System;
using System.Collections.Generic;
using System.Linq;

namespace FuncAction
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double GPA { get; set; }
        public string Major { get; set; }

        public override string ToString()
        {
            return $"{Name} (Age: {Age}, GPA: {GPA:F2}, Major: {Major})";
        }
    }

    public class FuncActionExercises
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== FUNC AND ACTION EXERCISES ===");

            // Exercise 1: Basic Func delegates
            Console.WriteLine("Exercise 1: Basic Func Operations");
            DemoBasicFuncOperations();
            Console.WriteLine();

            // Exercise 2: Action delegates for printing
            Console.WriteLine("Exercise 2: Action Delegates for Student Information");
            DemoStudentActions();
            Console.WriteLine();

            // Exercise 3: Func with collections
            Console.WriteLine("Exercise 3: Func with Collections");
            DemoFuncWithCollections();
            Console.WriteLine();

            // Exercise 4: Practical exercises for students
            Console.WriteLine("Exercise 4: Student Practice Exercises");
            StudentPracticeExercises();
            Console.WriteLine();

            // Exercise 5: Advanced Func and Action combinations
            Console.WriteLine("Exercise 5: Advanced Combinations");
            AdvancedFuncActionDemo();

            Console.ReadKey();
        }

        public static void DemoBasicFuncOperations()
        {
            // Students should create these Func delegates:

            // 1. Func to calculate square of a number
            Func<int, int> square = delegate(int x) 
            { 
                return x * x; 
            };

            // 2. Func to check if number is even
            Func<int, bool> isEven = delegate(int x) 
            { 
                return x % 2 == 0; 
            };

            // 3. Func to calculate power
            Func<int, int, int> power = delegate(int baseNum, int exponent)
            {
                int result = 1;
                for (int i = 0; i < exponent; i++)
                {
                    result *= baseNum;
                }
                return result;
            };

            // 4. Func to get absolute value
            Func<int, int> absoluteValue = delegate(int x)
            {
                return x < 0 ? -x : x;
            };

            // 5. Func for string operations
            Func<string, string> toUpperCase = delegate(string str)
            {
                return str.ToUpper();
            };

            // Testing the Func delegates
            Console.WriteLine($"Square of 5: {square(5)}");
            Console.WriteLine($"Is 6 even? {isEven(6)}");
            Console.WriteLine($"Is 7 even? {isEven(7)}");
            Console.WriteLine($"2 to the power of 3: {power(2, 3)}");
            Console.WriteLine($"Absolute value of -15: {absoluteValue(-15)}");
            Console.WriteLine($"Uppercase 'hello': {toUpperCase("hello")}");
        }

        public static void DemoStudentActions()
        {
            // Sample students
            var students = new List<Student>
            {
                new Student { Name = "Alice", Age = 20, GPA = 3.8, Major = "Computer Science" },
                new Student { Name = "Bob", Age = 22, GPA = 3.2, Major = "Mathematics" },
                new Student { Name = "Charlie", Age = 19, GPA = 3.9, Major = "Physics" },
                new Student { Name = "Diana", Age = 21, GPA = 3.5, Major = "Chemistry" }
            };

            // Students should create these Action delegates:

            // 1. Action to print basic student info
            Action<Student> printBasicInfo = delegate(Student student)
            {
                Console.WriteLine($"Student: {student.Name}, Age: {student.Age}");
            };

            // 2. Action to print detailed student info
            Action<Student> printDetailedInfo = delegate(Student student)
            {
                Console.WriteLine($"📚 {student}");
            };

            // 3. Action to print GPA status
            Action<Student> printGPAStatus = delegate(Student student)
            {
                string status = student.GPA >= 3.5 ? "Excellent" : 
                               student.GPA >= 3.0 ? "Good" : "Needs Improvement";
                Console.WriteLine($"{student.Name}: GPA {student.GPA:F2} - {status}");
            };

            // 4. Action with multiple parameters
            Action<string, double> printGPAReport = delegate(string name, double gpa)
            {
                Console.WriteLine($"📊 GPA Report - {name}: {gpa:F2}");
            };

            // 5. Action for formatting
            Action<Student> printFormattedInfo = delegate(Student student)
            {
                Console.WriteLine(new string('=', 50));
                Console.WriteLine($"Name: {student.Name}");
                Console.WriteLine($"Age: {student.Age}");
                Console.WriteLine($"GPA: {student.GPA:F2}");
                Console.WriteLine($"Major: {student.Major}");
                Console.WriteLine(new string('=', 50));
            };

            // Using the Action delegates
            Console.WriteLine("Basic Info:");
            students.ForEach(printBasicInfo);
            Console.WriteLine();

            Console.WriteLine("Detailed Info:");
            students.ForEach(printDetailedInfo);
            Console.WriteLine();

            Console.WriteLine("GPA Status:");
            students.ForEach(printGPAStatus);
            Console.WriteLine();

            Console.WriteLine("GPA Reports:");
            foreach (var student in students)
            {
                printGPAReport(student.Name, student.GPA);
            }
            Console.WriteLine();

            Console.WriteLine("Formatted Info for first student:");
            printFormattedInfo(students[0]);
        }

        public static void DemoFuncWithCollections()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25 };
            
            // Func delegates for filtering and transformation
            Func<int, bool> isEven = delegate(int n) { return n % 2 == 0; };
            Func<int, bool> isGreaterThan5 = delegate(int n) { return n > 5; };
            Func<int, bool> isDivisibleBy3 = delegate(int n) { return n % 3 == 0; };
            Func<int, int> triple = delegate(int n) { return n * 3; };
            Func<int, string> numberToWord = delegate(int n)
            {
                return n switch
                {
                    1 => "One", 2 => "Two", 3 => "Three", 4 => "Four", 5 => "Five",
                    _ => n.ToString()
                };
            };

            // Using Func with LINQ methods
            Console.WriteLine($"Original numbers: {string.Join(", ", numbers)}");
            
            var evenNumbers = numbers.Where(isEven);
            Console.WriteLine($"Even numbers: {string.Join(", ", evenNumbers)}");
            
            var largeNumbers = numbers.Where(isGreaterThan5);
            Console.WriteLine($"Numbers > 5: {string.Join(", ", largeNumbers)}");
            
            var divisibleBy3 = numbers.Where(isDivisibleBy3);
            Console.WriteLine($"Divisible by 3: {string.Join(", ", divisibleBy3)}");
            
            var tripled = numbers.Take(5).Select(triple);
            Console.WriteLine($"First 5 tripled: {string.Join(", ", tripled)}");
            
            var wordsFirst5 = numbers.Take(5).Select(numberToWord);
            Console.WriteLine($"First 5 as words: {string.Join(", ", wordsFirst5)}");

            // Combining multiple Func delegates
            var evenAndLarge = numbers.Where(isEven).Where(isGreaterThan5);
            Console.WriteLine($"Even and > 5: {string.Join(", ", evenAndLarge)}");
        }

        public static void StudentPracticeExercises()
        {
            Console.WriteLine("🎯 PRACTICE EXERCISES FOR STUDENTS:");
            Console.WriteLine("Try to create the following delegates:\n");

            // Exercise instructions for students
            Console.WriteLine("1. Create a Func<int, bool> to check if a number is prime");
            Console.WriteLine("2. Create a Func<string, int> to count vowels in a string");
            Console.WriteLine("3. Create an Action<string[]> to print all strings in an array");
            Console.WriteLine("4. Create a Func<double, double, double> to calculate hypotenuse");
            Console.WriteLine("5. Create an Action<int> to print multiplication table for a number");
            Console.WriteLine();

            // Sample solutions (students should try first)
            Console.WriteLine("📝 SAMPLE SOLUTIONS:");

            // 1. Prime number checker
            Func<int, bool> isPrime = delegate(int n)
            {
                if (n < 2) return false;
                for (int i = 2; i <= Math.Sqrt(n); i++)
                {
                    if (n % i == 0) return false;
                }
                return true;
            };

            // 2. Vowel counter
            Func<string, int> countVowels = delegate(string str)
            {
                int count = 0;
                string vowels = "aeiouAEIOU";
                foreach (char c in str)
                {
                    if (vowels.Contains(c)) count++;
                }
                return count;
            };

            // 3. Array printer
            Action<string[]> printArray = delegate(string[] arr)
            {
                Console.WriteLine($"Array contents: [{string.Join(", ", arr)}]");
            };

            // 4. Hypotenuse calculator
            Func<double, double, double> calculateHypotenuse = delegate(double a, double b)
            {
                return Math.Sqrt(a * a + b * b);
            };

            // 5. Multiplication table printer
            Action<int> printMultiplicationTable = delegate(int n)
            {
                Console.WriteLine($"Multiplication table for {n}:");
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine($"{n} × {i} = {n * i}");
                }
            };

            // Testing solutions
            Console.WriteLine($"Is 17 prime? {isPrime(17)}");
            Console.WriteLine($"Is 18 prime? {isPrime(18)}");
            Console.WriteLine($"Vowels in 'Programming': {countVowels("Programming")}");
            printArray(new string[] { "Apple", "Banana", "Cherry" });
            Console.WriteLine($"Hypotenuse of 3,4: {calculateHypotenuse(3, 4):F2}");
            printMultiplicationTable(7);
        }

        public static void AdvancedFuncActionDemo()
        {
            // Combining Func and Action in practical scenarios
            var students = new List<Student>
            {
                new Student { Name = "Emma", Age = 20, GPA = 3.9, Major = "Computer Science" },
                new Student { Name = "James", Age = 22, GPA = 2.8, Major = "Mathematics" },
                new Student { Name = "Sophie", Age = 19, GPA = 3.7, Major = "Physics" }
            };

            // Complex Func for student filtering
            Func<Student, bool> isHonorStudent = delegate(Student s)
            {
                return s.GPA >= 3.5 && s.Age <= 21;
            };

            // Action for processing honor students
            Action<Student> processHonorStudent = delegate(Student s)
            {
                Console.WriteLine($"🏆 Honor Student: {s.Name} - GPA: {s.GPA:F2}");
            };

            // Func for student transformation
            Func<Student, string> createStudentSummary = delegate(Student s)
            {
                return $"{s.Name} ({s.Major}) - GPA: {s.GPA:F2}";
            };

            // Using combinations
            Console.WriteLine("Honor Students:");
            students.Where(isHonorStudent).ToList().ForEach(processHonorStudent);
            Console.WriteLine();

            Console.WriteLine("All Student Summaries:");
            var summaries = students.Select(createStudentSummary);
            foreach (var summary in summaries)
            {
                Console.WriteLine(summary);
            }
        }
    }
}