using MBS.Core.Enums;

namespace MBS.Application.Models.Request;

public class CreateRequestRequestModel
{
    public string Title { get; set; }
    public  string MentorId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Guid? ProjectId { get; set; }
    public string CreaterId { get; set; }
}

public class CreateRequestResponseModel
{
    public Guid RequestId { get; set; }
}