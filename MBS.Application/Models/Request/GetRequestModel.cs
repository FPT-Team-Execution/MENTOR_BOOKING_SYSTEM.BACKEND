using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Request;

public class GetRequestResponseModel
{
    public IEnumerable<RequestResponseDto> Requests { get; set; }
}

public class GetRequestsPaginationRequest
{
    [Required]
    public int Page { get; set; } = 1;
    [Required]
    public int Size { get; set; } = 10;
    [Required]
    public string SortOrder { get; set; } = "asc";
}



