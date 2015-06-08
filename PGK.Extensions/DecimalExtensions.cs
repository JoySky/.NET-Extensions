using System;
using System.Collections.Generic;

/// <summary>
/// Contains extension methods for the <see cref="System.Decimal"/> class
/// </summary>
public static class DecimalExtenders
{
    /// <summary>
    /// Rounds the supplied decimal to the specified amount of decimal points
    /// </summary>
    /// <param name="val">The decimal to round</param>
    /// <param name="decimalPoints">The number of decimal points to round the output value to</param>
    /// <returns>A rounded decimal</returns>
    public static decimal RoundDecimalPoints(this decimal val, int decimalPoints)
    {
        return Math.Round(val, decimalPoints);
    }

    /// <summary>
    /// Rounds the supplied decimal value to two decimal points
    /// </summary>
    /// <param name="val">The decimal to round</param>
    /// <returns>A decimal value rounded to two decimal points</returns>
    public static decimal RoundToTwoDecimalPoints(this decimal val)
    {
        return Math.Round(val, 2);
    }

    /// <summary>
    /// Returns the absolute value of a <see cref="System.Decimal"/> number
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static decimal Abs(this decimal value)
    {
        return Math.Abs(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static IEnumerable<decimal> Abs(this IEnumerable<decimal> value)
    {
        foreach (decimal d in value)
            yield return d.Abs();
    }
}