using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MBS.Application.Services.Implements;

public class CalendarEventService : ICalendarEventService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly ICalendarEventRepository _calendarEventRepository;
    private readonly IMentorRepository _mentorRepository;
    public CalendarEventService(
        IMeetingRepository meetingRepository,
        ICalendarEventRepository calendarEventRepository,
        IMentorRepository mentorRepository  
        )
    {
        _meetingRepository = meetingRepository;
        _calendarEventRepository = calendarEventRepository;
        _mentorRepository = mentorRepository;
    }
    public async Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
    {
        try
        {
            //check mentorId
            var mentor = await _mentorRepository.GetAsync(m => m.UserId == request.MentorId);
            if (mentor == null)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            
            //check meetingId
            var meeting = await _meetingRepository.GetAsync(m => m.Id == request.MeetingId);
            if (meeting == null)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(request.MeetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            if (meeting.Status != MeetingStatusEnum.Delayed || meeting.Status != MeetingStatusEnum.New)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.InvalidMeetingSatus(meeting.Id.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var eventCreate = new CalendarEvent()
            {
                Id = request.Id,  
                Status = request.Status,
                HtmlLink = request.HtmlLink,
                Created = request.Created,
                Updated = request.Updated,
                Summary = request.Summary,
                ICalUID = request.ICalUID,
                Start = request.Start,
                End = request.End,
                MentorId = request.MentorId,
                MeetingId = request.MeetingId,  
            };
            var addSuccess = await _calendarEventRepository.AddAsync(eventCreate);
            if(addSuccess)
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.CreateSuccessfully("event"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateCalendarResponseModel
                    {
                        CalendarEventId = eventCreate.Id,
                    }
                };
            return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };   
           
        }
        catch (Exception e)
        {
            return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>> GetCalendarEventsByMentorId(GetCalendarEventsRequestModel request)
    {
        try
        {
            var mentor = await _mentorRepository.GetAsync(m => m.UserId == request.MentorId);
            if (mentor == null)
            {
                return new BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            //find events by mentor
            var events = await _calendarEventRepository.GetAllAsync(e => e.MentorId == mentor.UserId);
  
            return new BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                RequestModel = request,
                ResponseModel = new GetCalendarEventsResponseModel
                {
                    Events = events
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>
            {
                Message = e.Message,
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
            };
        }
    }

    public async Task<BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>> UpdateCalendarEvent(UpdateCalendarEventRequestModel request)
    {
        try
        {
            //check meeting Id
        var meeting = await _meetingRepository.GetAsync(m => m.Id == request.MeetingId);
        if (meeting == null)
            return new BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>
            {
                Message = MessageResponseHelper.MeetingNotFound(request.MeetingId.ToString()),
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                
            };
        if (meeting.Status != MeetingStatusEnum.New)
            return new BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>
            {
                Message = MessageResponseHelper.InvalidMeetingSatus(meeting.Id.ToString()),
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                
            };
        //update calendar event
        var calendarEvent = await _calendarEventRepository.GetAsync(m => m.Id == request.CalendarEventId);
        if(calendarEvent == null)
            return new BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>
            {
                Message = MessageResponseHelper.CalendarNotFound(request.CalendarEventId),
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                
            };
        //TODO: call google calendar api to recheck event props
        //~
        
        calendarEvent.HtmlLink = request.newHtmlLink;
        calendarEvent.Description = request.Description;
        calendarEvent.Updated = request.Updated;
        if (request.Start != null)
            calendarEvent.Start = request.Start.Value;
        if (request.End != null)
            calendarEvent.End = request.End.Value;
        calendarEvent.MeetingId = request.MeetingId;
        var updateSuccess = await _calendarEventRepository.UpdateAsync(calendarEvent);
        if (updateSuccess)
            return new BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>
            {
                Message = MessageResponseHelper.UpdateSuccessfully("event"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                RequestModel = request,
                ResponseModel = new UpdateCalendarEventResponseModel
                {
                    Event = calendarEvent,
                }
            };
        return new BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>
        {
            Message = MessageResponseHelper.UpdateFailed("event"),
            IsSuccess = false,
            StatusCode = StatusCodes.Status500InternalServerError,
        };
        }
        catch (Exception e)
        {
            return new BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>
            {
                Message = MessageResponseHelper.UpdateFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
    }

    public async Task<BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>> DeleteCalendarEvent(DeleteCalendarEventRequestModel request)
    {
        try
        {
            //check meeting status related to canlendar event ~ Cancled
            var calendarEvent =
                await _calendarEventRepository.GetAsync(e => e.Id == request.calendarEventId, "Meeting");
            if (calendarEvent == null)
                return new BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(request.calendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if (calendarEvent.Meeting!.Status != MeetingStatusEnum.Canceled)
                return new BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>
                {
                    Message = MessageResponseHelper.InvalidMeetingSatus(calendarEvent.MeetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            
            //update calendarEvent to cancled (deleted)
            calendarEvent.Status = EventStatus.Cancleled;
            var deleteSuccess = await _calendarEventRepository.UpdateAsync(calendarEvent);
            if(deleteSuccess)
                return new BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>
                {
                    Message = "",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status204NoContent,
                    RequestModel = request,
                    ResponseModel = new (),
                };
            return new BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>
            {
                Message = MessageResponseHelper.DeleteFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
                RequestModel = request,
                ResponseModel = new (),
            };
        }
        catch (Exception e)
        {
            return new BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}