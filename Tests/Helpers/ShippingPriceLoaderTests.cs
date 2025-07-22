using vinted_hw_assignment.Helpers;
using vinted_hw_assignment.Models;

namespace Tests.Helpers;

public class ShippingPriceLoaderTests
{
    [Fact]
    public void GetDataFromFile_ShouldReturnCorrectData()
    {
        //Arrange
        var filePath = "../../../ShippingInfoJsonFiles/shipping-info-valid.json";
        
        //Act
        var result = ShippingPriceLoader.GetDataFromFile(filePath);
        
        //Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey(ShippingProvider.LP));
        Assert.True(result.ContainsKey(ShippingProvider.MR));
        Assert.Equal(3, result[ShippingProvider.LP].Count);
        Assert.Equal(3, result[ShippingProvider.MR].Count);
        Assert.Equal(1.50m, result[ShippingProvider.LP][PackageSize.S]);
        Assert.Equal(4.90m, result[ShippingProvider.LP][PackageSize.M]);
        Assert.Equal(6.90m, result[ShippingProvider.LP][PackageSize.L]);
        Assert.Equal(2.00m, result[ShippingProvider.MR][PackageSize.S]);
        Assert.Equal(3.00m, result[ShippingProvider.MR][PackageSize.M]);
        Assert.Equal(4.00m, result[ShippingProvider.MR][PackageSize.L]);
    }
    
    [Fact]
    public void GetDataFromFile_InvalidPackageSize_ShouldIgnoreInvalidSize()
    {
        //Arrange
        var filePath = "../../../ShippingInfoJsonFiles/shipping-info-invalid-sizes.json";
        
        //Act
        var result = ShippingPriceLoader.GetDataFromFile(filePath);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey(ShippingProvider.LP));
        Assert.Equal(2, result[ShippingProvider.LP].Count);
        Assert.True(result[ShippingProvider.LP].ContainsKey(PackageSize.M));
        Assert.True(result[ShippingProvider.LP].ContainsKey(PackageSize.L));
        Assert.False(result[ShippingProvider.LP].ContainsKey(PackageSize.S));
    }
    
    [Fact]
    public void GetDataFromFile_InvalidShippingProvider_ShouldIgnoreInvalidProvider()
    {
        //Arrange
        var filePath = "../../../ShippingInfoJsonFiles/shipping-info-invalid-provider.json";
        
        //Act
        var result = ShippingPriceLoader.GetDataFromFile(filePath);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey(ShippingProvider.MR));
        Assert.Equal(3, result[ShippingProvider.MR].Count);
        Assert.True(result[ShippingProvider.MR].ContainsKey(PackageSize.S));
        Assert.True(result[ShippingProvider.MR].ContainsKey(PackageSize.M));
        Assert.True(result[ShippingProvider.MR].ContainsKey(PackageSize.L));
        Assert.False(result.ContainsKey(ShippingProvider.LP));
    }
}