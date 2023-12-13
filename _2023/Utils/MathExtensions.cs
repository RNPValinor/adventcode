namespace _2023.Utils;

public static class Maths
{
    public static long Gcd(long number1, long number2)
    {
        if (number1 == 0 || number2 == 0) return number1 + number2;
        
        var absNumber1 = Math.Abs(number1);
        var absNumber2 = Math.Abs(number2);
        var biggerValue = Math.Max(absNumber1, absNumber2);
        var smallerValue = Math.Min(absNumber1, absNumber2);
        
        return Gcd(biggerValue % smallerValue, smallerValue);
    }
    
    public static long Lcm(long number1, long number2)
    {
        if (number1 == 0 || number2 == 0) return 0;
        
        var gcd = Gcd(number1, number2);
        return Math.Abs(number1 * number2) / gcd;
    }
}