using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter06_Session2
{
    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
    }

    public class LinqQueryExpressions
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== LINQ QUERY EXPRESSIONS vs METHOD SYNTAX ===");

            // 1. Basic Query vs Method Syntax
            Console.WriteLine("1. Basic Filtering:");
            BasicQueryComparison();
            Console.WriteLine();

            // 2. Complex Query with Employee Data
            Console.WriteLine("2. Complex Employee Queries:");
            EmployeeQueries();
            Console.WriteLine();

            // 3. Group By Operations
            Console.WriteLine("3. Group By Operations:");
            GroupByDemo();
            Console.WriteLine();

            // 4. Query Expression Keywords
            Console.WriteLine("4. Query Keywords Demo:");
            QueryKeywordsDemo();

            Console.ReadKey();
        }

        public static void BasicQueryComparison()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Query Syntax
            var evenQuery = from n in numbers
                           where n % 2 == 0
                           orderby n descending
                           select n * n;

            // Method Syntax (equivalent)
            var evenMethod = numbers.Where(n => n % 2 == 0)
                                   .OrderByDescending(n => n)
                                   .Select(n => n * n);

            Console.WriteLine("Query Syntax: from n in numbers where n % 2 == 0 orderby n descending select n * n");
            Console.WriteLine($"Result: [{string.Join(", ", evenQuery)}]");
            Console.WriteLine();

            Console.WriteLine("Method Syntax: numbers.Where(n => n % 2 == 0).OrderByDescending(n => n).Select(n => n * n)");
            Console.WriteLine($"Result: [{string.Join(", ", evenMethod)}]");
        }

        public static void EmployeeQueries()
        {
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Department = "IT", Salary = 75000, Age = 32 },
                new Employee { Name = "Jane", Department = "HR", Salary = 65000, Age = 28 },
                new Employee { Name = "Mike", Department = "IT", Salary = 85000, Age = 35 },
                new Employee { Name = "Sarah", Department = "Finance", Salary = 70000, Age = 30 }
            };

            // Query Syntax
            var highPaidQuery = from emp in employees
                               where emp.Salary > 70000 && emp.Age > 30
                               orderby emp.Salary descending
                               select new { emp.Name, emp.Department, emp.Salary };

            Console.WriteLine("Query Syntax - High paid senior employees:");
            foreach (var emp in highPaidQuery)
            {
                Console.WriteLine($"  {emp.Name} ({emp.Department}): ${emp.Salary:N0}");
            }
            Console.WriteLine();

            // Method Syntax
            var highPaidMethod = employees.Where(emp => emp.Salary > 70000 && emp.Age > 30)
                                        .OrderByDescending(emp => emp.Salary)
                                        .Select(emp => new { emp.Name, emp.Department, emp.Salary });

            Console.WriteLine("Method Syntax - Same result:");
            foreach (var emp in highPaidMethod)
            {
                Console.WriteLine($"  {emp.Name} ({emp.Department}): ${emp.Salary:N0}");
            }
        }

        public static void GroupByDemo()
        {
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Department = "IT", Salary = 75000, Age = 32 },
                new Employee { Name = "Jane", Department = "HR", Salary = 65000, Age = 28 },
                new Employee { Name = "Mike", Department = "IT", Salary = 85000, Age = 35 },
                new Employee { Name = "Sarah", Department = "Finance", Salary = 70000, Age = 30 },
                new Employee { Name = "Tom", Department = "IT", Salary = 95000, Age = 40 }
            };

            // Query Syntax Group By
            var deptGroups = from emp in employees
                            group emp by emp.Department into deptGroup
                            select new
                            {
                                Department = deptGroup.Key,
                                Count = deptGroup.Count(),
                                AvgSalary = deptGroup.Average(e => e.Salary)
                            };

            Console.WriteLine("Department Groups (Query Syntax):");
            foreach (var group in deptGroups)
            {
                Console.WriteLine($"  {group.Department}: {group.Count} employees, avg ${group.AvgSalary:N0}");
            }
            Console.WriteLine();

            // Method Syntax Group By
            var deptGroupsMethod = employees.GroupBy(emp => emp.Department)
                                          .Select(g => new
                                          {
                                              Department = g.Key,
                                              Count = g.Count(),
                                              AvgSalary = g.Average(e => e.Salary)
                                          });

            Console.WriteLine("Department Groups (Method Syntax):");
            foreach (var group in deptGroupsMethod)
            {
                Console.WriteLine($"  {group.Department}: {group.Count} employees, avg ${group.AvgSalary:N0}");
            }
        }

        public static void QueryKeywordsDemo()
        {
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Department = "IT", Salary = 75000, Age = 32 },
                new Employee { Name = "Jane", Department = "HR", Salary = 65000, Age = 28 },
                new Employee { Name = "Mike", Department = "IT", Salary = 85000, Age = 35 }
            };

            // Demonstrating 'let' keyword
            var queryWithLet = from emp in employees
                              let bonus = emp.Salary * 0.1m
                              where bonus > 7000
                              select new { emp.Name, emp.Salary, Bonus = bonus };

            Console.WriteLine("Using 'let' keyword for intermediate calculations:");
            foreach (var emp in queryWithLet)
            {
                Console.WriteLine($"  {emp.Name}: Salary ${emp.Salary:N0}, Bonus ${emp.Bonus:N0}");
            }
            Console.WriteLine();

            // Demonstrating 'into' keyword (query continuation)
            var queryWithInto = from emp in employees
                               group emp by emp.Department into deptGroup
                               where deptGroup.Count() > 1
                               select new { Department = deptGroup.Key, Count = deptGroup.Count() };

            Console.WriteLine("Using 'into' keyword for query continuation:");
            foreach (var dept in queryWithInto)
            {
                Console.WriteLine($"  {dept.Department}: {dept.Count} employees");
            }
        }
    }
}