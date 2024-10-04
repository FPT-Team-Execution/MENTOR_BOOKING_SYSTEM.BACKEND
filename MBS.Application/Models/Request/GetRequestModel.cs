namespace MBS.Application.Models.Request;

public class GetRequestResponseModel
{
    public IEnumerable<Core.Entities.Request> Requests { get; set; }
}