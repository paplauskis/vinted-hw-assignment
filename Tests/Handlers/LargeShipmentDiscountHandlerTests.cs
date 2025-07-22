using vinted_hw_assignment.Context;
using vinted_hw_assignment.Handlers;
using vinted_hw_assignment.Models;

namespace Tests.Handlers;

public class LargeShipmentDiscountHandlerTests
{
    [Fact]
    public void ApplyDiscount_ThirdLargeShipment_DiscountApplied()
    {
        // Arrange
        var handler = new LargeShipmentDiscountHandler();
        var context = new DiscountContext();
        string yearMonth = "2018-7";
        
        context.IncrementLpLargeShipments(yearMonth);
        context.IncrementLpLargeShipments(yearMonth);

        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.L,
            Provider = ShippingProvider.LP,
            OriginalPrice = 6.90m,
            PriceWithDiscount = 6.90m,
            Date = new DateTime(2018, 7, 10),
            OriginalInput = new DateTime(2018, 7, 10).Date.ToString() + 
                            PackageSize.L + 
                            ShippingProvider.LP
        };

        // Act
        var result = handler.ApplyDiscount(transaction, context);

        // Assert
        Assert.Equal(6.90m, result.OriginalPrice);
        Assert.Equal(6.90m, result.Discount);
        Assert.Equal(0.00m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_WrongProvider_NoDiscountApplied()
    {
        // Arrange
        var handler = new LargeShipmentDiscountHandler();
        var context = new DiscountContext();
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.L,
            Provider = ShippingProvider.MR,
            OriginalPrice = 4.00m,
            PriceWithDiscount = 4.00m,
            Date = new DateTime(2015, 2, 1),
            OriginalInput = new DateTime(2015, 2, 1).Date.ToString() + 
            PackageSize.L + 
            ShippingProvider.MR
        };

        // Act
        var result = handler.ApplyDiscount(transaction, context);

        // Assert
        Assert.Equal(4.00m, result.OriginalPrice);
        Assert.Equal(0.00m, result.Discount);
        Assert.Equal(4.00m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_WrongSize_NoDiscountApplied()
    {
        // Arrange
        var handler = new LargeShipmentDiscountHandler();
        var context = new DiscountContext();
        
        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.M,
            Provider = ShippingProvider.LP,
            OriginalPrice = 4.90m,
            PriceWithDiscount = 4.90m,
            Date = new DateTime(2019, 2, 1),
            OriginalInput = new DateTime(2019, 2, 1).Date.ToString() + 
                            PackageSize.M + 
                            ShippingProvider.LP
        };

        // Act
        var result = handler.ApplyDiscount(transaction, context);

        // Assert
        Assert.Equal(4.90m, result.OriginalPrice);
        Assert.Equal(0.00m, result.Discount);
        Assert.Equal(4.90m, result.PriceWithDiscount);
    }

    [Fact] public void ApplyDiscount_ThirdLargeShipment_ApplyPartialDiscount()
    {
        // Arrange
        var handler = new LargeShipmentDiscountHandler();
        var context = new DiscountContext();
        string yearMonth = "2015-3";
        
        context.IncrementLpLargeShipments(yearMonth);
        context.IncrementLpLargeShipments(yearMonth);
        context.AddDiscount(yearMonth, 8.00m);

        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.L,
            Provider = ShippingProvider.LP,
            OriginalPrice = 11.0m,
            PriceWithDiscount = 11.0m,
            Date = new DateTime(2015, 3, 3),
            OriginalInput = new DateTime(2015, 3, 3).Date.ToString() + 
                            PackageSize.L + 
                            ShippingProvider.LP
        };

        // Act
        var result = handler.ApplyDiscount(transaction, context);

        // Assert
        Assert.Equal(11.00m, result.OriginalPrice);
        Assert.Equal(2.00m, result.Discount);
        Assert.Equal(9.00m, result.PriceWithDiscount);
    }
    
    [Fact]
    public void ApplyDiscount_FourthLargeShipment_NoDiscount()
    {
        // Arrange
        var handler = new LargeShipmentDiscountHandler();
        var context = new DiscountContext();
        var date = new DateTime(2016, 3, 22);
        string yearMonth = "2016-3";
        
        context.IncrementLpLargeShipments(yearMonth);
        context.IncrementLpLargeShipments(yearMonth);
        context.IncrementLpLargeShipments(yearMonth);
        context.RegisterLpLargeFreeShipment(yearMonth, date);

        var transaction = new Transaction
        {
            IsValid = true,
            PackageSize = PackageSize.L,
            Provider = ShippingProvider.LP,
            OriginalPrice = 6.90m,
            PriceWithDiscount = 6.90m,
            Date = date,
            OriginalInput = date.Date.ToString() + PackageSize.L + ShippingProvider.LP
        };

        // Act
        var result = handler.ApplyDiscount(transaction, context);

        // Assert
        Assert.Equal(6.90m, result.OriginalPrice);
        Assert.Equal(0, result.Discount);
        Assert.Equal(6.90m, result.PriceWithDiscount);
    }
}