using System.Collections.Generic;
using System.Windows;

namespace _7_4_Control;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        var people = new List<Person>
        {
            new Person { Id = 1, Name = "John Doe", Age = 25, City = "New York", IsActive = true },
            new Person { Id = 2, Name = "Jane Smith", Age = 30, City = "Los Angeles", IsActive = false },
            new Person { Id = 3, Name = "Mike Johnson", Age = 35, City = "Chicago", IsActive = true },
            new Person { Id = 4, Name = "Sarah Wilson", Age = 28, City = "Houston", IsActive = true }
        };

        dgData.ItemsSource = people;
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
    public bool IsActive { get; set; }
}