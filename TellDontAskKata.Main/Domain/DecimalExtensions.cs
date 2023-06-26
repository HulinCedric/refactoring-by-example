using System;

namespace TellDontAskKata.Main.Domain;

public static class DecimalExtensions
{
    public static decimal Round(this decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
}