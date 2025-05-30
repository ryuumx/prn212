using System;
using System.Windows;

namespace _7_1_Basic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHello_Click(object sender, RoutedEventArgs e)
        {
            string name = txtInput.Text;
            if (string.IsNullOrEmpty(name))
            {
                name = "Anonymous";
            }
            txtResult.Text = $"Hello {name}! Current time: {DateTime.Now:HH:mm:ss}";
        }
    }
}