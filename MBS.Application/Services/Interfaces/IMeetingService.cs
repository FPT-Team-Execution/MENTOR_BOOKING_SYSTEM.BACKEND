using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Models.MeetingMember;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.Application.Services.Interfaces;

public interface IMeetingService
{
    Task<BaseModel<MeetingResponseModel>> GetMeetingId(Guid  meetingId);
    Task<BaseModel<Pagination<Meeting>>> GetMeetings(int page, int size);
    Task<BaseModel<MeetingResponseModel, CreateMeetingRequestModel>> CreateMeeting(CreateMeetingRequestModel request);
    Task<BaseModel<MeetingResponseModel>> UpdateMeeting(Guid meetingId, UpdateMeetingRequestModel request);
    
 

}