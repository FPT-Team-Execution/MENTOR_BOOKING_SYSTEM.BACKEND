using MBS.Core.Enums;

namespace MBS.Application.Models.Request;

public class RequestResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string CalendarEventId { get; set; }
    public Guid? ProjectId { get; set; }
    public string CreaterId { get; set; }
    public Core.Entities.Student Creater { get; set; }
    public RequestStatusEnum Status { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}