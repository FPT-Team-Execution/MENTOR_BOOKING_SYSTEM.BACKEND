using MBS.Application.Exceptions;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
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

    public async Task<BaseResponseModel<RegisterStudentResponseModel>> SignUpStudentAsync(
        RegisterStudentRequestModel request)
    {
        try
        {
            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser is not null)
            {
                return new()
                {
                    Message = MessageResponseHelper.UserWasExisted(request.Email),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false
                };
            }

            var newUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                Gender = request.Gender,
                FullName = request.FullName,
                CreatedBy = request.Email,
                CreatedOn = DateTime.UtcNow,
            };

            var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createUserResult.Succeeded)
            {
                throw new DatabaseInsertException("user");
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(newUser, UserRoleEnum.Student.ToString());

            if (!assignRoleResult.Succeeded)
            {
                throw new DatabaseInsertException("role");
            }

            var newStudent = new Student()
            {
                UserId = newUser.Id,
                University = request.University,
                WalletPoint = 0
            };

            var addStudentResult = await _studentRepository.AddAsync(newStudent);

            if (addStudentResult is false)
            {
                throw new DatabaseInsertException("student");
            }

            return new BaseResponseModel<RegisterStudentResponseModel>()
            {
                Message = MessageResponseHelper.Register("student"),
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
                ResponseModel = new RegisterStudentResponseModel()
                {
                    UserId = newUser.Id
                },
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false
            };
        }
    }
}