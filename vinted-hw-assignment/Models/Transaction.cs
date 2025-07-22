namespace vinted_hw_assignment.Models;

public class Transaction
{
    //used enums to not allow invalid values to be set, also for readabiltity
    public PackageSize PackageSize { get; set; }
    
    public ShippingProvider Provider { get; set; }
    
    public decimal OriginalPrice { get; set; }
    
    public decimal PriceWithDiscount { get; set; }
    
    public decimal Discount { get; set; }
    
    public DateTime Date { get; set; }
    
    // stores original line from file, used if data cannot be parsed to enum
    public required string OriginalInput { get; set; }

    // is false if data from file cannot be parsed
    public bool IsValid { get; set; }

    public string GetDateYearAndMonth()
    {
        return Date.Year + "-" + Date.Month;
    }
}