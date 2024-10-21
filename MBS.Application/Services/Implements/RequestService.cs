using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class RequestService : BaseService<RequestService>, IRequestService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private IRequestRepository _requestRepository;
    private ICalendarEventRepository _calendarEventRepository;

    public RequestService(IUnitOfWork unitOfWork, IRequestRepository requestRepository, ICalendarEventRepository calendarEventRepository, ILogger<RequestService> logger,
        UserManager<ApplicationUser> userManager,
        IMapper mapper
        ) : base(unitOfWork, logger, mapper)
    {
        _userManager = userManager;
        _requestRepository = requestRepository;
        _calendarEventRepository = calendarEventRepository;
    }
    public async Task<BaseModel<Pagination<RequestResponseDto>>> GetRequests(int page, int size)
    {
        try
        {
            var requests = await _requestRepository.GetPagedListAsync(
                page: page,
                size: size
                );
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
            //var request = await _unitOfWork.GetRepository<Request>().SingleOrDefaultAsync(r => r.Id == requestId);
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
            if (request == null)
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

    public async Task<BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>> CreateRequest(CreateRequestRequestModel requestmodel)
    {
        try
        {
            //check calendar event
            var calendarEvent = await _calendarEventRepository.GetCalendarEventByIdAsync(requestmodel.CalendarEventId);
            if (calendarEvent == null)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(requestmodel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //Check user ~ creater
            var user = await _userManager.FindByIdAsync(requestmodel.CreaterId);
            if (user == null)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(requestmodel.CreaterId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            //Check project
            var project = await _unitOfWork.GetRepository<Project>().SingleOrDefaultAsync(p => p.Id == requestmodel.ProjectId);
            if (project == null)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.ProjectNotFound(requestmodel.CalendarEventId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if (project.Status != ProjectStatusEnum.Activated)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
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
            await _requestRepository.CreateAsync(newRequest);
            //if (await _unitOfWork.CommitAsync() > 0)
                return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("request"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = requestmodel,
                    ResponseModel = new CreateRequestResponseModel
                    {
                        RequestId = newRequest.Id
                    }
                };
            //return new BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>
            //{
            //    Message = MessageResponseHelper.CreateFailed("request"),
            //    IsSuccess = false,
            //    StatusCode = StatusCodes.Status200OK,
            //};
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
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
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
            var calendarEvent = await _calendarEventRepository.GetCalendarEventAsync(requestModel.CalendarEventId);
            if (calendarEvent == null)
                return new BaseModel<RequestResponseModel>
                {
                    Message = MessageResponseHelper.CalendarNotFound(requestModel.CalendarEventId),
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
            if (calendarEvent.Meeting.Status == MeetingStatusEnum.New)
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