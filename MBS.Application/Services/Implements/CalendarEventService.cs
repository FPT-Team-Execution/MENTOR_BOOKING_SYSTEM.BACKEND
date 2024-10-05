using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class CalendarEventService : BaseService<CalendarEventService>, ICalendarEventService
{

    public CalendarEventService(
       IUnitOfWork unitOfWork, ILogger<CalendarEventService> logger
        ) : base(unitOfWork, logger)
    {
    
    }
    public async Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
    {
        try
        {
            //check mentorId
            var mentor = await _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync(m => m.UserId == request.MentorId);
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
            var meeting = await _unitOfWork.GetRepository<Meeting>().SingleOrDefaultAsync(m => m.Id == request.MeetingId);
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
            await _unitOfWork.GetRepository<CalendarEvent>().InsertAsync(eventCreate);
            if(await _unitOfWork.CommitAsync() > 0)
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

    public async Task<BaseModel<GetCalendarEventsResponseModel>> GetCalendarEventsByMentorId(string mentorId)
    {
        try
        {
            var mentor = await _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync(m => m.UserId == mentorId);
            if (mentor == null)
            {
                return new BaseModel<GetCalendarEventsResponseModel>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            //find events by mentor
            var events = await _unitOfWork.GetRepository<CalendarEvent>().GetListAsync(e => e.MentorId == mentor.UserId);
  
            return new BaseModel<GetCalendarEventsResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetCalendarEventsResponseModel
                {
                    Events = events
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetCalendarEventsResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId)
    {
        try
        {
          
            var calendarEvent = await _unitOfWork.GetRepository<CalendarEvent>().SingleOrDefaultAsync(c => c.Id == calendarEventId);
            if (calendarEvent == null)
                return new BaseModel<CalendarEventResponseModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(calendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            
            return new BaseModel<CalendarEventResponseModel>
            {
                    Message = MessageResponseHelper.GetSuccessfully("event"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new CalendarEventResponseModel
                    {
                        CalendarEvent = calendarEvent
                    }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<CalendarEventResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, UpdateCalendarEventRequestModel request)
    {
        try
        {
            //check meeting Id
        var meeting = await _unitOfWork.GetRepository<Meeting>().SingleOrDefaultAsync(m => m.Id == request.MeetingId);
        if (meeting == null)
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = MessageResponseHelper.MeetingNotFound(request.MeetingId.ToString()),
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                
            };
        if (meeting.Status != MeetingStatusEnum.New)
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = MessageResponseHelper.InvalidMeetingSatus(meeting.Id.ToString()),
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                
            };
        //update calendar event
        var calendarEvent = await _unitOfWork.GetRepository<CalendarEvent>().SingleOrDefaultAsync(m => m.Id == calendarEventId);
        if(calendarEvent == null)
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = MessageResponseHelper.CalendarNotFound(calendarEventId),
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
        _unitOfWork.GetRepository<CalendarEvent>().UpdateAsync(calendarEvent);
        if (_unitOfWork.Commit() > 0)
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = MessageResponseHelper.UpdateSuccessfully("event"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new UpdateCalendarEventResponseModel
                {
                    Event = calendarEvent,
                }
            };
        return new BaseModel<UpdateCalendarEventResponseModel>
        {
            Message = MessageResponseHelper.UpdateFailed("event"),
            IsSuccess = false,
            StatusCode = StatusCodes.Status200OK,
        };
        }
        catch (Exception e)
        {
            return new BaseModel<UpdateCalendarEventResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<DeleteCalendarEventResponseModel>> DeleteCalendarEvent(string calendarEventId)
    {
        try
        {
            //check meeting status related to canlendar event ~ Cancled
            var calendarEvent =
                await _unitOfWork.GetRepository<CalendarEvent>().SingleOrDefaultAsync(
                    predicate: e => e.Id == calendarEventId, 
                    include: e => e.Include(x => x.Meeting));
            if (calendarEvent == null)
                return new BaseModel<DeleteCalendarEventResponseModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(calendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if (calendarEvent.Meeting!.Status != MeetingStatusEnum.Canceled)
                return new BaseModel<DeleteCalendarEventResponseModel>
                {
                    Message = MessageResponseHelper.InvalidMeetingSatus(calendarEvent.MeetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            
            //update calendarEvent to cancled (deleted)
            calendarEvent.Status = EventStatus.Cancleled;
            _unitOfWork.GetRepository<CalendarEvent>().UpdateAsync(calendarEvent);
            if(_unitOfWork.Commit() > 0)
                return new BaseModel<DeleteCalendarEventResponseModel>
                {
                    Message = "",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status204NoContent,
                    ResponseRequestModel = new (),
                };
            return new BaseModel<DeleteCalendarEventResponseModel>
            {
                Message = MessageResponseHelper.DeleteFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status204NoContent,
                ResponseRequestModel = new (),
            };
        }
        catch (Exception e)
        {
            return new BaseModel<DeleteCalendarEventResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorIdPagination(string mentorId, int page, int size)
    {
        try
        {
            var mentor = await _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync(m => m.UserId == mentorId);
            if (mentor == null)
            {
                return new BaseModel<Pagination<CalendarEvent>>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            //find events by mentor
            var events = await _unitOfWork.GetRepository<CalendarEvent>().GetPagingListAsync(
                predicate: e => e.MentorId == mentor.UserId,
                page: page,
                size: size
                );
  
            return new BaseModel<Pagination<CalendarEvent>>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = events
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<CalendarEvent>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}