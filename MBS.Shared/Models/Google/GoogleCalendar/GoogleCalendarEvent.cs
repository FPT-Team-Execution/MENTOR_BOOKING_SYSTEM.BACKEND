namespace MBS.Shared.Models.Google;

public class GoogleCalendarEvent
{
    //main Id ~ Id
    public string Id { get; set; }
    public string Status { get; set; }
    public string HtmlLink { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string Summary { get; set; }
    //event Id in calendar
    public string ICalUID { get; set; }
    public EventTime Start { get; set; }
    public EventTime End { get; set; }
    //number of modification of this event
    public int Sequence { get; set; }
}
public class EventTime
{
    public DateTime DateTime { get; set; }
    //* default for now
    public string TimeZone { get; set; } = "Asia/Ho_Chi_Minh";
}