using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MBS.Application.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.User;

public class UploadOwnDegreeRequestModel
{
	[FromBody]
	[MaxFileSize(5)]
	[AllowedExtensions(["jpg", "png", "jpeg"])]
	public required IFormFile File { get; set; }

	[FromBody]
	public string Name { get; set; } = default;
	[FromBody]
	[MaxLength(100)] public string? Institution { get; set; } = default;
}

public class UploadOwnDegreeResponseModel
{
	public required Guid DegreeId { get; set; }
	public required string DegreeName { get; set; }
	public required string DegreeUrl { get; set; }
}