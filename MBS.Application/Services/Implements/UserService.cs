using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MBS.Application.Services.Implements;

public class UserService : IUserService
{
    private readonly IStudentRepository _studentRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService
    (
        IStudentRepository studentRepository,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager
    )
    {
        _studentRepository = studentRepository;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public Task<RegisterStudentResponseModel> SignUpStudent(RegisterStudentModel registerStudentModel)
    {
        throw new NotImplementedException();
    }
}