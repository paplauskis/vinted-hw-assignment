using vinted_hw_assignment.Context;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Handlers;

public class SmallShipmentDiscountHandler : IDiscountHandler
{
    // returns Transaction for easier testing and/or for future use if code needs to be changed
    public Transaction ApplyDiscount(Transaction transaction, DiscountContext context)
    {
        if (!transaction.IsValid || transaction.PackageSize != PackageSize.S) return transaction;

        var originalPrice = transaction.OriginalPrice;
        var lowestPrice = ShippingPrices.GetLowestPriceForSize(PackageSize.S);

        if (originalPrice < lowestPrice) return transaction;

        var transactionYearMonth = transaction.GetDateYearAndMonth();
        
        var discount = originalPrice - lowestPrice;
        var availableDiscount = context.GetRemainingMonthlyDiscount(transactionYearMonth);
        var finalDiscount = discount < availableDiscount ? discount : availableDiscount;
        transaction.Discount = finalDiscount;
        transaction.PriceWithDiscount = originalPrice - finalDiscount;

        if (finalDiscount > 0)
        {
            context.AddDiscount(transactionYearMonth, finalDiscount);
        }
        
        return transaction;
    }
}