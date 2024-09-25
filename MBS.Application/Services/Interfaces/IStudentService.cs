using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IStudentService
{
    Task<BaseModel<GetStudentOwnProfileResponseModel>> GetStudentOwnProfile(
        ClaimsPrincipal claimsPrincipal);
}