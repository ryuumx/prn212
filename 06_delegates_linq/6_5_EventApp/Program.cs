using System;
using System.Collections.Generic;

namespace Event
{
    // Step 1: Define delegates for events
    public delegate void StockPriceChangedHandler(string stockName, decimal oldPrice, decimal newPrice);
    public delegate void OrderProcessedHandler(string orderNumber, string customerName, decimal amount);
    public delegate void SystemNotificationHandler(string message, DateTime timestamp);

    // Publisher class - Stock
    public class Stock
    {
        private string _symbol;
        private decimal _price;

        public string Symbol 
        { 
            get => _symbol; 
            set => _symbol = value; 
        }

        public decimal Price
        {
            get => _price;
            set
            {
                decimal oldPrice = _price;
                _price = value;
                
                // Step 4: Raise event when price changes
                OnPriceChanged(oldPrice, value);
            }
        }

        // Step 2: Declare event
        public event StockPriceChangedHandler PriceChanged;

        public Stock(string symbol, decimal initialPrice)
        {
            _symbol = symbol;
            _price = initialPrice;
        }

        // Protected method to raise event
        protected virtual void OnPriceChanged(decimal oldPrice, decimal newPrice)
        {
            // Check if there are any subscribers
            PriceChanged?.Invoke(_symbol, oldPrice, newPrice);
        }

        public void SimulatePriceChange()
        {
            Random rand = new Random();
            decimal change = (decimal)(rand.NextDouble() * 10 - 5); // -5 to +5
            Price += change;
        }
    }

    // Publisher class - Order System
    public class OrderSystem
    {
        public event OrderProcessedHandler OrderProcessed;
        public event SystemNotificationHandler SystemNotification;

        private List<string> _processedOrders = new List<string>();

        public void ProcessOrder(string orderNumber, string customerName, decimal amount)
        {
            Console.WriteLine($"Processing order {orderNumber}...");
            
            // Simulate processing time
            System.Threading.Thread.Sleep(100);
            
            _processedOrders.Add(orderNumber);
            
            // Raise events
            OnOrderProcessed(orderNumber, customerName, amount);
            OnSystemNotification($"Order {orderNumber} processed successfully", DateTime.Now);
        }

        protected virtual void OnOrderProcessed(string orderNumber, string customerName, decimal amount)
        {
            OrderProcessed?.Invoke(orderNumber, customerName, amount);
        }

        protected virtual void OnSystemNotification(string message, DateTime timestamp)
        {
            SystemNotification?.Invoke(message, timestamp);
        }

        public void GetOrderHistory()
        {
            Console.WriteLine($"Total processed orders: {_processedOrders.Count}");
            OnSystemNotification($"Order history requested - {_processedOrders.Count} orders found", DateTime.Now);
        }
    }

    // Subscriber classes
    public class StockMonitor
    {
        public string Name { get; set; }

        public StockMonitor(string name)
        {
            Name = name;
        }

        // Step 3: Create event handler methods
        public void OnPriceChanged(string stockName, decimal oldPrice, decimal newPrice)
        {
            decimal changePercent = oldPrice != 0 ? ((newPrice - oldPrice) / oldPrice) * 100 : 0;
            string trend = newPrice > oldPrice ? "📈" : "📉";
            
            Console.WriteLine($"{Name}: {stockName} {trend} ${oldPrice:F2} → ${newPrice:F2} ({changePercent:+0.00;-0.00}%)");
            
            // Alert for significant changes
            if (Math.Abs(changePercent) > 5)
            {
                Console.WriteLine($"⚠️  {Name}: ALERT! Significant price change in {stockName}!");
            }
        }
    }

    public class TradingBot
    {
        public string BotName { get; set; }
        private decimal _threshold;

        public TradingBot(string botName, decimal threshold)
        {
            BotName = botName;
            _threshold = threshold;
        }

        public void OnPriceChanged(string stockName, decimal oldPrice, decimal newPrice)
        {
            Console.WriteLine($"🤖 {BotName}: Analyzing {stockName} price change...");
            
            if (newPrice < _threshold)
            {
                Console.WriteLine($"🤖 {BotName}: BUY signal for {stockName} at ${newPrice:F2}");
            }
            else if (newPrice > _threshold * 1.2m)
            {
                Console.WriteLine($"🤖 {BotName}: SELL signal for {stockName} at ${newPrice:F2}");
            }
            else
            {
                Console.WriteLine($"🤖 {BotName}: HOLD {stockName} at ${newPrice:F2}");
            }
        }
    }

    public class OrderNotificationService
    {
        public void OnOrderProcessed(string orderNumber, string customerName, decimal amount)
        {
            Console.WriteLine($"📧 Email sent to {customerName}: Order {orderNumber} (${amount:F2}) confirmed");
        }

        public void OnSystemNotification(string message, DateTime timestamp)
        {
            Console.WriteLine($"🔔 System Alert [{timestamp:HH:mm:ss}]: {message}");
        }
    }

    public class AuditLogger
    {
        public void OnOrderProcessed(string orderNumber, string customerName, decimal amount)
        {
            Console.WriteLine($"📝 AUDIT LOG: Order {orderNumber} - Customer: {customerName}, Amount: ${amount:F2}");
        }

