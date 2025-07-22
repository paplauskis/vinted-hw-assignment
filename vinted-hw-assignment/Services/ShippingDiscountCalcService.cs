using vinted_hw_assignment.Context;
using vinted_hw_assignment.Handlers;
using vinted_hw_assignment.Helpers;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Services;

public class ShippingDiscountCalcService
{
    private readonly List<IDiscountHandler> _discountHandlers;

    public ShippingDiscountCalcService(List<IDiscountHandler> discountHandlers)
    {
        _discountHandlers = discountHandlers;
    }
    
    public List<Transaction> Calculate(List<Transaction> transactions)
    {
        var context = new DiscountContext();
        
        foreach (var transaction in transactions)
        {
            if (transaction.OriginalPrice == 0.00m)
            {
                transaction.OriginalPrice = ShippingPrices.GetPrice(
                    transaction.Provider,
                    transaction.PackageSize);

                transaction.PriceWithDiscount = transaction.OriginalPrice;
            }
            
            if (!transaction.IsValid) continue;
            
            foreach (var handler in _discountHandlers)
            {
                handler.ApplyDiscount(transaction, context);
            }
        }
        
        return transactions;
    }
}