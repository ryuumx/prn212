using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter06_Session2
{
    public class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }
        
        public override string ToString()
        {
            return $"{Name} ({Type}, {Age} years old)";
        }
    }

    public class PetOwner
    {
        public string Name { get; set; }
        public List<string> Pets { get; set; }
        public string City { get; set; }
        
        public PetOwner()
        {
            Pets = new List<string>();
        }
    }

    public class LinqToObjectsComplete
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== LINQ TO OBJECTS - COMPLETE EXAMPLES ===");

            // 1. Basic LINQ Examples from Slides
            Console.WriteLine("1. Basic LINQ Examples:");
            BasicLinqExamples();
            Console.WriteLine();

            // 2. Pet Examples from Slides
            Console.WriteLine("2. Pet Examples:");
            PetExamples();
            Console.WriteLine();

            // 3. SelectMany Advanced Examples
            Console.WriteLine("3. SelectMany Advanced Examples:");
            SelectManyExamples();
            Console.WriteLine();

            // 4. IEnumerable vs IQueryable
            Console.WriteLine("4. IEnumerable vs IQueryable:");
            EnumerableVsQueryableDemo();
            Console.WriteLine();

            // 5. Deferred Execution Demo
            Console.WriteLine("5. Deferred Execution:");
            DeferredExecutionDemo();
            Console.WriteLine();

            // 6. Advanced LINQ Operations
            Console.WriteLine("6. Advanced LINQ Operations:");
            AdvancedLinqOperations();

            Console.ReadKey();
        }

        public static void BasicLinqExamples()
        {
            // Example 1: Even numbers
            int[] n1 = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var evenQuery = from tmp in n1
                           where (tmp % 2) == 0
                           select tmp;

            Console.WriteLine("Example 1 - Even numbers:");
            Console.WriteLine($"Input: [{string.Join(", ", n1)}]");
            Console.WriteLine($"Even numbers: [{string.Join(", ", evenQuery)}]");
            Console.WriteLine();

            // Example 2: Range filtering
            int[] n2 = { 1, 3, -2, -4, -7, -3, -8, 12, 19, 6, 9, 10, 14 };
            var rangeQuery = from tmp in n2
                            where tmp > 0 && tmp < 12
                            select tmp;

            Console.WriteLine("Example 2 - Range filtering:");
            Console.WriteLine($"Input: [{string.Join(", ", n2)}]");
            Console.WriteLine($"Positive numbers < 12: [{string.Join(", ", rangeQuery)}]");
            Console.WriteLine();

            // Example 3: String operations
            List<string> animals = new List<string> { "zebra", "elephant", "cat", "dog", "rhino", "bat" };
            var selectedAnimals = animals.Where(s => s.Length >= 5).Select(x => x.ToUpper());

            Console.WriteLine("Example 3 - String operations:");
            Console.WriteLine($"Input: [{string.Join(", ", animals)}]");
            Console.WriteLine($"Animals with length >= 5 (uppercase): [{string.Join(", ", selectedAnimals)}]");
            Console.WriteLine();

            // Example 4: Top N elements
            List<int> numbers = new List<int> { 6, 0, 999, 11, 443, 6, 1, 24, 54 };
            var top5 = numbers.OrderByDescending(x => x).Take(5);

            Console.WriteLine("Example 4 - Top 5 elements:");
            Console.WriteLine($"Input: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Top 5: [{string.Join(", ", top5)}]");
        }

        public static void PetExamples()
        {
            // Example 5: Pet ordering
            Pet[] pets = { 
                new Pet { Name="Barley", Age=8, Type="Dog" },
                new Pet { Name="Boots", Age=4, Type="Cat" },
                new Pet { Name="Whiskers", Age=1, Type="Cat" },
                new Pet { Name="Daisy", Age=6, Type="Dog" },
                new Pet { Name="Fluffy", Age=2, Type="Rabbit" }
            };

            Console.WriteLine("All pets:");
            foreach (var pet in pets)
            {
                Console.WriteLine($"  {pet}");
            }
            Console.WriteLine();

            // Order by age
            var petsByAge = pets.OrderBy(pet => pet.Age);
            Console.WriteLine("Pets ordered by age:");
            foreach (var pet in petsByAge)
            {
                Console.WriteLine($"  {pet}");
            }
            Console.WriteLine();

            // Order by type, then by age
            var petsByTypeAndAge = pets.OrderBy(pet => pet.Type).ThenBy(pet => pet.Age);
            Console.WriteLine("Pets ordered by type, then by age:");
            foreach (var pet in petsByTypeAndAge)
            {
                Console.WriteLine($"  {pet}");
            }
            Console.WriteLine();

            // Group by type
            var petsByType = pets.GroupBy(pet => pet.Type);
            Console.WriteLine("Pets grouped by type:");
            foreach (var group in petsByType)
            {
                Console.WriteLine($"  {group.Key}s:");
                foreach (var pet in group)
                {
                    Console.WriteLine($"    {pet.Name} ({pet.Age} years)");
                }
            }
        }

        public static void SelectManyExamples()
        {
            // Example from slides
            PetOwner[] petOwners = { 
                new PetOwner { Name="Higa", Pets = new List<string>{ "Scruffy", "Sam" }, City = "Seattle" },
                new PetOwner { Name="Ashkenazi", Pets = new List<string>{ "Walker", "Sugar" }, City = "Redmond" },
                new PetOwner { Name="Price", Pets = new List<string>{ "Scratches", "Diesel" }, City = "Seattle" },
                new PetOwner { Name="Hines", Pets = new List<string>{ "Dusty" }, City = "Tacoma" }
            };

            Console.WriteLine("Pet Owners:");
            foreach (var owner in petOwners)
            {
                Console.WriteLine($"  {owner.Name} from {owner.City}: [{string.Join(", ", owner.Pets)}]");
            }
            Console.WriteLine();

            // SelectMany example from slides
            var query = petOwners.SelectMany(
                petOwner => petOwner.Pets, 
                (petOwner, petName) => new { petOwner, petName })
                .Where(ownerAndPet => ownerAndPet.petName.StartsWith("S"))
                .Select(ownerAndPet => new {
                    Owner = ownerAndPet.petOwner.Name,
                    Pet = ownerAndPet.petName
                });

            Console.WriteLine("Pet names starting with 'S' and their owners:");
            foreach (var item in query)
            {
                Console.WriteLine($"  {item.Pet} belongs to {item.Owner}");
            }
            Console.WriteLine();

            // Alternative SelectMany approaches
            Console.WriteLine("All pets (using SelectMany):");
            var allPets = petOwners.SelectMany(owner => owner.Pets);
            Console.WriteLine($"  All pets: [{string.Join(", ", allPets)}]");
            Console.WriteLine();

            // SelectMany with index
            var petsWithIndex = petOwners.SelectMany((owner, ownerIndex) => 
                owner.Pets.Select((pet, petIndex) => new 
                { 
                    OwnerIndex = ownerIndex,
                    PetIndex = petIndex,
                    OwnerName = owner.Name,
                    PetName = pet 
                }));

            Console.WriteLine("Pets with indices:");
            foreach (var item in petsWithIndex)
            {
                Console.WriteLine($"  Owner[{item.OwnerIndex}].Pet[{item.PetIndex}]: {item.PetName} ({item.OwnerName})");
            }
            Console.WriteLine();

            // Group pets by city
            var petsByCity = petOwners
                .SelectMany(owner => owner.Pets.Select(pet => new { owner.City, Pet = pet }))
                .GroupBy(x => x.City);

            Console.WriteLine("Pets grouped by city:");
            foreach (var cityGroup in petsByCity)
            {
                Console.WriteLine($"  {cityGroup.Key}: [{string.Join(", ", cityGroup.Select(x => x.Pet))}]");
            }
        }

        public static void EnumerableVsQueryableDemo()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // IEnumerable<T> - works with in-memory collections
            IEnumerable<int> evenNumbersEnumerable = numbers.Where(n => n % 2 == 0);
            
            Console.WriteLine("IEnumerable<T> characteristics:");
            Console.WriteLine($"Type: {evenNumbersEnumerable.GetType()}");
            Console.WriteLine($"Result: [{string.Join(", ", evenNumbersEnumerable)}]");
            Console.WriteLine("- Works with in-memory collections");
            Console.WriteLine("- Uses Func<T, bool> delegates");
            Console.WriteLine("- Executes using LINQ to Objects");
            Console.WriteLine();

            // IQueryable<T> - can be translated to other query languages (like SQL)
            IQueryable<int> evenNumbersQueryable = numbers.AsQueryable().Where(n => n % 2 == 0);
            
            Console.WriteLine("IQueryable<T> characteristics:");
            Console.WriteLine($"Type: {evenNumbersQueryable.GetType()}");
            Console.WriteLine($"Result: [{string.Join(", ", evenNumbersQueryable)}]");
            Console.WriteLine("- Can work with remote data sources");
            Console.WriteLine("- Uses Expression<Func<T, bool>> expression trees");
            Console.WriteLine("- Can be translated to SQL, etc.");
            Console.WriteLine();

            // Demonstrating expression trees
            Console.WriteLine("Expression Tree Demo:");
            IQueryable<int> queryableNumbers = numbers.AsQueryable();
            var expression = queryableNumbers.Where(n => n > 5).Expression;
            Console.WriteLine($"Expression: {expression}");
            Console.WriteLine("This expression tree can be analyzed and converted to SQL or other query languages");
        }

        public static void DeferredExecutionDemo()
        {
            Console.WriteLine("=== Deferred Execution Demo ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            
            Console.WriteLine($"Original list: [{string.Join(", ", numbers)}]");
            
            // Create a query (not executed yet)
            var evenNumbers = numbers.Where(n => 
            {
                Console.WriteLine($"  Checking if {n} is even...");
                return n % 2 == 0;
            });
            
            Console.WriteLine("Query created (no execution yet)");
            Console.WriteLine();
            
            // Modify the original list
            numbers.Add(6);
            numbers.Add(7);
            numbers.Add(8);
            
            Console.WriteLine($"Modified list: [{string.Join(", ", numbers)}]");
            Console.WriteLine();
            
            // Now execute the query
            Console.WriteLine("Executing query (foreach):");
            foreach (var num in evenNumbers)
            {
                Console.WriteLine($"  Result: {num}");
            }
            Console.WriteLine();
            
            // Execute again to show it processes the current state
            Console.WriteLine("Executing query again (ToList):");
            var result = evenNumbers.ToList();
            Console.WriteLine($"Final result: [{string.Join(", ", result)}]");
            Console.WriteLine();
            
            // Immediate execution methods
            Console.WriteLine("Immediate execution methods:");
            var immediateResult = numbers.Where(n => n % 2 == 0).ToArray(); // Executes immediately
            Console.WriteLine($"ToArray() result: [{string.Join(", ", immediateResult)}]");
            
            var count = numbers.Count(n => n > 3); // Executes immediately
            Console.WriteLine($"Count(n => n > 3): {count}");
            
            var first = numbers.First(n => n > 5); // Executes immediately
            Console.WriteLine($"First(n => n > 5): {first}");
        }

        public static void AdvancedLinqOperations()
        {
            var employees = new[]
            {
                new { Name = "John", Department = "IT", Salary = 50000, Skills = new[] { "C#", "SQL", "JavaScript" } },
                new { Name = "Jane", Department = "HR", Salary = 45000, Skills = new[] { "Communication", "Recruiting" } },
                new { Name = "Bob", Department = "IT", Salary = 60000, Skills = new[] { "Python", "SQL", "Machine Learning" } },
                new { Name = "Alice", Department = "Finance", Salary = 55000, Skills = new[] { "Excel", "SAP", "Analysis" } },
                new { Name = "Charlie", Department = "IT", Salary = 70000, Skills = new[] { "C#", "Azure", "DevOps" } }
            };

            Console.WriteLine("Employee Data:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp.Name} ({emp.Department}): ${emp.Salary:N0} - Skills: [{string.Join(", ", emp.Skills)}]");
            }
            Console.WriteLine();

            // Aggregate operations
            Console.WriteLine("Aggregate Operations:");
            var totalSalary = employees.Sum(e => e.Salary);
            var avgSalary = employees.Average(e => e.Salary);
            var maxSalary = employees.Max(e => e.Salary);
            var minSalary = employees.Min(e => e.Salary);

            Console.WriteLine($"Total Salary: ${totalSalary:N0}");
            Console.WriteLine($"Average Salary: ${avgSalary:N0}");
            Console.WriteLine($"Max Salary: ${maxSalary:N0}");
            Console.WriteLine($"Min Salary: ${minSalary:N0}");
            Console.WriteLine();

            // Custom aggregation
            var salaryStats = employees.Aggregate(
                new { Count = 0, Sum = 0, Min = int.MaxValue, Max = int.MinValue },
                (acc, emp) => new
                {
                    Count = acc.Count + 1,
                    Sum = acc.Sum + emp.Salary,
                    Min = Math.Min(acc.Min, emp.Salary),
                    Max = Math.Max(acc.Max, emp.Salary)
                },
                acc => new
                {
                    acc.Count,
                    Average = acc.Sum / acc.Count,
                    acc.Min,
                    acc.Max,
                    Range = acc.Max - acc.Min
                });

            Console.WriteLine("Custom Aggregation:");
            Console.WriteLine($"Count: {salaryStats.Count}, Average: ${salaryStats.Average:N0}");
            Console.WriteLine($"Range: ${salaryStats.Range:N0} (${salaryStats.Min:N0} - ${salaryStats.Max:N0})");
            Console.WriteLine();

            // Set operations
            var itSkills = employees.Where(e => e.Department == "IT").SelectMany(e => e.Skills).Distinct();
            var allSkills = employees.SelectMany(e => e.Skills).Distinct();
            var nonItSkills = allSkills.Except(itSkills);

            Console.WriteLine("Set Operations:");
            Console.WriteLine($"IT Skills: [{string.Join(", ", itSkills)}]");
            Console.WriteLine($"All Skills: [{string.Join(", ", allSkills)}]");
            Console.WriteLine($"Non-IT Skills: [{string.Join(", ", nonItSkills)}]");
            Console.WriteLine();

            // Partition operations
            var highEarners = employees.Where(e => e.Salary > 55000);
            var topTwoEarners = employees.OrderByDescending(e => e.Salary).Take(2);
            var skipFirstTwo = employees.OrderBy(e => e.Name).Skip(2);

            Console.WriteLine("Partition Operations:");
            Console.WriteLine("High Earners (> $55K):");
            foreach (var emp in highEarners)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
            }

            Console.WriteLine("Top 2 Earners:");
            foreach (var emp in topTwoEarners)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
            }

            Console.WriteLine("Skip First 2 (by name):");
            foreach (var emp in skipFirstTwo)
            {
                Console.WriteLine($"  {emp.Name}");
            }
            Console.WriteLine();

            // Quantifier operations
            bool allEarnAbove40K = employees.All(e => e.Salary > 40000);
            bool anyInFinance = employees.Any(e => e.Department == "Finance");
            bool anyEarnAbove80K = employees.Any(e => e.Salary > 80000);

            Console.WriteLine("Quantifier Operations:");
            Console.WriteLine($"All earn above $40K: {allEarnAbove40K}");
            Console.WriteLine($"Any in Finance: {anyInFinance}");
            Console.WriteLine($"Any earn above $80K: {anyEarnAbove80K}");
            Console.WriteLine();

            // Complex grouping and projection
            var departmentAnalysis = employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    AverageSalary = g.Average(e => e.Salary),
                    TotalSalary = g.Sum(e => e.Salary),
                    SkillCount = g.SelectMany(e => e.Skills).Distinct().Count(),
                    TopSkills = g.SelectMany(e => e.Skills)
                              .GroupBy(skill => skill)
                              .OrderByDescending(sg => sg.Count())
                              .Take(3)
                              .Select(sg => $"{sg.Key} ({sg.Count()})")
                              .ToList()
                })
                .OrderByDescending(d => d.AverageSalary);

            Console.WriteLine("Department Analysis:");
            foreach (var dept in departmentAnalysis)
            {
                Console.WriteLine($"{dept.Department} Department:");
                Console.WriteLine($"  Employees: {dept.EmployeeCount}");
                Console.WriteLine($"  Average Salary: ${dept.AverageSalary:N0}");
                Console.WriteLine($"  Total Salary: ${dept.TotalSalary:N0}");
                Console.WriteLine($"  Unique Skills: {dept.SkillCount}");
                Console.WriteLine($"  Top Skills: [{string.Join(", ", dept.TopSkills)}]");
                Console.WriteLine();
            }

            // Method chaining complexity - Fixed to show results
            var complexQuery = employees
                .Where(e => e.Salary > 45000)                    // Lower threshold to include more employees
                .SelectMany(e => e.Skills.Select(skill => new    // Flatten skills with employee info
                {
                    e.Name,
                    e.Department,
                    e.Salary,
                    Skill = skill
                }))
                .GroupBy(x => x.Skill)                           // Group by skill
                .Where(g => g.Count() >= 1)                      // Skills used by at least one person
                .Select(g => new                                 // Project to summary
                {
                    Skill = g.Key,
                    UserCount = g.Count(),
                    Departments = g.Select(x => x.Department).Distinct().ToList(),
                    AverageSalaryOfUsers = g.Average(x => x.Salary),
                    Users = g.Select(x => $"{x.Name} (${x.Salary:N0})").ToList()
                })
                .OrderByDescending(x => x.UserCount)             // Sort by popularity
                .ThenByDescending(x => x.AverageSalaryOfUsers);   // Then by salary

            Console.WriteLine("Complex Query - Skills Analysis:");
            foreach (var skillInfo in complexQuery)
            {
                Console.WriteLine($"{skillInfo.Skill}:");
                Console.WriteLine($"  Users: {skillInfo.UserCount}");
                Console.WriteLine($"  Departments: [{string.Join(", ", skillInfo.Departments)}]");
                Console.WriteLine($"  Avg Salary: ${skillInfo.AverageSalaryOfUsers:N0}");
                Console.WriteLine($"  Used by: [{string.Join(", ", skillInfo.Users)}]");
                Console.WriteLine();
            }

            // Additional example - Skills shared across multiple people
            Console.WriteLine("Skills Used by Multiple People:");
            var sharedSkills = employees
                .SelectMany(e => e.Skills.Select(skill => new { e.Name, Skill = skill }))
                .GroupBy(x => x.Skill)
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    Skill = g.Key,
                    Users = g.Select(x => x.Name).ToList()
                });

            foreach (var skill in sharedSkills)
            {
                Console.WriteLine($"  {skill.Skill}: Used by {string.Join(", ", skill.Users)}");
            }
            
            if (!sharedSkills.Any())
            {
                Console.WriteLine("  No skills are shared between multiple employees in this dataset.");
            }
        }
    }
}