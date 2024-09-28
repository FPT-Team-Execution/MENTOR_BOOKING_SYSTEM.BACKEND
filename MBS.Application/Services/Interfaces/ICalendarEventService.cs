
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;

namespace MBS.Application.Services.Interfaces;

public interface ICalendarEventService
{
    Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request);
    Task<BaseModel<GetCalendarEventsResponseModel>> GetCalendarEventsByMentorId(string mentorId);

    Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId,
        UpdateCalendarEventRequestModel request);
    Task<BaseModel<DeleteCalendarEventResponseModel>> DeleteCalendarEvent(string calendarEventId);

}