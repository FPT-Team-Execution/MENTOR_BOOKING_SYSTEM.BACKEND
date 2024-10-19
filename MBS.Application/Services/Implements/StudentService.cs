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
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace MBS.Application.Services.Implements;

public class StudentService : BaseService2<StudentService>, IStudentService
{
    private readonly UserManager<ApplicationUser> _userManager;
<<<<<<< HEAD
    private IStudentRepository _studentRepository;

    public StudentService(UserManager<ApplicationUser> userManager, IStudentRepository studentRepository, IUnitOfWork unitOfWork,
        ILogger<StudentService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
=======
    private readonly IStudentRepository _studentRepository;

    public StudentService(ILogger<StudentService> logger, IMapper mapper, IStudentRepository studentRepository,
        UserManager<ApplicationUser> userManager) : base(logger, mapper)
>>>>>>> develop
    {
        _studentRepository = studentRepository;
        _userManager = userManager;
        _studentRepository = studentRepository;
    }

    public async Task<BaseModel<Pagination<StudentResponseDto>>> GetStudents(int page, int size)
    {
        try
        {
<<<<<<< HEAD
            var students = await _studentRepository.GetStudentsAsync(
                    page: page,
                    size: size
                );


=======
            var user = await _studentRepository.GetPagedListAsync(page, size);
>>>>>>> develop
            return new BaseModel<Pagination<StudentResponseDto>>()
            {
                Message = MessageResponseHelper.GetSuccessfully("students"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<StudentResponseDto>>(students)
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

<<<<<<< HEAD
            var student = await _studentRepository.GetByUserIdAsync(userId);
=======
            var student = await _studentRepository.GetByIdAsync(userId, "UserId");
>>>>>>> develop

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
<<<<<<< HEAD
            //var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync
            //(
            //    predicate: x => x.UserId == request.Id,
            //    include: x => x.Include(x => x.User)
            //);

            var student = await _studentRepository.GetByUserIdAsync(request.Id);

=======
            var student = await _studentRepository.GetByIdAsync(request.Id, "UserId");
>>>>>>> develop

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