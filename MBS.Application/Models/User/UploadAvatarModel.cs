using System.ComponentModel.DataAnnotations;
using MBS.Application.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace MBS.Application.Models.User;

public class UploadAvatarRequestModel
{
    [MaxFileSize(5)]
    [AllowedExtensions([".jpg", ".png", ".jpeg"])]
    public required IFormFile File { get; set; }
}

public class UploadAvatarResponseModel
{
    public required string AvatarUrl { get; set; }
}