using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IUserService
{
    Task<BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>> SignUpStudentAsync(
        RegisterStudentRequestModel request);

    Task<BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>> SignUpMentorAsync(
        RegisterMentorRequestModel request);

    Task<BaseModel<SignInResponseModel, SignInRequestModel>> SignIn(SignInRequestModel request);
    
}