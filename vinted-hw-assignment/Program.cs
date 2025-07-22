using vinted_hw_assignment.Handlers;
using vinted_hw_assignment.Helpers;
using vinted_hw_assignment.Loggers;
using vinted_hw_assignment.Models;
using vinted_hw_assignment.Services;

namespace vinted_hw_assignment;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("No file specified.");
            }
            
            var file = args[0];

            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"File {file} not found.");
            }

            var handlers = new List<IDiscountHandler>
            {
                new SmallShipmentDiscountHandler(),
                new LargeShipmentDiscountHandler()
            };
            
            var data = TransactionLoader.GetDataFromFile(file);
            var calculator = new ShippingDiscountCalcService(handlers);
            var transactions = calculator.Calculate(data);
            
            TransactionLogger.Log(transactions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }
}