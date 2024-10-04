using System.ComponentModel.DataAnnotations;
using MBS.Core.Enums;

namespace MBS.Application.Models.Request;

public class UpdateRequestRequestModel
{
    [Required]
    public string CalendarEventId { get; set; }
    [Required, MaxLength(100)]
    public string Title { get; set; }

    public RequestStatusEnum Status { get; set; }
    
}