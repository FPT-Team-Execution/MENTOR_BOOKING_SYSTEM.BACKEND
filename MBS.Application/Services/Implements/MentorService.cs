using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;

namespace MBS.Application.Services.Implements;

public class MentorService : IMentorService
{
    public Task<BaseModel<GetMentorOwnProfileResponseModel>> GetMentorOwnProfile(ClaimsPrincipal claimsPrincipal)
    {
        throw new NotImplementedException();
    }
}