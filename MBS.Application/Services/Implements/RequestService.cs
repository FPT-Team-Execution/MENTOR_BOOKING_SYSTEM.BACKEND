using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class RequestService : BaseService2<RequestService>, IRequestService
{
    private readonly IMentorRepository _mentorRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICalendarEventRepository _eventRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICalendarEventService _calendarEventService;
    private readonly IStudentRepository _studentRepository;
    
    public RequestService(
        IMentorRepository mentorRepository,
        IProjectRepository projectRepository,
        ICalendarEventRepository eventRepository,
        IRequestRepository requestRepository,
        IStudentRepository _studentRepository,
        UserManager<ApplicationUser> userManager,
        ILogger<RequestService> logger,
        IMapper mapper
        ) : base(logger, mapper)
    {
        _mentorRepository = mentorRepository;
        _projectRepository = projectRepository;
        _eventRepository = eventRepository;
        _userManager = userManager;
        _requestRepository = requestRepository;
        this._studentRepository = _studentRepository;
    }
    public async Task<BaseModel<Pagination<RequestResponseDto>>> GetRequests(GetRequestsPaginationRequest request)
    {
        try
        {
            var requests = await _requestRepository.GetRequestPaginationAsync(request.Page, request.Size, request.SortOrder);
            return new BaseModel<Pagination<RequestResponseDto>>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<RequestResponseDto>>(requests)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<RequestResponseDto>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<RequestResponseModel>> GetRequestById(Guid requestId)
    {
        try
        {
            var request = await _requestRepository.GetRequestById(requestId);
            if(request == null)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.RequestNotFound(requestId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            return new BaseModel<RequestResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("event"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new RequestResponseModel
                {
                    Request = _mapper.Map<RequestResponseDto>(request)
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<RequestResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>> CreateRequest(CreateRequestRequestModel request)
    {
        try
        {
            if (request.Start <= DateTime.Now || request.End <= DateTime.Now || request.Start >= request.End)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.InvalidInputParameterDetail("time range"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            //check mentor 
            var mentor = await _mentorRepository.GetByIdAsync(request.MentorId, "UserId");
            if(mentor == null)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(request.MentorId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            var dateRange = ConvertUtils.GetStartEndTime(request.Start);
            var events =  await _eventRepository.GetCalendarEventsByMentorIdAsync(request.MentorId, dateRange.Start, dateRange.End);

            var isOverlayed = IsOverlapping(request.Start, request.End, events.Where(x => x.Start >= DateTime.Now).ToList());
            if (isOverlayed)
            {
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.OverlayCalendar(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            
            //Check user ~ creater
            var user = await _userManager.FindByIdAsync(request.CreaterId);
            if(user == null)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(request.CreaterId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            
            // check student point
            switch (request.ProjectId)
            {
                case null:
                {
                    break;
                }
            }
            
            //Check project
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, "Id");
            if(project == null)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.ProjectNotFound(request.ProjectId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if(project.Status != ProjectStatusEnum.Activated)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.ProjectNotActivated(request.ProjectId.ToString()!),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            //Create request
            var newRequest = new Request()
            {
                Id = Guid.NewGuid(),
                //CalendarEventId = calendarEvent.Id,
                ProjectId = project.Id,
                CreaterId = user.Id,
                MentorId = request.MentorId,
                Start = request.Start,
                End = request.End,
                Title = request.Title,
                Status = RequestStatusEnum.Pending
            };
            var addResult = await _requestRepository.CreateAsync(newRequest);
            if(addResult)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("request"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateRequestResponseModel
					{
                        RequestId = newRequest.Id
					}
                };
            return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("request"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
    private bool IsOverlapping(DateTime start, DateTime end, List<CalendarEvent> events)
    {

        foreach (var item in events)
        {
            if(item.Start >= DateTime.Now)
            {
                // Check if the two intervals overlap
                if (start < item.End && end > item.Start)
                {
                    return true;
                }
            }  
        }
        return false;
    }


    public async Task<BaseModel<RequestResponseModel>> UpdateRequest(Guid requestId, UpdateRequestRequestModel requestModel)
    {
        try
        {
            //check request
            var request = await _requestRepository.GetByIdAsync(requestId, "Id");
            if (request == null)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.RequestNotFound(requestId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    
                };
            if (request.Status != RequestStatusEnum.Pending)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.InvalidRequestStatus(requestId.ToString(), nameof(RequestStatusEnum.Pending)),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    
                };
            //Check calendar event
            //var calendarEvent = await _eventRepository.GetEventByIdAsync(request.CalendarEventId);
            //if(calendarEvent == null)
            //    return new BaseModel<RequestResponseModel>
            //    {
            //        Message = MessageResponseHelper.NotFoundCalendar(requestModel.CalendarEventId),
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status404NotFound,
                    
            //    };
            //check calendar and meeting
            //if(calendarEvent.Start <= DateTime.Now)
            //    return new BaseModel<RequestResponseModel>
            //    {
            //        Message = MessageResponseHelper.CalendarInThePast(requestModel.CalendarEventId),
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
                    
            //    };
            //if(calendarEvent.Meeting != null && calendarEvent.Meeting.Status == MeetingStatusEnum.New)
            //    return new BaseModel<RequestResponseModel>
            //    {
            //        Message = MessageResponseHelper.BusyCalendar(requestModel.CalendarEventId),
            //        IsSuccess = false,
            //        StatusCode = StatusCodes.Status400BadRequest,
                    
            //    };
            
            //Update request
            //request.CalendarEventId = requestModel.CalendarEventId;
            request.Title = requestModel.Title;
            request.Status = requestModel.Status;
            var updateResult = _requestRepository.Update(request);
            if (updateResult)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("event"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new RequestResponseModel()
                    {
                        Request = _mapper.Map<RequestResponseDto>(request),
                    }
                };
            return new BaseModel<RequestResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<RequestResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
    
}