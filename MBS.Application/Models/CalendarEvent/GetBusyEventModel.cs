namespace MBS.Application.Models.CalendarEvent;

public class GetBusyEventRequest
{
    public required string MentorId { get; set; }
    public required DateOnly Day { get; set; }
}

public class GetBusyEventResponse
{
    public List<BusyEventModel> Events { get; set; } = [];
}

public class BusyEventModel
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}