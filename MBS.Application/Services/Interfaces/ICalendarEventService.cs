
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.Application.Services.Interfaces;

public interface ICalendarEventService
{
    Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request);
    Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId,string accessToken, CalendarEventQueryParameters parameters);
    Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId);
    Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, UpdateCalendarEventRequestModel request);
    Task<BaseModel> DeleteCalendarEvent(string calendarEventId);
    // Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorIdPagination(string mentorId, int page, int size);

}