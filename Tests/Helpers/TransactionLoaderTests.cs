using vinted_hw_assignment.Helpers;
using vinted_hw_assignment.Models;

namespace Tests.Helpers;

public class TransactionLoaderTests
{
    [Theory]
    [InlineData("2025-01-10 S LP")]
    [InlineData("2025-02-15 S MR")]
    [InlineData("2025-03-20 M LP")]
    [InlineData("2025-04-25 M MR")]
    [InlineData("2025-05-30 L LP")]
    [InlineData("2025-06-05 L MR")]
    public void ParseTransaction_ValidLine_ShouldReturnValidTransaction(string line)
    {
        // Act
        var transaction = TransactionLoader.ParseTransaction(line);

        // Assert
        Assert.True(transaction.IsValid);
        Assert.Equal(line, transaction.OriginalInput);
    }
    
    [Fact]
    public void ParseTransaction_ValidLine_ShouldParseTransactionCorrectly()
    {
        //Assert
        string line = "2016-07-20 S LP";
        
        // Act
        var transaction = TransactionLoader.ParseTransaction(line);

        // Assert
        Assert.True(transaction.IsValid);
        Assert.Equal(PackageSize.S, transaction.PackageSize);
        Assert.Equal(ShippingProvider.LP, transaction.Provider);
        Assert.Equal(new DateTime(2016, 7, 20), transaction.Date);
        Assert.Equal(line, transaction.OriginalInput);
        Assert.Equal(0.00m, transaction.OriginalPrice);
        Assert.Equal(0.00m, transaction.PriceWithDiscount);
    }
    
    [Theory]
    [InlineData("2038-11-23 L LP")]
    [InlineData("2022-11-23MMR")]
    [InlineData("2022-01-13 S-LP")]
    [InlineData("2022-02-21_S-MR")]
    [InlineData("2022-11-02")]
    [InlineData("2022-01-01 S PP")]
    [InlineData("2022-01-01 S")]
    [InlineData("2022-01-01 s LP")]
    [InlineData("2022-01-01 M mr")]
    [InlineData("L LP")]
    [InlineData("")]
    [InlineData(" ")]
    public void ParseTransaction_InvalidLine_ShouldReturnInvalidTransaction(string line)
    {
        // Act
        var transaction = TransactionLoader.ParseTransaction(line);
        var invalidLine = line + " Ignored";

        // Assert
        Assert.False(transaction.IsValid);
        Assert.Equal(invalidLine, transaction.OriginalInput);
    }
    
    [Fact]
    public void GetDataFromFile_ReturnsCorrectTransactions()
    {
        // Arrange
        var filePath = "../../../TransactionTxtFiles/transactions.txt";

        // Act
        var transactions = TransactionLoader.GetDataFromFile(filePath);

        // Assert
        Assert.Equal(4, transactions.Count);
        Assert.True(transactions[0].IsValid);
        Assert.True(transactions[1].IsValid);
        Assert.False(transactions[2].IsValid);
        Assert.True(transactions[3].IsValid);
    }
}