using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponseModel<RegisterStudentResponseModel>> SignUpStudentAsync(RegisterStudentRequestModel registerStudentModel);
}