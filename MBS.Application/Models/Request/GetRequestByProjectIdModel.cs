using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Request;

public class GetRequestByProjectIdPaginationRequest
{
    [FromRoute(Name = "projectId")]
    public required string ProjectId { get; set; }
    [FromQuery]
    public int Page { get; set; } = 1;
    [FromQuery]
    public int Size { get; set; } = 10;
    [FromQuery]
    public required string SortOrder { get; set; } = "asc";
    [EnumValidation(typeof(RequestStatusEnum))]
    public string? RequestStatus { get; set; }
}