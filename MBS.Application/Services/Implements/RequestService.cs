using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class RequestService : BaseService<RequestService>, IRequestService
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public RequestService(IUnitOfWork unitOfWork, ILogger<RequestService> logger,
        UserManager<ApplicationUser> userManager
        ) : base(unitOfWork, logger)
    {
        _userManager = userManager;
    }
    public async Task<BaseModel<GetRequestResponseModel>> GetRequests()
    {
        try
        {
            var requests = await _unitOfWork.GetRepository<Request>().GetAllAsync();
            return new BaseModel<GetRequestResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetRequestResponseModel
                {
                    Requests = requests
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetRequestResponseModel>
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
            var request = await _unitOfWork.GetRepository<Request>().GetAsync(r => r.Id == requestId);
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
                    Request = request
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

    public async Task<BaseModel<RequestResponseModel, CreateRequestRequestModel>> CreateRequest(CreateRequestRequestModel requestmodel)
    {
        try
        {
            //check calendar event
            var calendarEvent = await _unitOfWork.GetRepository<CalendarEvent>().GetAsync(c => c.Id == requestmodel.CalendarEventId);
            if(calendarEvent == null)
                return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(requestmodel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //Check user ~ creater
            var user = await _userManager.FindByIdAsync(requestmodel.CreaterId);
            if(user == null)
                return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(requestmodel.CreaterId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            
            //Check project
            var project = await _unitOfWork.GetRepository<Project>().GetAsync(p => p.Id == requestmodel.ProjectId);
            if(project == null)
                return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.ProjectNotFound(requestmodel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if(project.Status != ProjectStatusEnum.Activated)
                return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.ProjectNotActivated(requestmodel.ProjectId.ToString()),
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
                Title = requestmodel.Title,
                Status = RequestStatusEnum.Pending
            };
            var addResult = await _unitOfWork.GetRepository<Request>().AddAsync(newRequest);
            if(addResult)
                return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("request"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = requestmodel,
                    ResponseModel = new RequestResponseModel
                    {
                        Request = newRequest
                    }
                };
            return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("request"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<RequestResponseModel, CreateRequestRequestModel>
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
            var request = await _unitOfWork.GetRepository<Request>().GetAsync(m => m.Id == requestId);
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
            var calendarEvent = await _unitOfWork.GetRepository<CalendarEvent>().GetAsync(m => m.Id == requestModel.CalendarEventId, "Meeting");
            if(calendarEvent == null)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(requestModel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    
                };
            //check calendar and meeting
            if(calendarEvent.Start <= DateTime.Now)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.CalendarInThePast(requestModel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    
                };
            if(calendarEvent.Meeting.Status == MeetingStatusEnum.New)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.BusyCalendar(requestModel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    
                };
            //TODO: call google calendar api to recheck event props
            //~
            
            //Update request
            request.CalendarEventId = requestModel.CalendarEventId;
            request.Title = requestModel.Title;
            request.Status = requestModel.Status;
            _unitOfWork.GetRepository<Request>().UpdateAsync(request);
            if (_unitOfWork.Commit() > 0)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("event"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new RequestResponseModel()
                    {
                        Request = request,
                    }
                };
            return new BaseModel<RequestResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("event"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
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