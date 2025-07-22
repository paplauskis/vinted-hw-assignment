using System.Text.Json;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Helpers;

//used a file to store provider, size and pricing data for easier use 
public static class ShippingPriceLoader
{
    public static Dictionary<ShippingProvider, Dictionary<PackageSize, decimal>> GetDataFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }
        
        string json = File.ReadAllText(filePath);
        
        if (string.IsNullOrEmpty(json))
        {
            throw new InvalidDataException("File is empty.");
        }

        var rawData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(json);

        var result = new Dictionary<ShippingProvider, Dictionary<PackageSize, decimal>>();

        foreach (var providerPair in rawData!)
        {
            if (!Enum.TryParse(providerPair.Key, out ShippingProvider provider))
                continue;

            var sizeDict = new Dictionary<PackageSize, decimal>();
            foreach (var sizePair in providerPair.Value)
            {
                if (!Enum.TryParse(sizePair.Key, out PackageSize size))
                    continue;

                sizeDict[size] = sizePair.Value;
            }

            result[provider] = sizeDict;
        }

        return result;
    }
}