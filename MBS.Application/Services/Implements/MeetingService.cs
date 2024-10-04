using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MBS.Application.Services.Implements;

public class MeetingService : IMeetingService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IRequestRepository _requestRepository;
    public MeetingService(IMeetingRepository meetingRepository, IRequestRepository requestRepository)
    {
        _meetingRepository = meetingRepository;
        _requestRepository = requestRepository; 
    }
    public async Task<BaseModel<MeetingResponseModel>> GetMeetingId(Guid meetingId)
    {
        try
        {
            var meeting = await _meetingRepository.GetAsync(r => r.Id == meetingId);
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
                    Meeting = meeting
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

    public async Task<BaseModel<GetMeetingResponseModel>> GetMeetings()
    {
        try
        {
            var meetings = await _meetingRepository.GetAllAsync();
            return new BaseModel<GetMeetingResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("events"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetMeetingResponseModel
                {
                    Meetings = meetings
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetMeetingResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<MeetingResponseModel, CreateMeetingRequestModel>> CreateMeeting(CreateMeetingRequestModel request)
    {
        try
        {
            //check request
            var requestCheck = await _requestRepository.GetAsync(c => c.Id == request.RequestId);
            if(requestCheck == null)
                return new BaseModel<MeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.RequestNotFound(request.RequestId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            
            if(requestCheck.Status != RequestStatusEnum.Accepted)
                return new BaseModel<MeetingResponseModel, CreateMeetingRequestModel>
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
            var addResult = await _meetingRepository.AddAsync(newMeeting);
            if(addResult)
                return new BaseModel<MeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("meeting"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new MeetingResponseModel
                    {
                        Meeting = newMeeting
                    }
                };
            return new BaseModel<MeetingResponseModel, CreateMeetingRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("meeting"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingResponseModel, CreateMeetingRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<MeetingResponseModel>> UpdateMeeting(Guid meetingId, UpdateMeetingRequestModel request)
    {
        try
        {
            //check meeting
            var meeting = await _meetingRepository.GetAsync(m => m.Id == meetingId);
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
            var updateSuccess = await _meetingRepository.UpdateAsync(meeting);
            if (updateSuccess)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("meeting"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new MeetingResponseModel()
                    {
                        Meeting = meeting,
                    }
                };
            return new BaseModel<MeetingResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("meeting"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
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
}