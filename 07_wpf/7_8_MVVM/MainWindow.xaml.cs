using System;
using System.ComponentModel;
using System.Windows;

namespace _7_8_MVVM;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}

// Model
public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

// ViewModel
    public class StudentViewModel : INotifyPropertyChanged
    {
        private Student _student;

        public StudentViewModel()
        {
            _student = new Student { Name = "John Doe", Age = 20, Email = "john@example.com" };
        }

        public string Name
        {
            get { return _student.Name; }
            set
            {
                _student.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Age
        {
            get { return _student.Age; }
            set
            {
                _student.Age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string Email
        {
            get { return _student.Email; }
            set
            {
                _student.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }