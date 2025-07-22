using vinted_hw_assignment.Handlers;
using vinted_hw_assignment.Helpers;
using vinted_hw_assignment.Models;
using vinted_hw_assignment.Services;

namespace Tests.Services;

public class ShippingDiscountCalcServiceTests
{
    [Fact]
    public void Calculate_ShouldNotModifyOriginalData_AndSetPrices()
    {
        // Arrange
        var handlers = new List<IDiscountHandler>
        {
            new SmallShipmentDiscountHandler(),
            new LargeShipmentDiscountHandler()
        };
        
        var service = new ShippingDiscountCalcService(handlers);
        var transactions = TransactionLoader.GetDataFromFile("input.txt");

        // Act
        var updatedTransactions = service.Calculate(transactions);
        bool isOriginalDataMatching = IsOriginalDataMatching(transactions, updatedTransactions);
        bool arePricesNotZero = ArePricesNotZero(transactions);

        // Assert
        Assert.True(isOriginalDataMatching);
        Assert.True(arePricesNotZero);
    }

    private static bool IsOriginalDataMatching(
        List<Transaction>? oldTransactions,
        List<Transaction>? updatedTransactions)
    {
        if (oldTransactions == null || updatedTransactions == null) return false;
        if (oldTransactions.Count != updatedTransactions.Count) return false;

        for (int tr = 0; tr < oldTransactions.Count; tr++)
        {
            if (oldTransactions[tr].Date != updatedTransactions[tr].Date) return false;
            if (oldTransactions[tr].PackageSize != updatedTransactions[tr].PackageSize) return false;
            if (oldTransactions[tr].Provider != updatedTransactions[tr].Provider) return false;
        }
        
        return true;
    }

    private static bool ArePricesNotZero(List<Transaction>? transactions)
    {
        if (transactions == null) return false;
            
        foreach (var tr in transactions)
        {
            if (tr.OriginalPrice == 0.00m && tr.IsValid) return false;
        }

        return true;
    }
}