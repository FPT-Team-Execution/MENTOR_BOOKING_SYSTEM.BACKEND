using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Models.MeetingMember;

namespace MBS.Application.Services.Interfaces;

public interface IMeetingService
{
    Task<BaseModel<MeetingResponseModel>> GetMeetingId(Guid  meetingId);
    Task<BaseModel<GetMeetingResponseModel>> GetMeetings();
    Task<BaseModel<MeetingResponseModel, CreateMeetingRequestModel>> CreateMeeting(CreateMeetingRequestModel request);
    Task<BaseModel<MeetingResponseModel>> UpdateMeeting(Guid meetingId, UpdateMeetingRequestModel request);
    
 

}