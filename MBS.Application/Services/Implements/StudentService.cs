using System.Security.Claims;
using AutoMapper;
using MBS.Application.Exceptions;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Student;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Common.Email;
using MBS.Shared.Services.Interfaces;
using MBS.Shared.Templates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class StudentService : BaseService2<StudentService>, IStudentService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IStudentRepository _studentRepository;
	private readonly IEmailService _emailService;
	private readonly ITemplateService _templateService;

	public StudentService(ILogger<StudentService> logger, IMapper mapper, IStudentRepository studentRepository,
		UserManager<ApplicationUser> userManager, IEmailService emailService, ITemplateService templateService) : base(logger, mapper)
	{
		_studentRepository = studentRepository;
		_userManager = userManager;
		_emailService = emailService;
		_templateService = templateService;
	}

	public async Task<BaseModel<Pagination<StudentResponseDto>>> GetStudents(int page, int size, string? sortOrder)
	{
		try
		{
			var user = await _studentRepository.GetStudentsAsync(page, size, sortOrder);
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

	public async Task<BaseModel<UpdateStudentResponseModel>> UpdateOwnProfile(ClaimsPrincipal User,
		UpdateStudentRequestModel request)
	{
		try
		{
			var student = await _studentRepository.GetByUserIdAsync(request.Id, include: x => x.Include(x => x.User));
			if (student is null)
			{
				return new BaseModel<UpdateStudentResponseModel>()
				{
					Message = MessageResponseHelper.UserNotFound(),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK
				};
			}

			student.University = request.University;
			student.WalletPoint = request.WalletPoint;
			student.MajorId = Guid.Parse(request.MajorId);

			var user = student.User;
			user.FullName = request.FullName;
			user.Birthday = request.Birthday;
			user.Gender = request.Gender;
			user.PhoneNumber = request.PhoneNumber;
			user.LockoutEnd = request.LockoutEnd;
			user.LockoutEnabled = request.LockoutEnabled;
			user.UpdatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString();
			user.UpdatedOn = DateTime.UtcNow;

			_studentRepository.Update(student);
			_userManager.UpdateAsync(user);

			return new BaseModel<UpdateStudentResponseModel>()
			{
				Message = MessageResponseHelper.Successfully("Update student"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK
			};
		}
		catch (Exception e)
		{
			return new BaseModel<UpdateStudentResponseModel>()
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
				ResponseRequestModel = new UpdateStudentResponseModel()
				{
					Succeed = false
				}
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

	public async Task<BaseModel<CreateStudentResponseModel, CreateStudentRequestModel>> CreateStudent(CreateStudentRequestModel request)
	{
		try
		{
			var existUser = await _userManager.FindByEmailAsync(request.Email);

			if (existUser is not null)
			{
				return new BaseModel<CreateStudentResponseModel, CreateStudentRequestModel>
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
				MajorId = request.MajorId,
				WalletPoint = 100
			};

			var result = await _studentRepository.CreateAsync(newStudent);

			if (!result)
			{
				throw new DatabaseInsertException("student");
			}

			var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.InvitationEmail);

			await _emailService.SendEmailAsync(EmailMessage.Create(newUser.Email!, emailTemplate, "[MBS]Invite you to the System"));
			
			return new BaseModel<CreateStudentResponseModel, CreateStudentRequestModel>()
			{
				Message = MessageResponseHelper.Register("student"),
				StatusCode = StatusCodes.Status201Created,
				IsSuccess = true,
			};
		}
		catch (Exception ex)
		{
			return new BaseModel<CreateStudentResponseModel, CreateStudentRequestModel>()
			{
				Message = ex.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
				RequestModel = request
			};

		}
	}
}