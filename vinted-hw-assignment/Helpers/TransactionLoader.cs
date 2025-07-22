using System.Globalization;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Helpers;

public static class TransactionLoader
{
    public static List<Transaction> GetDataFromFile(string filePath)
    {
        List<Transaction> transactions = new();
        string[] content = File.ReadAllText(filePath).Split("\n");

        foreach (var line in content)
        {
            var transaction = ParseTransaction(line);
            transactions.Add(transaction);
        }

        return transactions;
    }

    public static Transaction ParseTransaction(string line)
    {
        Transaction transaction = new()
        {
            OriginalInput = line
        };
        
        string[] data = line.Split(" ");

        try
        {
            if (DateTime.TryParseExact(data[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                if (parsedDate > DateTime.Today)
                    throw new ArgumentException("Date cannot be in the future.");
                
                transaction.Date = parsedDate;
            }

            transaction.PackageSize = Enum.Parse<PackageSize>(data[1]);
            transaction.Provider = Enum.Parse<ShippingProvider>(data[2]);
            transaction.IsValid = true;
        }
        catch (Exception ex) when (ex is ArgumentException || ex is IndexOutOfRangeException)
        {
            transaction.OriginalInput += " Ignored";
            transaction.IsValid = false;
        }

        return transaction;
    }
}