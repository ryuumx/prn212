// Demo WPF Control Hierarchy
Button myButton = new Button();
Console.WriteLine($"IsDispatcherObject: {myButton is DispatcherObject}");
Console.WriteLine($"IsVisual: {myButton is Visual}");
Console.WriteLine($"IsUIElement: {myButton is UIElement}");
Console.WriteLine($"IsFrameworkElement: {myButton is FrameworkElement}");
Console.WriteLine($"IsControl: {myButton is Control}");
Console.WriteLine($"IsContentControl: {myButton is ContentControl}");