using System.Security.Claims;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Student;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class StudentService : BaseService<StudentService>, IStudentService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
        ILogger<StudentService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
    {
        _userManager = userManager;
    }

    public async Task<BaseModel<Pagination<StudentResponseDto>>> GetStudents(int page, int size)
    {
        try
        {
            var user = await _unitOfWork.GetRepository<Student>().GetPagingListAsync(
                include: s => s.Include(x => x.User),
                page: page,
                size: size
            );
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

            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(x => x.UserId == userId);

            if (student is null)
            {
                return new BaseModel<GetStudentResponseModel, GetStudentRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

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
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync
            (
                predicate: x => x.UserId == request.Id,
                include: x => x.Include(x => x.User)
            );

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