using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>> SignUpStudentAsync(
        RegisterStudentRequestModel request);

    Task<BaseResponseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>> SignUpMentorAsync(
        RegisterMentorRequestModel request);
}