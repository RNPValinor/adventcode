namespace _2022.Days;

public class DayFactory
{
    public static Day GetDay(int day)
    {
        return day switch
        {
            1 => new Day1(),
            2 => new Day2(),
            3 => new Day3(),
            _ => throw new ArgumentOutOfRangeException(nameof(day), $"Invalid or missing day: {day}")
        };
    }
}