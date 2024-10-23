using System.Security.Claims;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Student;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class StudentService : BaseService2<StudentService>, IStudentService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStudentRepository _studentRepository;

    public StudentService(ILogger<StudentService> logger, IMapper mapper, IStudentRepository studentRepository,
        UserManager<ApplicationUser> userManager) : base(logger, mapper)
    {
        _studentRepository = studentRepository;
        _userManager = userManager;
    }

    public async Task<BaseModel<Pagination<StudentResponseDto>>> GetStudents(int page, int size)
    {
        try
        {
            var user = await _studentRepository.GetStudentsAsync(page, size);
            return new BaseModel<Pagination<StudentResponseDto>>()
            {
                Message = MessageResponseHelper.GetSuccessfully("students"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<StudentResponseDto>>(user)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<StudentResponseDto>>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<GetStudentResponseModel, GetStudentRequestModel>> GetOwnProfile(
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            var student = await _studentRepository.GetByIdAsync(userId, "UserId");

            if (student is null)
            {
                return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            student.User = user;
            
            return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("student profile"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseModel = _mapper.Map<GetStudentResponseModel>(student)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }

    public async Task<BaseModel<GetStudentResponseModel, GetStudentRequestModel>> GetStudent(
        GetStudentRequestModel request)
    {
        try
        {
            var student = await _studentRepository.GetByIdAsync(request.Id, "UserId");

            if (student is null)
            {
                return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    RequestModel = request,
                    ResponseModel = null
                };
            }

            var user = await _userManager.FindByIdAsync(student.UserId);

            student.User = user!;
            var response = _mapper.Map<GetStudentResponseModel>(student);

            return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("student"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseModel = response,
                RequestModel = request
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                RequestModel = request,
                ResponseModel = null
            };
        }
    }
}