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
            6 => new Day6(),
            7 => new Day7(),
            8 => new Day8(),
            9 => new Day9(),
            10 => new Day10(),
            _ => throw new ArgumentOutOfRangeException(nameof(day), $"Invalid or missing day: {day}")
        };
    }
}