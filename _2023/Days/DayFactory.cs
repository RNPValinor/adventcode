namespace _2023.Days;

public static class DayFactory
{
    public static Day GetDay(int day)
    {
        var type = Type.GetType($"_2023.Days.Day{day}");

        if (type is null)
            throw new ArgumentOutOfRangeException($"Invalid or missing day: {day}");

        var dayObj = Activator.CreateInstance(type);

        if (dayObj is Day dayCls)
            return dayCls;

        throw new ApplicationException("Failed to instantiate correct day");
    }
}