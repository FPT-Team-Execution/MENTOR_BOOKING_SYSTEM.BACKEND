namespace MBS.Shared.Utils;

public class ConvertUtils
{
    public string JoinAuthorizeRoles(params string[] roles)
    {
        return string.Join(',', roles);
    }
    public static (DateTime Start, DateTime End) GetStartEndTime(DateTime date)
    {
        // Start of the day: 00:00:00
        DateTime start = date.Date;

        // End of the day: 23:59:59.999
        DateTime end = date.Date.AddDays(1).AddMilliseconds(-1);

        return (start, end);
    }
    public static (DateTime Start, DateTime End) GetStartEndTime(DateOnly date)
    {
        // Start of the day: 00:00:00
        DateTime start = date.ToDateTime(TimeOnly.MinValue);

        // End of the day: 23:59:59.999
        DateTime end = date.ToDateTime(TimeOnly.MaxValue);

        return (start, end);
    }
    public static string FormatDateTime(DateTime dateTime, string format)
    {
        return dateTime.ToString(format);
    }

}