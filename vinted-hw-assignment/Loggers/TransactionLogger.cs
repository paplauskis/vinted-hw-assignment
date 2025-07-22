using System.Globalization;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Loggers;

public static class TransactionLogger
{
    public static void Log(List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            if (!transaction.IsValid)
            {
                Console.WriteLine($"{transaction.OriginalInput}");
            }
            else
            {
                Console.WriteLine($"{transaction.Date:yyyy-MM-dd} " +
                                  $"{transaction.PackageSize} " +
                                  $"{transaction.Provider} " +
                                  $"{FormatPrice(transaction.PriceWithDiscount)} " + 
                                  $"{FormatDiscount(transaction.Discount)}");
            }
        }
    }

    private static string FormatPrice(decimal price)
    {
        return price == 0 ? "0.00" : price.ToString(CultureInfo.InvariantCulture);
    }

    private static string FormatDiscount(decimal amount)
    {
        return amount == 0 ? "-" : amount.ToString(CultureInfo.InvariantCulture);
    }
}