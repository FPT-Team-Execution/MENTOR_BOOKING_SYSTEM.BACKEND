
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;

namespace MBS.Application.Services.Interfaces;

public interface ICalendarEventService
{
    Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request);
    Task<BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>> GetCalendarEventsByMentorId(GetCalendarEventsRequestModel request);
    Task<BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>> UpdateCalendarEvent(UpdateCalendarEventRequestModel request);
    Task<BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>> DeleteCalendarEvent(DeleteCalendarEventRequestModel request);

}