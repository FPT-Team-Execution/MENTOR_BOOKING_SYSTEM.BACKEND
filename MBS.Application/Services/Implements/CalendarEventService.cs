using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Application.Services.Interfaces;

namespace MBS.Application.Services.Implements;

public class CalendarEventService : ICalendarEventService
{
    public Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
    {
        throw new NotImplementedException();
    }
}