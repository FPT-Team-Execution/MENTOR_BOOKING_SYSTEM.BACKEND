using System.ComponentModel.DataAnnotations;
using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Project;

public class GetProjectsByUserIdRequest
{
    [FromRoute(Name = "userId")]
    public required string UserId { get; set; }
    [FromQuery]
    [EnumValidation(typeof(UserRoleEnum))]
    public required string UserRole { get; set; }
    [FromQuery]
    [EnumValidation(typeof(ProjectStatusEnum))]
    public string? ProjectStatus { get; set; }
    [Required]
    public int Page { get; set; } = 1;
    [Required]
    public int Size { get; set; } = 10;
    public required string SortOrder { get; set; } = "asc";
}

public class GetProjectsByUserIdResponse
{
    public IEnumerable<ProjectResponseDto> Projects {
        get;
        set;
    }
}