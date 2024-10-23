namespace MBS.Application.Models.Request;

public class GetRequestResponseModel
{
    public IEnumerable<RequestResponseDto> Requests { get; set; }
}

public class GetRequestsPaginationRequest
{
    public int Page
    {
        get;
        set;
    } = 1;

    public int Size { get; set; } = 10;
    public string SortOrder { get; set; } = "asc";
}



