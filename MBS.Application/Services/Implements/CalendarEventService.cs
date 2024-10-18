using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class CalendarEventService : BaseService2<CalendarEventService>, ICalendarEventService
{
    private readonly IMentorRepository _mentorRepository;
    private readonly ICalendarEventRepository _calendarEventRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IGoogleService _googleService;
    public CalendarEventService(ILogger<CalendarEventService> logger, IMapper mapper,
        IMentorRepository mentorRepository,
        ICalendarEventRepository calendarRepository,
        IMeetingRepository meetingRepository,
        IGoogleService googleService
        ) : base(logger, mapper)
    {
       _mentorRepository = mentorRepository;
       _calendarEventRepository = calendarRepository;
       _meetingRepository = meetingRepository;
       _googleService = googleService;
    }
    public async Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
    {
        try
        {
            //check mentorId
            var mentor = await _mentorRepository.GetByIdAsync(request.MentorId, "UserId");
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
            // var meeting = await _unitOfWork.GetRepository<Meeting>().SingleOrDefaultAsync(m => m.Id == request.MeetingId);
            // if (meeting == null)
            // {
            //     return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
            //     {
            //         Message = MessageResponseHelper.MeetingNotFound(request.MeetingId.ToString()),
            //         IsSuccess = false,
            //         StatusCode = StatusCodes.Status404NotFound
            //     };
            // }
            // if (meeting.Status != MeetingStatusEnum.Delayed || meeting.Status != MeetingStatusEnum.New)
            // {
            //     return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
            //     {
            //         Message = MessageResponseHelper.InvalidMeetingSatus(meeting.Id.ToString()),
            //         IsSuccess = false,
            //         StatusCode = StatusCodes.Status400BadRequest
            //     };
            // }
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
            var addResult = await  _calendarEventRepository.CreateAsync(eventCreate);
            if(addResult)
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
                StatusCode = StatusCodes.Status500InternalServerError,
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
    

    public async Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId,string googleAccessToken ,CalendarEventQueryParameters parameters)
    {
        try
        {
            //check mentor
            var mentor = await _mentorRepository.GetMentorByIdAsync(mentorId);
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
            var events = await _calendarEventRepository.GetAllCalendarEventsByMentorIdAsync(mentorId, parameters);
            //get events from google calendar
            var gRequest = new GetGoogleCalendarEventsRequest
            {   
                Email = mentor.User.Email,
                AccessToken = googleAccessToken,
                TimeMin = parameters.StartTime,
                TimeMax = parameters.EndTime,
            };
            var googleResponse = await _googleService.ListEvents(gRequest);
            if(!googleResponse.IsSuccess){
                return new BaseModel<Pagination<CalendarEvent>>
                {
                    Message = ((GoogleErrorResponse)googleResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleResponse).Error.Code
                };
            }
            // filter new and old
            var newEventsFromGoogle = FilterNewEvents(mentorId, (List<CalendarEvent>)events.Items, ((GetGoogleCalendarEventsResponse)googleResponse).Items);
            if (newEventsFromGoogle.Any())
            {
                var addRangeResult = await _calendarEventRepository.CreateRangeAsync(newEventsFromGoogle);
                if(!addRangeResult){
                    return new BaseModel<Pagination<CalendarEvent>>
                    {
                        Message = MessageResponseHelper.CreateFailed("events"),
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
                }
            }
            var asyncEvents = await _calendarEventRepository.GetAllCalendarEventsByMentorIdAsync(mentorId, parameters);
            return new BaseModel<Pagination<CalendarEvent>>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = asyncEvents
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

    private List<CalendarEvent> FilterNewEvents(string mentorId, List<CalendarEvent> localEvents, List<GoogleCalendarEvent> googleEvents)
    {
        // Initialize dictionary with local events
        var localEventDictionary = localEvents.ToDictionary(e => e.Id, e => e);

        // Filter new events from Google Calendar that don't exist in the local events
        var newEvents = googleEvents
            .Where(googleEvent => !localEventDictionary.ContainsKey(googleEvent.Id))
            .Select(googleEvent => new CalendarEvent
            {
                Id = googleEvent.Id,
                HtmlLink = googleEvent.HtmlLink,
                Summary = googleEvent.Summary,
                Description = string.Empty,
                ICalUID = googleEvent.ICalUID,
                Created = googleEvent.Created,
                Updated = googleEvent.Updated,
                MeetingId = null,
                MentorId = mentorId,
                Start = googleEvent.Start.DateTime,
                End = googleEvent.End.DateTime,
                Status = (EventStatus)Enum.Parse(typeof(EventStatus), googleEvent.Status)
            })
            .ToList();

        return newEvents;
    }

    public async Task<BaseModel<CalendarEventResponseModel>> GetCalendarEventId(string calendarEventId)
    {
        try
        {
            var calendarEvent = await _calendarEventRepository.GetByIdAsync(calendarEventId, "Id");
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
                var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");
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
        var calendarEvent = await _calendarEventRepository.GetByIdAsync(calendarEventId, "Id");
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
        var updateResult = _calendarEventRepository.Update(calendarEvent);
        if (updateResult)
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
            StatusCode = StatusCodes.Status500InternalServerError,
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

    public async Task<BaseModel> DeleteCalendarEvent(string calendarEventId)
    {
        try
        {
            //check meeting status related to canlendar event ~ Cancled
            var calendarEvent =
                await _calendarEventRepository.GetByIdAsync(calendarEventId);
            if (calendarEvent == null)
                return new BaseModel
                {
                    Message = MessageResponseHelper.CalendarNotFound(calendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            // if (calendarEvent.Meeting!.Status != MeetingStatusEnum.Canceled)
            //     return new BaseModel<DeleteCalendarEventResponseModel>
            //     {
            //         Message = MessageResponseHelper.InvalidMeetingSatus(calendarEvent.MeetingId.ToString()),
            //         IsSuccess = false,
            //         StatusCode = StatusCodes.Status400BadRequest,
            //     };
            
            //update calendarEvent to cancled (deleted)
            calendarEvent.Status = EventStatus.Cancleled;
            var updateResult = _calendarEventRepository.Update(calendarEvent);
            if(updateResult)
                return new BaseModel
                {
                    Message = "",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status204NoContent,
                };
            return new BaseModel
            {
                Message = MessageResponseHelper.DeleteFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        catch (Exception e)
        {
            return new BaseModel
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    // public async Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorIdPagination(string mentorId, int page, int size)
    // {
    //     try
    //     {
    //         var mentor = await _mentorRepository.GetByIdAsync(mentorId, "UserId");
    //         if (mentor == null)
    //         {
    //             return new BaseModel<Pagination<CalendarEvent>>
    //             {
    //                 Message = MessageResponseHelper.UserNotFound(),
    //                 IsSuccess = false,
    //                 StatusCode = StatusCodes.Status404NotFound,
    //             };
    //         }
    //         //find events by mentor
    //         var events = await _unitOfWork.GetRepository<CalendarEvent>().GetPagingListAsync(
    //             predicate: e => e.MentorId == mentor.UserId,
    //             page: page,
    //             size: size
    //             );
    //
    //         return new BaseModel<Pagination<CalendarEvent>>
    //         {
    //             Message = MessageResponseHelper.GetSuccessfully("events"),
    //             IsSuccess = true,
    //             StatusCode = StatusCodes.Status200OK,
    //             ResponseRequestModel = events
    //         };
    //     }
    //     catch (Exception e)
    //     {
    //         return new BaseModel<Pagination<CalendarEvent>>
    //         {
    //             Message = e.Message,
    //             IsSuccess = false,
    //             StatusCode = StatusCodes.Status500InternalServerError,
    //         };
    //     }
    // }
}