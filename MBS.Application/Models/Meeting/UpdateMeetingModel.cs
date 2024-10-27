using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;

namespace MBS.Application.Models.Meeting;

public class UpdateMeetingRequestModel
{
    public required string Description { get; set; }
    public required string Location  { get; set; }
    public required string MeetUp { get; set; }
    [EnumValidation(typeof(MeetingStatusEnum))]
    public required string  Status { get; set; }
}