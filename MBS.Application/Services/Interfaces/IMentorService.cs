using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IMentorService
{
    Task<BaseModel<GetMentorOwnProfileResponseModel>> GetOwnProfile(ClaimsPrincipal claimsPrincipal);
}