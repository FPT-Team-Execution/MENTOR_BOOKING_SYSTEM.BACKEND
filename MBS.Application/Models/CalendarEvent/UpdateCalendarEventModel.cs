using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.CalendarEvent;

public class UpdateCalendarEventRequestModel
{
    // [Required]
    // [Description("new url's event after updating from Google Calendar")]
    // public string newHtmlLink { get; set; }
    [Required]
    [Description("new description's event after updating from Google Calendar")]
    public string Description { get; set; } = default!;
    // [Required]
    // [Description("new updated time's event after updating from Google Calendar")]
    // public DateTime Updated { get; set; }
    [Required, RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}[+-]\d{2}:\d{2}$")]
    [Description("new updated Start time's event after updating from Google Calendar")]
    public DateTime? Start { get; set; }
    [Required, RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}[+-]\d{2}:\d{2}$")]
    [Description("new updated End time's event after updating from Google Calendar")]
    public DateTime? End { get; set; }
    [Required]
    public Guid MeetingId { get; set; }
}
public class UpdateCalendarEventResponseModel
{
    public Core.Entities.CalendarEvent Event {
        get;
        set;
    }
}