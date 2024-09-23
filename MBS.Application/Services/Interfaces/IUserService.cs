using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IUserService
{
    Task<RegisterStudentResponseModel> SignUpStudent(RegisterStudentModel registerStudentModel);
}