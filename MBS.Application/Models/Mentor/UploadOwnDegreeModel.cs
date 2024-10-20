using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MBS.Application.Models.User;

public class UploadOwnDegreeRequestModel
{
    public required IFormFile File { get; set; }
    public string Name { get; set; } = default;
    [MaxLength(100)] public string? Institution { get; set; } = default;
}

public class UploadOwnDegreeResponseModel
{
    public required Guid DegreeId { get; set; }
    public required string DegreeName { get; set; }
    public required string DegreeUrl { get; set; }
}