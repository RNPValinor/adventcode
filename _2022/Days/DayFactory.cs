namespace _2022.Days;

public static class DayFactory
{
    public static Day GetDay(int day)
    {
        return day switch
        {
            1 => new Day1(),
            2 => new Day2(),
            3 => new Day3(),
            4 => new Day4(),
            5 => new Day5(),
            _ => throw new ArgumentOutOfRangeException(nameof(day), $"Invalid or missing day: {day}")
        };
    }
}