        public void OnSystemNotification(string message, DateTime timestamp)
        {
            Console.WriteLine($"📝 SYSTEM LOG: {timestamp:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }

    // Class demonstrating event unsubscription and memory leaks
    public class TemporarySubscriber
    {
        public string Name { get; set; }

        public TemporarySubscriber(string name)
        {
            Name = name;
        }

        public void OnPriceChanged(string stockName, decimal oldPrice, decimal newPrice)
        {
            Console.WriteLine($"⏰ {Name}: Temporary monitoring of {stockName}: ${newPrice:F2}");
        }

        // Finalizer to demonstrate memory leak issues
        ~TemporarySubscriber()
        {
            Console.WriteLine($"🗑️  {Name} is being garbage collected");
        }
    }

    public class EventsImplementation
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== EVENTS IMPLEMENTATION DEMO ===");

            // Create publisher objects
            Stock appleStock = new Stock("AAPL", 150.00m);
            Stock microsoftStock = new Stock("MSFT", 300.00m);
            OrderSystem orderSystem = new OrderSystem();

            // Create subscriber objects
            StockMonitor personalMonitor = new StockMonitor("Personal Monitor");
            StockMonitor institutionalMonitor = new StockMonitor("Institutional Monitor");
            TradingBot buyBot = new TradingBot("BuyBot", 140.00m);
            TradingBot sellBot = new TradingBot("SellBot", 160.00m);

            OrderNotificationService notificationService = new OrderNotificationService();
            AuditLogger auditLogger = new AuditLogger();

            Console.WriteLine("1. Subscribing to stock price events...");
            
            // Step 3: Subscribe to events
            appleStock.PriceChanged += personalMonitor.OnPriceChanged;
            appleStock.PriceChanged += institutionalMonitor.OnPriceChanged;
            appleStock.PriceChanged += buyBot.OnPriceChanged;
            
            microsoftStock.PriceChanged += personalMonitor.OnPriceChanged;
            microsoftStock.PriceChanged += sellBot.OnPriceChanged;

            // Subscribe to order events
            orderSystem.OrderProcessed += notificationService.OnOrderProcessed;
            orderSystem.OrderProcessed += auditLogger.OnOrderProcessed;
            orderSystem.SystemNotification += notificationService.OnSystemNotification;
            orderSystem.SystemNotification += auditLogger.OnSystemNotification;

            Console.WriteLine("\n2. Simulating stock price changes...");
            Console.WriteLine(new string('=', 60));
            
            // Simulate price changes
            appleStock.Price = 145.50m;
            Console.WriteLine();
            
            microsoftStock.Price = 305.75m;
            Console.WriteLine();
            
            appleStock.Price = 155.25m; // Should trigger alerts
            Console.WriteLine();

            Console.WriteLine("3. Processing orders...");
            Console.WriteLine(new string('=', 60));
            
            orderSystem.ProcessOrder("ORD001", "John Doe", 1250.00m);
            Console.WriteLine();
            
            orderSystem.ProcessOrder("ORD002", "Jane Smith", 750.50m);
            Console.WriteLine();
            
            orderSystem.GetOrderHistory();
            Console.WriteLine();

            Console.WriteLine("4. Demonstrating event unsubscription...");
            Console.WriteLine(new string('=', 60));
            
            // Unsubscribe institutional monitor from Apple stock
            appleStock.PriceChanged -= institutionalMonitor.OnPriceChanged;
            Console.WriteLine("Institutional monitor unsubscribed from AAPL");
            
            appleStock.Price = 148.75m;
            Console.WriteLine();

            Console.WriteLine("5. Demonstrating temporary subscription (memory leak prevention)...");
            Console.WriteLine(new string('=', 60));
            
            DemonstrateTemporarySubscription(appleStock);
            
            // Force garbage collection to show finalizer
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine();

            Console.WriteLine("6. Anonymous method event handlers...");
            Console.WriteLine(new string('=', 60));
            
            // Add anonymous method event handlers
            appleStock.PriceChanged += delegate(string symbol, decimal oldPrice, decimal newPrice)
            {
                Console.WriteLine($"🔍 Anonymous handler: {symbol} price change detected!");
            };

            orderSystem.OrderProcessed += delegate(string orderNum, string customer, decimal amount)
            {
                if (amount > 1000)
                {
                    Console.WriteLine($"💰 High-value order alert: {orderNum} - ${amount:F2}");
                }
            };

            appleStock.Price = 152.00m;
            orderSystem.ProcessOrder("ORD003", "Big Customer", 5000.00m);

            Console.WriteLine("\n7. Event vs Direct Delegate Access Demo...");
            Console.WriteLine(new string('=', 60));
            DemonstrateEventVsDelegate();

            Console.ReadKey();
        }

        public static void DemonstrateTemporarySubscription(Stock stock)
        {
            TemporarySubscriber tempSubscriber = new TemporarySubscriber("Temp Monitor");
            
            // Subscribe
            stock.PriceChanged += tempSubscriber.OnPriceChanged;
            
            // Trigger event
            stock.Price = 149.00m;
            
            // Unsubscribe (important for memory management)
            stock.PriceChanged -= tempSubscriber.OnPriceChanged;
            Console.WriteLine("Temporary subscriber unsubscribed");
            
            // Set to null to make eligible for garbage collection
            tempSubscriber = null;
        }

        public static void DemonstrateEventVsDelegate()
        {
            Stock demoStock = new Stock("DEMO", 100.00m);
            
            // This is allowed - subscribing to event
            demoStock.PriceChanged += (symbol, oldPrice, newPrice) => 
            {
                Console.WriteLine($"Event subscriber: {symbol} changed to ${newPrice:F2}");
            };

            // This would cause compilation error if we tried to access PriceChanged as delegate:
            // demoStock.PriceChanged = someMethod; // Error: can only use += or -=
            // var handlers = demoStock.PriceChanged; // Error: cannot access event directly

            Console.WriteLine("✅ Events provide encapsulation - external classes can only subscribe/unsubscribe");
            Console.WriteLine("✅ Cannot directly access or reset the event handler list");
            
            demoStock.Price = 105.00m;
        }
    }
}