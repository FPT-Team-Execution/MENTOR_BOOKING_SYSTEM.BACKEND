using MBS.Application.Models.General;
using MBS.Application.Models.MeetingMember;

namespace MBS.Application.Services.Interfaces;

public interface IMeetingMemberService
{
    Task<BaseModel<GetMeetingMemberResponseModel>> GetMembersByMeetingId(Guid meetingId);
    Task<BaseModel<CreateMeetingMemberResponseModel, CreateMeetingMemberRequestModel>> AddMemberMeeting(CreateMeetingMemberRequestModel request);
    Task<BaseModel<MeetingMemberResponseModel>> UpdateMeetingMember(Guid memberMeetingId, UpdateMeetingMemberRequestModel request);
        
}