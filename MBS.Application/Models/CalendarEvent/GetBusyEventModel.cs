using System.ComponentModel.DataAnnotations;

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
    [ RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$")]
    public DateTime Start { get; set; }
    [ RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$")]
    public DateTime End { get; set; }
}