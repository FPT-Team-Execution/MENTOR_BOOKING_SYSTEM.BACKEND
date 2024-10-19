using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class MeetingService : BaseService2<MeetingService>, IMeetingService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IRequestRepository _requestRepository;
<<<<<<< HEAD

    public MeetingService(IUnitOfWork unitOfWork, IMeetingRepository meetingRepository, IRequestRepository requestRepository, ILogger<MeetingService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
=======
    public MeetingService(
        IRequestRepository requestRepository,
        IMeetingRepository meetingRepository,
        ILogger<MeetingService> logger, IMapper mapper) : base(logger, mapper)
>>>>>>> develop
    {
        _meetingRepository = meetingRepository;
        _requestRepository = requestRepository;
    }
    public async Task<BaseModel<MeetingResponseModel>> GetMeetingId(Guid meetingId)
    {
        try
        {
<<<<<<< HEAD
            var meeting = await _meetingRepository.GetMeetingByIdAsync(meetingId);
=======
            var meeting = await _meetingRepository.GetByIdAsync(meetingId, "Id");
>>>>>>> develop
            if(meeting == null)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(meetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            return new BaseModel<MeetingResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("meeting"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new MeetingResponseModel
                {
                    Meeting = _mapper.Map<MeetingResponseDto>(meeting),
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<MeetingResponseDto>>> GetMeetings(int page, int size)
    {
        try
        {
<<<<<<< HEAD
            var meetings = await _meetingRepository.GetMeetingsPagingAsync(page, size);
=======
            var meetings = await _meetingRepository.GetPagedListAsync(page, size);
>>>>>>> develop
            return new BaseModel<Pagination<MeetingResponseDto>>
            {
                Message = MessageResponseHelper.GetSuccessfully("meetings"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<MeetingResponseDto>>(meetings)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<MeetingResponseDto>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>> CreateMeeting(CreateMeetingRequestModel request)
    {
        try
        {
            //check request
<<<<<<< HEAD
            var requestCheck = await _requestRepository.GetRequestByIdAsync(request.RequestId);
=======
            var requestCheck = await _requestRepository.GetByIdAsync(request.RequestId, "Id");
>>>>>>> develop
            if(requestCheck == null)
                return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.RequestNotFound(request.RequestId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            
            if(requestCheck.Status != RequestStatusEnum.Accepted)
                return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.InvalidRequestStatus(request.RequestId.ToString(), nameof(RequestStatusEnum.Accepted)),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            //Create meeting
            var newMeeting = new Meeting()
            {
                Id = Guid.NewGuid(),
                RequestId = request.RequestId, 
                Description = request.Description,
                Location = request.Location,
                MeetUp = request.MeetUp,
                Status = MeetingStatusEnum.New
            };
<<<<<<< HEAD
            await _meetingRepository.CreateAsync(newMeeting);
            //if(await _unitOfWork.CommitAsync() > 0)
=======
            var addResult =  await _meetingRepository.CreateAsync(newMeeting);
            if(addResult)
>>>>>>> develop
                return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("meeting"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateMeetingResponseModel
					{
                        RequestId = newMeeting.Id,
                    }
                };
<<<<<<< HEAD
            
=======
            return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("meeting"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
>>>>>>> develop
        }
        catch (Exception e)
        {
            return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<MeetingResponseModel>> UpdateMeeting(Guid meetingId,UpdateMeetingRequestModel request)
    {
        try
        {
            //check meeting
<<<<<<< HEAD
            var meeting = await _meetingRepository.GetMeetingByIdAsync(meetingId);
=======
            var meeting = await _meetingRepository.GetByIdAsync(meetingId, "Id");
>>>>>>> develop
            if (meeting == null)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(meetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    
                };
            if (meeting.Status != MeetingStatusEnum.New || meeting.Status != MeetingStatusEnum.Delayed)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.InvalidMeetingSatus(meetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    
                };
            
            //Update request
            meeting.Description = request.Description;
            meeting.Location = request.Location;
            meeting.MeetUp = meeting.MeetUp;
            meeting.Status = request.Status;
<<<<<<< HEAD
            _meetingRepository.Update(meeting);
            //if (_unitOfWork.Commit() > 0)
=======
            var updateResult = _meetingRepository.Update(meeting);
            if (updateResult)
>>>>>>> develop
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("meeting"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new MeetingResponseModel()
                    {
                        Meeting = _mapper.Map<MeetingResponseDto>(meeting),
                    }
                };
<<<<<<< HEAD
=======
            return new BaseModel<MeetingResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("meeting"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
>>>>>>> develop
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}