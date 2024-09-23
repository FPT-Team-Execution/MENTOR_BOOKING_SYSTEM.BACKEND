using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
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
}