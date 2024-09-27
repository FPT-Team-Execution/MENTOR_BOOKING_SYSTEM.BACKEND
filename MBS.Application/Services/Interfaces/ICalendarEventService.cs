
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;

namespace MBS.Application.Services.Interfaces;

public interface ICalendarEventService
{
    Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request);
    // Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> GetCalendarEvents(CreateCalendarRequestModel request);

}