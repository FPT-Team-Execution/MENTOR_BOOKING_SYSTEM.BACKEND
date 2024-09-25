using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [AllowAnonymous]
    [HttpPost]
    [Route("student/sign-up")]
    public async Task<ActionResult<BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>>>
        SignUpStudent(
            [FromBody] RegisterStudentRequestModel request)
    {
        var response = await _userService.SignUpStudentAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("mentor/sign-up")]
    public async Task<ActionResult<BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>>>
        SignUpStudent(
            [FromBody] RegisterMentorRequestModel request)
    {
        var response = await _userService.SignUpMentorAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("sign-in")]
    public async Task<ActionResult<BaseModel<SignInResponseModel, SignInRequestModel>>> SignIn(
        SignInRequestModel request)
    {
        var response = await _userService.SignIn(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>>> Refresh(
        GetRefreshTokenRequestModel request)
    {
        var response = await _userService.Refresh(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("confirm-email")]
    public async Task<ActionResult<BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>>> ConfirmEmail(
        ConfirmEmailRequestModel request)
    {
        var response = await _userService.ConfirmEmailAsync(request);
        return StatusCode(response.StatusCode, response);
    }


    [HttpGet]
    [Route("test-auth-student")]
    [Authorize(Roles = nameof(UserRoleEnum.Student))]
    public async Task<IActionResult> TestAuthStudent()
    {
        return Ok("Hello");
    }


    [HttpGet]
    [Route("test-auth-mentor")]
    [Authorize(Roles = nameof(UserRoleEnum.Admin))]
    public async Task<IActionResult> TestAuthMentor()
    {
        return Ok("Hello");
    }
}