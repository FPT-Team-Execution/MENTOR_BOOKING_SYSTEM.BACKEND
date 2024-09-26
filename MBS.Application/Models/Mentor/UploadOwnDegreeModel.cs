using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MBS.Application.Models.User;

public class UploadOwnDegreeRequestModel
{
    public required IFormFile File { get; set; }
}

public class UploadOwnDegreeResponseModel
{
    public bool Completed { get; set; }
}