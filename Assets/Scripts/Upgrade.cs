using System;
using UnityEngine;

[Serializable]
public class Upgrade
{
    public string id;
    public string upgradeName;
    public string description;
    public string icon;
    public double baseCost;
    public double coinsPerSecond;
    public int owned;

    private const double costMultiplier = 1.15;

    public double GetCurrentCost()
    {
        return baseCost * Math.Pow(costMultiplier, owned);
    }

    public double GetTotalCoinsPerSecond()
    {
        return coinsPerSecond * owned;
    }

    public string GetDisplayCost()
    {
        return FormatNumber(GetCurrentCost());
    }

    public string GetDisplayProduction()
    {
        return FormatNumber(coinsPerSecond);
    }

    private string FormatNumber(double number)
    {
        if (number >= 1_000_000_000_000)
            return $"{number / 1_000_000_000_000:F2}T";
        if (number >= 1_000_000_000)
            return $"{number / 1_000_000_000:F2}B";
        if (number >= 1_000_000)
            return $"{number / 1_000_000:F2}M";
        if (number >= 1_000)
            return $"{number / 1_000:F2}K";
        return $"{number:F0}";
    }
}
