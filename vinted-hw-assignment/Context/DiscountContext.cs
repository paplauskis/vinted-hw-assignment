namespace vinted_hw_assignment.Context;

// discount context is needed for flexibility and for easily adding new rules
public class DiscountContext
{
    public const decimal MaxMonthlyDiscount = 10.00m;
    
    public Dictionary<string, decimal> MonthlyDiscountTotals { get; } = new();
    
    public Dictionary<string, int> MonthlyLpLargeShipments { get; } = new();
    
    public Dictionary<string, List<DateTime>> MonthlyLpLargeFreeShipments { get; } = new();
    
    public void AddDiscount(string yearMonth, decimal discount)
    {
        MonthlyDiscountTotals[yearMonth] = MonthlyDiscountTotals.GetValueOrDefault(yearMonth, 0m) + discount;
    }
    
    public decimal GetRemainingMonthlyDiscount(string yearMonth)
    {
        var used = MonthlyDiscountTotals.GetValueOrDefault(yearMonth, 0m);
        return Math.Max(0, MaxMonthlyDiscount - used);
    }

    public void IncrementLpLargeShipments(string yearMonth)
    {
        MonthlyLpLargeShipments[yearMonth] = MonthlyLpLargeShipments.GetValueOrDefault(yearMonth, 0) + 1;
    }

    public int GetLpLargeShipmentCount(string yearMonth)
    {
        return MonthlyLpLargeShipments.GetValueOrDefault(yearMonth, 0);
    }

    public void RegisterLpLargeFreeShipment(string yearMonth, DateTime date)
    {
        if (!MonthlyLpLargeFreeShipments.ContainsKey(yearMonth))
            MonthlyLpLargeFreeShipments[yearMonth] = new ();
            
        if (!MonthlyLpLargeFreeShipments[yearMonth].Contains(date.Date))
        {
            MonthlyLpLargeFreeShipments[yearMonth].Add(date.Date);
        }
    }

    public bool IsLpLargeFreeShipmentForMonthValid(string yearMonth)
    {
        return MonthlyLpLargeFreeShipments.ContainsKey(yearMonth) && 
               MonthlyLpLargeFreeShipments[yearMonth].Any();
    }
}