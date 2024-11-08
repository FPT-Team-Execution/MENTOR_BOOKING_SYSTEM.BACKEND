
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.Application.Services.Interfaces;

public interface ICalendarEventService
{
    Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request);
    Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId,string accessToken, CalendarEventPaginationQueryParameters parameters);
    Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId);
    Task<BaseModel<GetBusyEventResponse, GetBusyEventRequest>> GetBusyEvent(GetBusyEventRequest request);
    Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request);
    Task<BaseModel> DeleteCalendarEvent(string calendarEventId);
    //create calendar with flow
    /*
        update request => accepted
        create calendar event
        create meeting
     */
    Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request);


}