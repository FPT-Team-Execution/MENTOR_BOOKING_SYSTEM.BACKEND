using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.MeetingMember;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class MeetingMemberService : BaseService<MeetingMemberService>, IMeetingMemberService
{

    public MeetingMemberService(IUnitOfWork unitOfWork, ILogger<MeetingMemberService> logger) : base(unitOfWork, logger)
    {
    }
    public async Task<BaseModel<GetMeetingMemberResponseModel>> GetMembersByMeetingId(Guid meetingId)
    {
        try
        {
            var meetingMembers = await _unitOfWork.GetRepository<MeetingMember>()
                .GetListAsync(
                    predicate: r => r.MeetingId == meetingId,
                    include: m => m.Include(x => x.Student));
            return new BaseModel<GetMeetingMemberResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("meeting members"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetMeetingMemberResponseModel
                {
                    MeetingMembers = meetingMembers
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetMeetingMemberResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<MeetingMemberResponseModel>> AddMemberMeeting(CreateMeetingMemberRequestModel request)
    {
        try
        {
            //check meeting
            var meeting = await _unitOfWork.GetRepository<Meeting>().SingleOrDefaultAsync(m => m.Id == request.MeetingId);
            
            if(meeting == null)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(request.MeetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if(meeting.Status != MeetingStatusEnum.New)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.InvalidMeetingSatus(request.MeetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            //create member meeting
            var newMeetingMem = new MeetingMember
            {
                Id = Guid.NewGuid(),
                MeetingId = request.MeetingId,
                StudentId = request.StudentId,
                JoinTime = request.JoinTime,
            };
            
            await _unitOfWork.GetRepository<MeetingMember>().InsertAsync(newMeetingMem);
            if(await _unitOfWork.CommitAsync() > 0)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("meeting member"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new MeetingMemberResponseModel
                    {
                        MeetingMember = newMeetingMem
                    }
                };
            return new BaseModel<MeetingMemberResponseModel>
            {
                Message = MessageResponseHelper.CreateFailed("meeting memeber"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingMemberResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
    

    public async Task<BaseModel<MeetingMemberResponseModel>> UpdateMeetingMember(Guid memberMeetingId, UpdateMeetingMemberRequestModel request)
    {
        try
        {
            //check meeting
            var meetingMember = await _unitOfWork.GetRepository<MeetingMember>().SingleOrDefaultAsync(
                predicate: m => m.Id == memberMeetingId, 
                include: m => m.Include(x => x.Meeting));
            if(meetingMember == null)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.DetailException(
                        objectName: "meeting member", 
                        target: "Id", 
                        targetValue: memberMeetingId.ToString(),
                        targetException: "not found"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if(meetingMember.Meeting.Status != MeetingStatusEnum.New)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.DetailException(
                        objectName: "meeting", 
                        target: "Id", 
                        targetValue: meetingMember.Meeting.Id.ToString(),
                        targetException: "not valid status"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            //check new value from request and update
            if(request.JoinTime == null || request.JoinTime < meetingMember.JoinTime)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.DetailException(
                        objectName: "meeting member", 
                        target: "joinTime", 
                        targetValue: request.JoinTime.ToString() ?? "",
                        targetException: "not invalid"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            meetingMember.JoinTime = request.JoinTime.Value;
            if(meetingMember.LeaveTime != null && (request.LeaveTime == null || request.LeaveTime < meetingMember.LeaveTime))
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.DetailException(
                        objectName: "meeting member", 
                        target: "leaveTime", 
                        targetValue: request.JoinTime.ToString() ?? "",
                        targetException: "not valid"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            meetingMember.LeaveTime = request.LeaveTime!.Value;
            _unitOfWork.GetRepository<MeetingMember>().UpdateAsync(meetingMember);
            if(_unitOfWork.Commit() > 0)
                return new BaseModel<MeetingMemberResponseModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("meeting member"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new MeetingMemberResponseModel
                    {
                        MeetingMember = meetingMember
                    }
                };
            return new BaseModel<MeetingMemberResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("meeting memeber"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingMemberResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}