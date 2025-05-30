using System;
using System.Windows;

namespace _7_2_Navigation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnToPage01_Click(object sender, RoutedEventArgs e)
        {
            frMain.Source = new Uri("Page_01.xaml", UriKind.Relative);
        }

        private void btnToPage02_Click(object sender, RoutedEventArgs e)
        {
            frMain.Source = new Uri("Page_02.xaml", UriKind.Relative);
        }
    }
}