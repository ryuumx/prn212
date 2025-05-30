// Demo Control Properties
Button myButton = new Button();
myButton.Background = Brushes.LightBlue;
myButton.Foreground = Brushes.DarkBlue;
myButton.FontSize = 16;
myButton.FontWeight = FontWeights.Bold;
myButton.Padding = new Thickness(10);
myButton.Margin = new Thickness(5);
myButton.Content = "My Custom Button";
myButton.Width = 150;
myButton.Height = 40;

// Add to parent container
StackPanel panel = new StackPanel();
panel.Children.Add(myButton);