using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Meeting;

public class GetMeetingByProjectIdRequest
{
    [FromRoute(Name = "projectId")]
    public required Guid ProjectId { get; set; }
    [FromQuery]
    [EnumValidation(typeof(MeetingStatusEnum))]
    public required string MeetingStatus { get; set; }
}

public class GetMeetingByProjectIdResponse
{
    public IEnumerable<MeetingResponseDto> Meetings { get; set; }
}