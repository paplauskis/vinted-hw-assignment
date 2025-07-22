using vinted_hw_assignment.Context;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Handlers;

public class LargeShipmentDiscountHandler : IDiscountHandler
{
    // returns Transaction for easier testing and/or for future use if code needs to be changed
    public Transaction ApplyDiscount(Transaction transaction, DiscountContext context)
    {
        if (!transaction.IsValid ||
            transaction.PackageSize != PackageSize.L ||
            transaction.Provider != ShippingProvider.LP)
            return transaction;
        
        var transactionYearMonth = transaction.GetDateYearAndMonth();
        context.IncrementLpLargeShipments(transactionYearMonth);
        var shipmentCount = context.GetLpLargeShipmentCount(transactionYearMonth);

        if (shipmentCount != 3 || context.IsLpLargeFreeShipmentForMonthValid(transactionYearMonth))
            return transaction;
        
        var availableDiscount = context.GetRemainingMonthlyDiscount(transactionYearMonth);
        var discount = transaction.OriginalPrice < availableDiscount
            ? transaction.OriginalPrice
            : availableDiscount;
        
        transaction.PriceWithDiscount = transaction.OriginalPrice - discount;
        transaction.Discount = discount;

        if (discount > 0)
        {
            context.AddDiscount(transactionYearMonth, discount);
            context.RegisterLpLargeFreeShipment(transactionYearMonth, transaction.Date);
        }

        return transaction;
    }
}