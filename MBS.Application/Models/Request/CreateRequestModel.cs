using MBS.Core.Enums;

namespace MBS.Application.Models.Request;

public class CreateRequestRequestModel
{
    public string Title { get; set; }
    public  string CalendarEventId { get; set; }
    public Guid? ProjectId { get; set; }
    public string CreaterId { get; set; }
}