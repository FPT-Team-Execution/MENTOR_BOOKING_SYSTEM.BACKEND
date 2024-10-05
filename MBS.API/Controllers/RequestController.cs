using MBS.Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/requests")]
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
    public async Task<ActionResult<BaseModel<RequestResponseModel>>> GetRequest(int page, int size)
    {
        var result = await _requestService.GetRequests(page, size);
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
    public async Task<ActionResult<BaseModel<RequestResponseModel>>> UpdateEvent([FromRoute] Guid requestId, UpdateRequestRequestModel requestModel)
    {
        var result = await _requestService.UpdateRequest(requestId, requestModel);
        return StatusCode(result.StatusCode, result);
        
    }

}