using vinted_hw_assignment.Helpers;

namespace vinted_hw_assignment.Models;

public static class ShippingPrices
{
    private static Dictionary<ShippingProvider, Dictionary<PackageSize, decimal>> Prices { get; set; } 
        = ShippingPriceLoader.GetDataFromFile("shipping-info.json");

    public static decimal GetPrice(ShippingProvider provider, PackageSize packageSize)
    {
        if (Prices.ContainsKey(provider) && Prices[provider].ContainsKey(packageSize))
        {
            return Prices[provider][packageSize];
        }
        
        return 0;
    }

    public static decimal GetLowestPriceForSize(PackageSize packageSize)
    {
        return Prices.Values
            .Where(p => p.ContainsKey(packageSize))
            .Select(p => p[packageSize])
            .Min();
    }
}