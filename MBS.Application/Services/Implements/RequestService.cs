using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class RequestService : BaseService2<RequestService>, IRequestService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICalendarEventRepository _eventRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly UserManager<ApplicationUser> _userManager;
<<<<<<< HEAD
    private IRequestRepository _requestRepository;
    private ICalendarEventRepository _calendarEventRepository;

    public RequestService(IUnitOfWork unitOfWork, IRequestRepository requestRepository, ICalendarEventRepository calendarEventRepository, ILogger<RequestService> logger,
=======
    
    public RequestService(
        IProjectRepository projectRepository,
        ICalendarEventRepository eventRepository,
        IRequestRepository requestRepository,
>>>>>>> develop
        UserManager<ApplicationUser> userManager,
        ILogger<RequestService> logger,
        IMapper mapper
        ) : base(logger, mapper)
    {
        _projectRepository = projectRepository;
        _eventRepository = eventRepository;
        _userManager = userManager;
        _requestRepository = requestRepository;
<<<<<<< HEAD
        _calendarEventRepository = calendarEventRepository;
=======
>>>>>>> develop
    }
    public async Task<BaseModel<Pagination<RequestResponseDto>>> GetRequests(GetRequestsPaginationRequest request)
    {
        try
        {
<<<<<<< HEAD
            var requests = await _requestRepository.GetPagedListAsync(
                page: page,
                size: size
                );
=======
            var requests = await _requestRepository.GetRequestPaginationAsync(request.Page, request.Size, request.SortOrder);
>>>>>>> develop
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
<<<<<<< HEAD
            //var request = await _unitOfWork.GetRepository<Request>().SingleOrDefaultAsync(r => r.Id == requestId);
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
            if (request == null)
=======
            var request = await _requestRepository.GetRequestById(requestId);
            if(request == null)
>>>>>>> develop
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
            //check calendar event
<<<<<<< HEAD
            var calendarEvent = await _calendarEventRepository.GetCalendarEventByIdAsync(requestmodel.CalendarEventId);
            if (calendarEvent == null)
=======
            var calendarEvent = await _eventRepository.GetByIdAsync(request.CalendarEventId, "Id");
            if(calendarEvent == null)
>>>>>>> develop
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.NotFoundCalendar(request.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //Check user ~ creater
<<<<<<< HEAD
            var user = await _userManager.FindByIdAsync(requestmodel.CreaterId);
            if (user == null)
=======
            var user = await _userManager.FindByIdAsync(request.CreaterId);
            if(user == null)
>>>>>>> develop
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(request.CreaterId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            //Check project
<<<<<<< HEAD
            var project = await _unitOfWork.GetRepository<Project>().SingleOrDefaultAsync(p => p.Id == requestmodel.ProjectId);
            if (project == null)
=======
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, "Id");
            if(project == null)
>>>>>>> develop
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.ProjectNotFound(request.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if (project.Status != ProjectStatusEnum.Activated)
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
                CalendarEventId = calendarEvent.Id,
                ProjectId = project.Id,
                CreaterId = user.Id,
                Title = request.Title,
                Status = RequestStatusEnum.Pending
            };
<<<<<<< HEAD
            await _requestRepository.CreateAsync(newRequest);
            //if (await _unitOfWork.CommitAsync() > 0)
=======
            var addResult = await _requestRepository.CreateAsync(newRequest);
            if(addResult)
>>>>>>> develop
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
<<<<<<< HEAD
            //return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
            //{
            //    Message = MessageResponseHelper.CreateFailed("request"),
            //    IsSuccess = false,
            //    StatusCode = StatusCodes.Status200OK,
            //};
=======
            return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("request"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
>>>>>>> develop
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

    public async Task<BaseModel<RequestResponseModel>> UpdateRequest(Guid requestId, UpdateRequestRequestModel requestModel)
    {
        try
        {
            //check request
<<<<<<< HEAD
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
=======
            var request = await _requestRepository.GetByIdAsync(requestId, "Id");
>>>>>>> develop
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
<<<<<<< HEAD
            var calendarEvent = await _calendarEventRepository.GetCalendarEventAsync(requestModel.CalendarEventId);
            if (calendarEvent == null)
=======
            var calendarEvent = await _eventRepository.GetEventByIdAsync(request.CalendarEventId);
            if(calendarEvent == null)
>>>>>>> develop
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.NotFoundCalendar(requestModel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,

                };
            //check calendar and meeting
            if (calendarEvent.Start <= DateTime.Now)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.CalendarInThePast(requestModel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,

                };
<<<<<<< HEAD
            if (calendarEvent.Meeting.Status == MeetingStatusEnum.New)
=======
            if(calendarEvent.Meeting != null && calendarEvent.Meeting.Status == MeetingStatusEnum.New)
>>>>>>> develop
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.BusyCalendar(requestModel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,

                };
<<<<<<< HEAD
            //TODO: call google calendar api to recheck event props
            //~

=======
            
>>>>>>> develop
            //Update request
            request.CalendarEventId = requestModel.CalendarEventId;
            request.Title = requestModel.Title;
            request.Status = requestModel.Status;
<<<<<<< HEAD

            //_unitOfWork.GetRepository<Request>().UpdateAsync(request);
            _requestRepository.Update(request);
            //if (_requestRepository.)
            return new BaseModel<RequestResponseModel>
            {
                Message = MessageResponseHelper.UpdateSuccessfully("event"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new RequestResponseModel()
                {
                    Request = _mapper.Map<RequestResponseDto>(request),
                }
                //};
                //return new BaseModel<RequestResponseModel>
                //{
                //    Message = MessageResponseHelper.UpdateFailed("event"),
                //    IsSuccess = false,
                //    StatusCode = StatusCodes.Status200OK,
=======
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
>>>>>>> develop
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