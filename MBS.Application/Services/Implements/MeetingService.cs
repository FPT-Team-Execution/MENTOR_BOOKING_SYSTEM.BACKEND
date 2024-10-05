using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class MeetingService : BaseService<MeetingService>, IMeetingService
{

    public MeetingService(IUnitOfWork unitOfWork, ILogger<MeetingService> logger) : base(unitOfWork, logger)
    {

    }
    public async Task<BaseModel<MeetingResponseModel>> GetMeetingId(Guid meetingId)
    {
        try
        {
            var meeting = await _unitOfWork.GetRepository<Meeting>().SingleOrDefaultAsync(r => r.Id == meetingId);
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
            var meetings = await _unitOfWork.GetRepository<Meeting>().GetListAsync();
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
            var requestCheck = await _unitOfWork.GetRepository<Request>().SingleOrDefaultAsync(c => c.Id == request.RequestId);
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
            await _unitOfWork.GetRepository<Meeting>().InsertAsync(newMeeting);
            if(await _unitOfWork.CommitAsync() > 0)
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
            var meeting = await _unitOfWork.GetRepository<Meeting>().SingleOrDefaultAsync(m => m.Id == meetingId);
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
            _unitOfWork.GetRepository<Meeting>().UpdateAsync(meeting);
            if (_unitOfWork.Commit() > 0)
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