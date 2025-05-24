using System;
using System.Collections.Generic;
using System.Linq;

namespace Lambda
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }

        public override string ToString()
        {
            return $"{Name} - ${Price:F2} ({Category}) [Stock: {Stock}]";
        }
    }

    public class LambdaExpressions
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== LAMBDA EXPRESSIONS DEMONSTRATION ===");

            // 1. Lambda vs Anonymous Methods Comparison
            Console.WriteLine("1. Lambda vs Anonymous Methods:");
            CompareAnonymousMethodsWithLambdas();
            Console.WriteLine();

            // 2. Basic Lambda Syntax
            Console.WriteLine("2. Basic Lambda Syntax:");
            DemoBasicLambdaSyntax();
            Console.WriteLine();

            // 3. Lambda with Standard Query Operators
            Console.WriteLine("3. Lambda with Standard Query Operators:");
            DemoLambdaWithQueryOperators();
            Console.WriteLine();

            // 4. Lambda Closure and Variable Capture
            Console.WriteLine("4. Lambda Closure and Variable Capture:");
            DemoLambdaClosure();
            Console.WriteLine();

            // 5. Complex Lambda Expressions
            Console.WriteLine("5. Complex Lambda Expressions:");
            DemoComplexLambdas();

            Console.ReadKey();
        }

        public static void CompareAnonymousMethodsWithLambdas()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Anonymous method approach
            Func<int, bool> isEvenAnonymous = delegate(int x) { return x % 2 == 0; };
            Func<int, int> squareAnonymous = delegate(int x) { return x * x; };

            // Lambda expression approach
            Func<int, bool> isEvenLambda = x => x % 2 == 0;
            Func<int, int> squareLambda = x => x * x;

            // Comparison
            Console.WriteLine("Anonymous method results:");
            var evenAnonymous = numbers.Where(isEvenAnonymous).Select(squareAnonymous);
            Console.WriteLine($"Even squares: {string.Join(", ", evenAnonymous)}");

            Console.WriteLine("Lambda expression results:");
            var evenLambda = numbers.Where(isEvenLambda).Select(squareLambda);
            Console.WriteLine($"Even squares: {string.Join(", ", evenLambda)}");

            // Inline lambda (most common)
            Console.WriteLine("Inline lambda results:");
            var evenInline = numbers.Where(x => x % 2 == 0).Select(x => x * x);
            Console.WriteLine($"Even squares: {string.Join(", ", evenInline)}");
        }

        public static void DemoBasicLambdaSyntax()
        {
            // Single parameter lambda (parentheses optional)
            Func<int, int> square = x => x * x;
            Func<string, int> getLength = s => s.Length;
            Func<double, double> absoluteValue = x => x < 0 ? -x : x;

            Console.WriteLine($"Square of 5: {square(5)}");
            Console.WriteLine($"Length of 'Hello': {getLength("Hello")}");
            Console.WriteLine($"Absolute value of -7.5: {absoluteValue(-7.5)}");

            // Multiple parameters (parentheses required)
            Func<int, int, int> add = (x, y) => x + y;
            Func<string, string, string> concatenate = (s1, s2) => s1 + " " + s2;
            Func<double, double, double> hypotenuse = (a, b) => Math.Sqrt(a * a + b * b);

            Console.WriteLine($"Add 3 + 7: {add(3, 7)}");
            Console.WriteLine($"Concatenate 'Hello' + 'World': {concatenate("Hello", "World")}");
            Console.WriteLine($"Hypotenuse of 3,4: {hypotenuse(3, 4):F2}");

            // No parameters (parentheses required)
            Func<string> getTimestamp = () => DateTime.Now.ToString("HH:mm:ss");
            Func<int> getRandomNumber = () => new Random().Next(1, 101);

            Console.WriteLine($"Current time: {getTimestamp()}");
            Console.WriteLine($"Random number: {getRandomNumber()}");

            // Action lambdas (no return value)
            Action<string> printMessage = msg => Console.WriteLine($"📝 {msg}");
            Action<int, int> printSum = (x, y) => Console.WriteLine($"Sum: {x} + {y} = {x + y}");
            Action greetUser = () => Console.WriteLine("👋 Hello, User!");

            printMessage("This is a lambda Action");
            printSum(15, 25);
            greetUser();

            // Multi-statement lambda (braces required)
            Func<int, string> analyzeNumber = n =>
            {
                if (n < 0) return "Negative";
                if (n == 0) return "Zero";
                if (n % 2 == 0) return "Positive Even";
                return "Positive Odd";
            };

            Console.WriteLine($"Analysis of 7: {analyzeNumber(7)}");
            Console.WriteLine($"Analysis of -3: {analyzeNumber(-3)}");
            Console.WriteLine($"Analysis of 8: {analyzeNumber(8)}");
        }

        public static void DemoLambdaWithQueryOperators()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics", Stock = 15 },
                new Product { Id = 2, Name = "Book", Price = 24.99m, Category = "Education", Stock = 50 },
                new Product { Id = 3, Name = "Phone", Price = 699.99m, Category = "Electronics", Stock = 8 },
                new Product { Id = 4, Name = "Desk", Price = 299.99m, Category = "Furniture", Stock = 25 },
                new Product { Id = 5, Name = "Pen", Price = 2.99m, Category = "Office", Stock = 100 },
                new Product { Id = 6, Name = "Monitor", Price = 399.99m, Category = "Electronics", Stock = 12 }
            };

            Console.WriteLine("Original products:");
            products.ForEach(p => Console.WriteLine($"  {p}"));
            Console.WriteLine();

            // Where - filtering
            var expensiveProducts = products.Where(p => p.Price > 300);
            Console.WriteLine("Expensive products (> $300):");
            expensiveProducts.ToList().ForEach(p => Console.WriteLine($"  {p}"));
            Console.WriteLine();

            // Select - transformation
            var productNames = products.Select(p => p.Name.ToUpper());
            Console.WriteLine($"Product names (uppercase): {string.Join(", ", productNames)}");

            var priceCategories = products.Select(p => new { p.Name, Category = p.Price > 100 ? "Expensive" : "Affordable" });
            Console.WriteLine("Price categories:");
            priceCategories.ToList().ForEach(pc => Console.WriteLine($"  {pc.Name}: {pc.Category}"));
            Console.WriteLine();

            // OrderBy / OrderByDescending - sorting
            var sortedByPrice = products.OrderBy(p => p.Price);
            Console.WriteLine("Products sorted by price (ascending):");
            sortedByPrice.ToList().ForEach(p => Console.WriteLine($"  {p.Name}: ${p.Price:F2}"));
            Console.WriteLine();

            var sortedByStockDesc = products.OrderByDescending(p => p.Stock).ThenBy(p => p.Name);
            Console.WriteLine("Products sorted by stock (descending), then by name:");
            sortedByStockDesc.ToList().ForEach(p => Console.WriteLine($"  {p.Name}: {p.Stock} units"));
            Console.WriteLine();

            // Aggregate functions
            var totalValue = products.Sum(p => p.Price * p.Stock);
            var averagePrice = products.Average(p => p.Price);
            var maxPrice = products.Max(p => p.Price);
            var minStock = products.Min(p => p.Stock);
            var productCount = products.Count(p => p.Category == "Electronics");

            Console.WriteLine($"Total inventory value: ${totalValue:F2}");
            Console.WriteLine($"Average price: ${averagePrice:F2}");
            Console.WriteLine($"Most expensive product: ${maxPrice:F2}");
            Console.WriteLine($"Minimum stock level: {minStock}");
            Console.WriteLine($"Electronics products count: {productCount}");
            Console.WriteLine();

            // GroupBy
            var groupedByCategory = products.GroupBy(p => p.Category);
            Console.WriteLine("Products grouped by category:");
            foreach (var group in groupedByCategory)
            {
                Console.WriteLine($"  {group.Key}:");
                group.ToList().ForEach(p => Console.WriteLine($"    {p.Name} - ${p.Price:F2}"));
            }
            Console.WriteLine();

            // Any / All
            bool hasExpensiveItems = products.Any(p => p.Price > 500);
            bool allInStock = products.All(p => p.Stock > 0);
            bool hasElectronics = products.Any(p => p.Category == "Electronics");

            Console.WriteLine($"Has expensive items (> $500): {hasExpensiveItems}");
            Console.WriteLine($"All products in stock: {allInStock}");
            Console.WriteLine($"Has electronics: {hasElectronics}");
            Console.WriteLine();

            // Take / Skip
            var firstThree = products.Take(3);
            var skipFirstTwo = products.Skip(2).Take(2);

            Console.WriteLine("First 3 products:");
            firstThree.ToList().ForEach(p => Console.WriteLine($"  {p.Name}"));

            Console.WriteLine("Products 3-4 (skip 2, take 2):");
            skipFirstTwo.ToList().ForEach(p => Console.WriteLine($"  {p.Name}"));
        }

        public static void DemoLambdaClosure()
        {
            Console.WriteLine("=== Lambda Closure Examples ===");

            // Example 1: Capturing local variables
            int multiplier = 10;
            Func<int, int> multiplyByTen = x => x * multiplier;
            
            Console.WriteLine($"5 * {multiplier} = {multiplyByTen(5)}");
            
            // Change the captured variable
            multiplier = 20;
            Console.WriteLine($"5 * {multiplier} = {multiplyByTen(5)} (captured variable changed)");
            Console.WriteLine();

            // Example 2: Closure in loop (common gotcha)
            Console.WriteLine("Closure in loop - INCORRECT way:");
            var incorrectActions = new List<Action>();
            for (int i = 0; i < 3; i++)
            {
                // This captures the variable 'i', not its value
                incorrectActions.Add(() => Console.WriteLine($"  Loop variable: {i}"));
            }
            
            foreach (var action in incorrectActions)
            {
                action(); // All will print 3!
            }
            Console.WriteLine();

            Console.WriteLine("Closure in loop - CORRECT way:");
            var correctActions = new List<Action>();
            for (int i = 0; i < 3; i++)
            {
                int localCopy = i; // Create local copy
                correctActions.Add(() => Console.WriteLine($"  Local copy: {localCopy}"));
            }
            
            foreach (var action in correctActions)
            {
                action(); // Will print 0, 1, 2
            }
            Console.WriteLine();

            // Example 3: Closure with method parameters
            Func<int, Func<int, int>> CreateMultiplier = factor =>
            {
                return value => value * factor; // Closes over 'factor'
            };

            var multiplyBy5 = CreateMultiplier(5);
            var multiplyBy7 = CreateMultiplier(7);

            Console.WriteLine($"10 * 5 = {multiplyBy5(10)}");
            Console.WriteLine($"10 * 7 = {multiplyBy7(10)}");
            Console.WriteLine();

            // Example 4: Modifying captured variables
            int counter = 0;
            Action incrementCounter = () => counter++;
            Func<int> getCounter = () => counter;

            Console.WriteLine($"Initial counter: {getCounter()}");
            incrementCounter();
            incrementCounter();
            Console.WriteLine($"After 2 increments: {getCounter()}");
        }

        public static void DemoComplexLambdas()
        {
            // Define a proper Student class for complex operations
            var students = new[]
            {
                new { Name = "Alice", Grades = new[] { 85, 92, 78, 96 }, Age = 20 },
                new { Name = "Bob", Grades = new[] { 78, 85, 90, 87 }, Age = 22 },
                new { Name = "Charlie", Grades = new[] { 92, 95, 89, 94 }, Age = 19 },
                new { Name = "Diana", Grades = new[] { 88, 84, 91, 87 }, Age = 21 }
            };

            // Complex lambda with multiple operations
            var studentAnalysis = students
                .Select(s => new
                {
                    s.Name,
                    s.Age,
                    s.Grades,
                    Average = s.Grades.Average(),
                    Highest = s.Grades.Max(),
                    Lowest = s.Grades.Min(),
                    GradeCount = s.Grades.Length
                })
                .Where(s => s.Average >= 85)
                .OrderByDescending(s => s.Average)
                .Select(s => new
                {
                    s.Name,
                    s.Age,
                    s.Grades,
                    Average = Math.Round(s.Average, 2),
                    Performance = s.Average >= 90 ? "Excellent" : "Good"
                });

            Console.WriteLine("Student Analysis (Average >= 85):");
            foreach (var student in studentAnalysis)
            {
                Console.WriteLine($"  {student.Name} ({student.Age}): {student.Average}% - {student.Performance}");
            }
            Console.WriteLine();

            // Lambda with conditional logic
            Func<string, string, int, string> formatStudentInfo = (name, performance, age) =>
                $"{(age < 21 ? "🎓" : "👨‍🎓")} {name} is {age} years old and performing {performance.ToLower()}";

            foreach (var student in studentAnalysis)
            {
                Console.WriteLine(formatStudentInfo(student.Name, student.Performance, student.Age));
            }
            Console.WriteLine();

            // Predicate lambda for complex filtering - using proper typed lambda
            var highPerformers = students.Where(student =>
                student.Grades.Average() > 85 && 
                student.Grades.All(grade => grade >= 75) &&
                student.Age <= 21);

            Console.WriteLine("High performers (avg > 85, all grades >= 75, age <= 21):");
            foreach (var student in highPerformers)
            {
                Console.WriteLine($"  {student.Name}: {student.Grades.Average():F1}% average");
            }
            Console.WriteLine();

            // Advanced lambda expressions with multiple criteria
            var advancedAnalysis = students
                .Select(s => new 
                {
                    s.Name,
                    s.Age,
                    s.Grades,
                    Average = s.Grades.Average(),
                    Range = s.Grades.Max() - s.Grades.Min(),
                    Consistency = s.Grades.All(g => Math.Abs(g - s.Grades.Average()) <= 10)
                })
                .Where(s => s.Average >= 80)
                .OrderBy(s => s.Range)  // Students with most consistent grades first
                .ThenByDescending(s => s.Average);

            Console.WriteLine("Advanced Analysis - Consistent High Performers:");
            foreach (var student in advancedAnalysis)
            {
                var consistencyText = student.Consistency ? "Consistent" : "Variable";
                Console.WriteLine($"  {student.Name}: {student.Average:F1}% avg, Range: {student.Range}, {consistencyText}");
            }
            Console.WriteLine();

            // Demonstrating complex lambda with nested operations
            var gradeDistribution = students
                .SelectMany(s => s.Grades.Select(grade => new { s.Name, Grade = grade }))
                .GroupBy(x => x.Grade >= 90 ? "A" : x.Grade >= 80 ? "B" : x.Grade >= 70 ? "C" : "D")
                .Select(g => new
                {
                    LetterGrade = g.Key,
                    Count = g.Count(),
                    Students = g.Select(x => x.Name).Distinct().ToList(),
                    AverageScore = g.Average(x => x.Grade)
                })
                .OrderByDescending(g => g.AverageScore);

            Console.WriteLine("Grade Distribution Analysis:");
            foreach (var grade in gradeDistribution)
            {
                Console.WriteLine($"Grade {grade.LetterGrade}: {grade.Count} grades, avg {grade.AverageScore:F1}");
                Console.WriteLine($"  Students: [{string.Join(", ", grade.Students)}]");
            }
        }
    }
}