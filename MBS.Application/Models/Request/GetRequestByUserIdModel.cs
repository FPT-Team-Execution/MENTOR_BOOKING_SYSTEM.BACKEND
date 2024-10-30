using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Request;

public class GetRequestByUserIdPaginationRequest
{
    [FromRoute(Name = "UserId")]
    public required string UserId { get; set; }
    [FromQuery]
    public int Page { get; set; } = 1;
    [FromQuery]
    public int Size { get; set; } = 10;
    [FromQuery]
    public required string SortOrder { get; set; } = "asc";
    [EnumValidation(typeof(RequestStatusEnum))]
    public string? Status { get; set; }
}
