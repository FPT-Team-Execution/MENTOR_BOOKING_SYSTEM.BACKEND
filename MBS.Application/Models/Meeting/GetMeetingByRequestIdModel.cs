using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Meeting
{
    public class GetMeetingByRequestIdRequest
    {
        [FromRoute(Name = "requestId")]
        public required Guid RequestId { get; set; }
        [FromQuery]
        [EnumValidation(typeof(MeetingStatusEnum))]
        public string? MeetingStatus { get; set; }
    }
    public class GetMeetingByRequestIdResponse
    {
        public IEnumerable<MeetingResponseDto> Meetings { get; set; }
    }
}
