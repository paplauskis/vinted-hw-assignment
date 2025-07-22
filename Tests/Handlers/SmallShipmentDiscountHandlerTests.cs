using vinted_hw_assignment.Context;
using vinted_hw_assignment.Handlers;
using vinted_hw_assignment.Models;

namespace Tests.Handlers;

public class SmallShipmentDiscountHandlerTests
{
    [Fact]
    public void ApplyDiscount_SmallPackageNotLowestPrice_DiscountApplied()
    {
        //Arrange
        var handler = new SmallShipmentDiscountHandler();
        var context = new DiscountContext();
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.S,
            OriginalPrice = 2.00m,
            PriceWithDiscount = 2.00m,
            Provider = ShippingProvider.LP,
            Date = new DateTime(2018, 7, 10),
            OriginalInput = new DateTime(2018, 7, 10).Date.ToString() + 
                            PackageSize.S + 
                            ShippingProvider.LP
        };

        //Act
        var result = handler.ApplyDiscount(transaction, context);

        decimal lowestPrice = ShippingPrices.GetLowestPriceForSize(PackageSize.S);
        decimal discount = transaction.OriginalPrice - lowestPrice;

        //Assert
        Assert.Equal(discount, result.Discount);
        Assert.Equal(lowestPrice, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_SmallPackageAlreadyLowestPrice_NoDiscountApplied()
    {
        //Arrange
        var handler = new SmallShipmentDiscountHandler();
        var context = new DiscountContext();
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.S,
            OriginalPrice = 1.50m,
            PriceWithDiscount = 1.50m,
            Provider = ShippingProvider.MR,
            Date = new DateTime(2018, 8, 8),
            OriginalInput = new DateTime(2018, 8, 8).Date.ToString() + 
                            PackageSize.S + 
                            ShippingProvider.MR
        };
        
        //Act
        var result = handler.ApplyDiscount(transaction, context);
    
        //Assert
        Assert.Equal(1.50m, result.OriginalPrice);
        Assert.Equal(0.00m, result.Discount);
        Assert.Equal(1.50m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_SmallPackageNotLowestPrice_PartialDiscountApplied()
    {
        //Arrange
        var handler = new SmallShipmentDiscountHandler();
        var context = new DiscountContext();
        var yearMonth = "2019-10";
        var wrongYearMonth = "2019-11";
        
        context.AddDiscount(yearMonth, 9.75m);
        context.AddDiscount(wrongYearMonth, 5.00m);
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.S,
            OriginalPrice = 2.00m,
            PriceWithDiscount = 2.00m,
            Provider = ShippingProvider.MR,
            Date = new DateTime(2019, 10, 10),
            OriginalInput = new DateTime(2019, 10, 10).Date.ToString() + 
                            PackageSize.S + 
                            ShippingProvider.MR
        };

        //Act
        var result = handler.ApplyDiscount(transaction, context);
        var remainingDiscount = context.GetRemainingMonthlyDiscount(yearMonth);

        //Assert
        Assert.Equal(2.00m, result.OriginalPrice);
        Assert.Equal(0.00m, remainingDiscount);
        Assert.Equal(1.75m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_InvalidTransaction_NoDiscountApplied()
    {
        //Arrange
        var handler = new SmallShipmentDiscountHandler();
        var context = new DiscountContext();
        
        var transaction = new Transaction
        {
            IsValid = false,
            PackageSize = PackageSize.S,
            OriginalPrice = 2.00m,
            PriceWithDiscount = 2.00m,
            Date = new DateTime(2025, 7, 7),
            OriginalInput = new DateTime(2025, 7, 7).Date.ToString() + 
            PackageSize.S
        };

        //Act
        var result = handler.ApplyDiscount(transaction, context);

        //Assert
        Assert.Equal(2.00m, result.OriginalPrice);
        Assert.Equal(0.00m, result.Discount);
        Assert.Equal(2.00m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_InvalidPackageSize_NoDiscountApplied()
    {
        //Arrange
        var handler = new SmallShipmentDiscountHandler();
        var context = new DiscountContext();
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.M,
            Provider = ShippingProvider.LP,
            OriginalPrice = 3.00m,
            PriceWithDiscount = 3.00m,
            Date = new DateTime(2023, 2, 2),
            OriginalInput = new DateTime(2022, 2, 2).Date.ToString() + 
                            PackageSize.S
        };

        //Act
        var result = handler.ApplyDiscount(transaction, context);

        //Assert
        Assert.Equal(3.00m, result.OriginalPrice);
        Assert.Equal(0.00m, result.Discount);
        Assert.Equal(3.00m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_SmallPackageNoRemainingDiscount_NoDiscountApplied()
    {
        //Arrange
        var handler = new SmallShipmentDiscountHandler();
        var context = new DiscountContext();
        var yearMonth = "2019-6";
        
        context.AddDiscount(yearMonth, 10.0m);
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.S,
            OriginalPrice = 2.00m,
            PriceWithDiscount = 2.00m,
            Provider = ShippingProvider.MR,
            Date = new DateTime(2019, 6, 13),
            OriginalInput = new DateTime(2019, 6, 3).Date.ToString() + 
                            PackageSize.S + 
                            ShippingProvider.MR
        };

        //Act
        var result = handler.ApplyDiscount(transaction, context);
        var remainingDiscount = context.GetRemainingMonthlyDiscount(yearMonth);

        //Assert
        Assert.Equal(2.00m, result.OriginalPrice);
        Assert.Equal(0.00m, remainingDiscount);
        Assert.Equal(2.00m, result.PriceWithDiscount);
    }
}