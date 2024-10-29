using MBS.Application.Models.Request;
using MBS.Application.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/requests")]
[Authorize]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;
    public RequestController(IRequestService requestService)
    {
       _requestService = requestService;   
    }
    [HttpGet]
    [ProducesResponseType(typeof(BaseModel<RequestResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<RequestResponseModel>>> GetRequest([FromQuery]GetRequestsPaginationRequest request)
    {
        var result = await _requestService.GetRequests(request);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpGet("{requestId}")]
    [ProducesResponseType(typeof(BaseModel<RequestResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<RequestResponseModel>>> GetRequestById([FromRoute] Guid requestId)
    {
        var result = await _requestService.GetRequestById(requestId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [CustomAuthorize(UserRoleEnum.Admin,UserRoleEnum.Student)]
    [ProducesResponseType(typeof(BaseModel<RequestResponseModel, CreateRequestRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    
    public async Task<ActionResult<BaseModel<RequestResponseModel, CreateRequestRequestModel>>> CreateRequest([FromBody]CreateRequestRequestModel requestModel)
    {
        var result = await _requestService.CreateRequest(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    
    [HttpPut("{requestId}")]
    [ProducesResponseType(typeof(BaseModel<RequestResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<RequestResponseModel>>> UpdateRequest([FromRoute] Guid requestId, UpdateRequestRequestModel requestModel)
    {
        var result = await _requestService.UpdateRequest(requestId, requestModel);
        return StatusCode(result.StatusCode, result);
        
    }

}