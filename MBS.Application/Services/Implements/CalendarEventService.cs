using AutoMapper;
using Azure.Core;
using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Models.Google;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Models.Google.GoogleMeeting.Response;
using MBS.Shared.Services.Interfaces;
using MBS.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Transactions;

namespace MBS.Application.Services.Implements;

public class CalendarEventService : BaseService2<CalendarEventService>, ICalendarEventService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMentorRepository _mentorRepository;
    private readonly ICalendarEventRepository _calendarEventRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IGoogleService _googleService;
    public CalendarEventService(ILogger<CalendarEventService> logger, IMapper mapper,
        IRequestRepository requestRepository,
        IMentorRepository mentorRepository,
        ICalendarEventRepository calendarRepository,
        IMeetingRepository meetingRepository,
        IGoogleService googleService
        ) : base(logger, mapper)
    {
        _requestRepository = requestRepository;
        _mentorRepository = mentorRepository;
        _calendarEventRepository = calendarRepository;
        _meetingRepository = meetingRepository;
        _googleService = googleService;
    }
    public async Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>> CreateCalendarEvent(CreateCalendarRequestModel request)
    {
        try
        {
            //* get project from meeting Id
            var meetingDetail = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");

            if (meetingDetail == null)
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(request.MeetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            //* find project by meetingId
            var project = await _requestRepository.GetRequestById(meetingDetail.RequestId);

            var startDatetime = DateTime.Parse(request.Start);
            var endDatetime = DateTime.Parse(request.End);

            //check mentorId
            var mentor = await _mentorRepository.GetMentorByIdAsync(request.MentorId);
            if (mentor == null)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            //find overlayed events
            var freeBusyRequest = new FreeBusyParamters()
            {
                Email = mentor.User.Email,
                AccessToken = request.AccessToken,
                Day = startDatetime,
            };

            var freeBusyResponse = await _googleService.GetFreeBusyPeriod(freeBusyRequest);

            var isOverlayed = IsOverlapping(startDatetime, endDatetime, ((FreeBusyResponse)freeBusyResponse).Calendars[mentor.User.Email].Busy);

            if (isOverlayed)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = MessageResponseHelper.OverlayCalendar(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //create event on google calendar
            var createGEventRequest = new CreateGoogleCalendarEventRequest()
            {
                Summary = "Meeting",
                Description = $"You have meeting with project: {project.Title.ToUpper()}",
                Start = startDatetime,
                End = endDatetime,
                TimeZone = "Asia/Ho_Chi_Minh"
            };
            var googleEventResponse = await _googleService.InsertEvent(mentor.User.Email, request.AccessToken, createGEventRequest);
            if (!googleEventResponse.IsSuccess)
            {
                return new BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>
                {
                    Message = ((GoogleErrorResponse)googleEventResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleEventResponse).Error.Code
                };
            }

            var googleEvent = ((GoogleCalendarEvent)googleEventResponse);
            //add new calendar event
            var eventCreate = new CalendarEvent()
            {
                Id = googleEvent.Id,
                Status = (EventStatus)Enum.Parse(typeof(EventStatus), googleEvent.Status, ignoreCase: true),
                Description = $"You have meeting with project: {project.Title.ToUpper()}",
                HtmlLink = googleEvent.HtmlLink,
                Created = googleEvent.Created,
                Updated = googleEvent.Updated,
                Summary = googleEvent.Summary,
                ICalUID = googleEvent.ICalUID,
                Start = googleEvent.Start.DateTime,
                End = googleEvent.End.DateTime,
                MentorId = request.MentorId,
                MeetingId = request.MeetingId,
            };
            var addResult = await _calendarEventRepository.CreateAsync(eventCreate);
            if (addResult)
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
    //private bool IsOverlapping(DateTime start, DateTime end, List<BusySlot> busySlots)
    //{
    //    //DateTime startFormated = DateTime.ParseExact(start.ToString(), "HH:mm", CultureInfo.InvariantCulture);
    //    //DateTime time12 = DateTime.ParseExact(time12Hour, "h:mm tt", CultureInfo.InvariantCulture);
    //    //DateTime endFormated = DateTime.ParseExact(start.ToString(), "HH:mm", CultureInfo.InvariantCulture)

    //    foreach (var slot in busySlots)
    //    {
    //        var startTime = DateTime.Parse(slot.End);

    //        var endTime = DateTime.Parse(slot.End);
    //        // Check if the two intervals overlap
    //        if (start < endTime && end > startTime)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    private bool IsOverlapping(DateTime start, DateTime end, List<BusySlot> busySlots)
    {
        // Convert the start and end times to DateTimeOffset, assuming they are in the same timezone (no offset)
        DateTimeOffset startDateTimeOffset = new DateTimeOffset(start);
        DateTimeOffset endDateTimeOffset = new DateTimeOffset(end);

        foreach (var slot in busySlots)
        {
            // Parse the end and start times of the busy slot using DateTimeOffset to account for time zone
            DateTimeOffset busyStartTime = DateTimeOffset.Parse(slot.Start);
            DateTimeOffset busyEndTime = DateTimeOffset.Parse(slot.End);

            // Check if the two intervals overlap
            // An overlap occurs when start is before the busy slot end time and end is after the busy slot start time
            if (startDateTimeOffset < busyEndTime && endDateTimeOffset > busyStartTime)
            {
                return true;
            }
        }
        return false;
    }


    public async Task<BaseModel<Pagination<CalendarEvent>>> GetCalendarEventsByMentorId(string mentorId, string googleAccessToken, CalendarEventPaginationQueryParameters parameters)
    {
        try
        {
            var startDatetime = DateTime.Parse(parameters.StartTime);
            var endDatetime = DateTime.Parse(parameters.EndTime);

            //check time
            if (startDatetime >= endDatetime)
            {
                return new BaseModel<Pagination<CalendarEvent>>
                {
                    Message = MessageResponseHelper.InvalidInputParameterDetail("starttime and endtime"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
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
            var events = await _calendarEventRepository.GetCalendarEventsByMentorIdPaginationAsync(
                mentorId, startDatetime, endDatetime, parameters.SortBy!, parameters.Page, parameters.Size);
            //get events from google calendar
            var gRequest = new GetGoogleCalendarEventsRequest
            {
                Email = mentor.User.Email!,
                AccessToken = googleAccessToken,
                TimeMin = startDatetime,
                TimeMax = endDatetime,
            };
            var googleResponse = await _googleService.ListEvents(gRequest);
            if (!googleResponse.IsSuccess)
            {
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
                if (!addRangeResult)
                {
                    return new BaseModel<Pagination<CalendarEvent>>
                    {
                        Message = MessageResponseHelper.CreateFailed("events"),
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
                }
            }
            var asyncEvents = await _calendarEventRepository.GetCalendarEventsByMentorIdPaginationAsync(
                mentorId, startDatetime, endDatetime, parameters.SortBy, parameters.Page, parameters.Size);
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
                    Message = MessageResponseHelper.NotFoundCalendar(calendarEventId),
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

    public async Task<BaseModel<GetBusyEventResponse, GetBusyEventRequest>> GetBusyEvent(GetBusyEventRequest request)
    {
        try
        {
            var mentor = await _mentorRepository.GetByIdAsync(request.MentorId, "UserId");
            if (mentor == null)
            {
                return new BaseModel<GetBusyEventResponse, GetBusyEventRequest>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            var (start, end) = ConvertUtils.GetStartEndTime(request.Day);
            var events = await _calendarEventRepository.GetCalendarEventsByMentorIdAsync(request.MentorId, start, end);
            var busyEventsInDay = _mapper.Map<IEnumerable<BusyEventModel>>(events);
            return new BaseModel<GetBusyEventResponse, GetBusyEventRequest>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                RequestModel = request,
                ResponseModel = new GetBusyEventResponse
                {
                    Events = busyEventsInDay.ToList()
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetBusyEventResponse, GetBusyEventRequest>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<UpdateCalendarEventResponseModel>> UpdateCalendarEvent(string calendarEventId, string accessToken, UpdateCalendarEventRequestModel request)
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
            var calendarEvent = await _calendarEventRepository.GetEventByIdAsync(calendarEventId);
            if (calendarEvent == null)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = MessageResponseHelper.NotFoundCalendar(calendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,

                };
            //TODO: call google calendar api to recheck event props
            //~
            var updateGEventRequets = new UpdateGoogleCalendarEventRequest()
            {
                Start = request.Start.Value,
                End = request.End.Value,
                TimeZone = "Asia/Ho_Chi_Minh"
            };

            var googleUpdateResponse = await _googleService.UpdateEvent(
                eventId: calendarEventId,
                email: calendarEvent.Mentor.UserId,
                accessToken: accessToken,
                updateRequest: updateGEventRequets
                );
            if (!googleUpdateResponse.IsSuccess)
                return new BaseModel<UpdateCalendarEventResponseModel>
                {
                    Message = ((GoogleErrorResponse)googleUpdateResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleUpdateResponse).Error.Code
                };
            GoogleCalendarEvent googleCalendarEventUpdated = (GoogleCalendarEvent)googleUpdateResponse;
            //update local events
            calendarEvent.HtmlLink = googleCalendarEventUpdated.HtmlLink;
            calendarEvent.Description = request.Description;
            calendarEvent.Summary = googleCalendarEventUpdated.Summary;
            calendarEvent.ICalUID = googleCalendarEventUpdated.ICalUID;
            calendarEvent.Updated = googleCalendarEventUpdated.Updated;
            // if (request.Start != null)
            //     calendarEvent.Start = request.Start.Value;
            // if (request.End != null)
            //     calendarEvent.End = request.End.Value;
            calendarEvent.Start = googleCalendarEventUpdated.Start.DateTime;
            calendarEvent.End = googleCalendarEventUpdated.End.DateTime;
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
                await _calendarEventRepository.GetEventByIdAsync(calendarEventId);
            if (calendarEvent == null)
                return new BaseModel
                {
                    Message = MessageResponseHelper.NotFoundCalendar(calendarEventId),
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
            if (updateResult)
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

    public async Task<BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>> CreateCalendarEventOnelFlow(CreateCalendarEventOneFlowRequest request)
    {
        try
        {
            //* get  request 
            var requestFound = await _requestRepository.GetRequestById(request.RequestId);

            if (requestFound == null)
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = MessageResponseHelper.RequestNotFound(request.RequestId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            if (requestFound.Status != RequestStatusEnum.Pending)
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = MessageResponseHelper.InvalidRequestStatus(request.RequestId.ToString(), "valid status to update"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            var startDatetime = DateTime.Parse(request.Start);
            var endDatetime = DateTime.Parse(request.End);

            //check mentorId
            var mentor = await _mentorRepository.GetMentorByIdAsync(request.MentorId);
            if (mentor == null)
            {
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            //find overlayed events
            var freeBusyRequest = new FreeBusyParamters()
            {
                Email = mentor.User.Email,
                AccessToken = request.AccessToken,
                Day = startDatetime,
            };

            var freeBusyResponse = await _googleService.GetFreeBusyPeriod(freeBusyRequest);

            var isOverlayed = IsOverlapping(startDatetime, endDatetime, ((FreeBusyResponse)freeBusyResponse).Calendars[mentor.User.Email].Busy);

            if (isOverlayed)
            {
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = MessageResponseHelper.OverlayCalendar(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //* create event on google calendar
            var createGEventRequest = new CreateGoogleCalendarEventRequest()
            {
                Summary = $"Meeting {(request.IsOnline ? "ONLINE" : "OFFLINE")}",
                Description = $"You have meeting with project: {requestFound.Project.Title.ToUpper()}",
                Start = startDatetime,
                End = endDatetime,
                TimeZone = "Asia/Ho_Chi_Minh"
            };
            var googleEventResponse = await _googleService.InsertEventWithGoogleMeetCreate(
                email: mentor.User.Email,
                accessToken: request.AccessToken,
                createRequest: createGEventRequest,
                location: request.Location,
                isOnline: request.IsOnline
                );
            if (!googleEventResponse.IsSuccess)
            {
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = ((GoogleErrorResponse)googleEventResponse).Error.Message,
                    IsSuccess = false,
                    StatusCode = ((GoogleErrorResponse)googleEventResponse).Error.Code
                };
            }
            var googleEvent = ((GoogleCalendarEvent)googleEventResponse);

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //update request to accepted
                requestFound.Status = RequestStatusEnum.Accepted;
                _requestRepository.Update(requestFound);
                //*create meeting 
                var googleMeetingUrl = string.Empty;
                if (request.IsOnline)
                {
                    GoogleResponse googleMeetingResponse = await _googleService.CreateMeeting(request.AccessToken);
                    if (!googleMeetingResponse.IsSuccess)
                        return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                        {
                            Message = ((GoogleErrorResponse)googleMeetingResponse).Error.Message,
                            IsSuccess = false,
                            StatusCode = ((GoogleErrorResponse)googleMeetingResponse).Error.Code
                        };
                    googleMeetingUrl = ((GoogleMeetingResponse)googleMeetingResponse).MeetingUri;
                }
                var newMeeting = new Meeting()
                {
                    Id = Guid.NewGuid(),
                    RequestId = requestFound.Id,
                    Description = request.Description,
                    Location = googleEvent.Location,
                    MeetUp = googleMeetingUrl,
                    Status = MeetingStatusEnum.New
                };
                //create meeting
                await _meetingRepository.CreateAsync(newMeeting);
                //add new calendar event
                var eventCreate = new CalendarEvent()
                {
                    Id = googleEvent.Id,
                    Status = (EventStatus)Enum.Parse(typeof(EventStatus), googleEvent.Status, ignoreCase: true),
                    Description = $"You have meeting with project: {requestFound.Project.Title.ToUpper()}",
                    HtmlLink = googleEvent.HtmlLink,
                    Created = googleEvent.Created,
                    Updated = googleEvent.Updated,
                    Summary = googleEvent.Summary,
                    ICalUID = googleEvent.ICalUID,
                    Start = googleEvent.Start.DateTime,
                    End = googleEvent.End.DateTime,
                    MentorId = request.MentorId,
                    MeetingId = newMeeting.Id,
                };
                var addResult = await _calendarEventRepository.CreateAsync(eventCreate);
                if (addResult)
                {
                    transactionScope.Complete();
                    return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                    {
                        Message = MessageResponseHelper.CreateSuccessfully("event"),
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        RequestModel = request,
                        ResponseModel = new CreateCalendarEventOneFlowResponse
                        {
                            CalendarEventId = eventCreate.Id,
                            MeetingId = newMeeting.Id
                        }
                    };
                }
                
                return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
                {
                    Message = MessageResponseHelper.CreateFailed("event"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
                
        }
        catch (Exception e)
        {
            return new BaseModel<CreateCalendarEventOneFlowResponse, CreateCalendarEventOneFlowRequest>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }

    }
}