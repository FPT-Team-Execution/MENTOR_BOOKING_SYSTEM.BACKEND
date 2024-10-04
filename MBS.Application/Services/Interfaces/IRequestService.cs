using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Core.Enums;

namespace MBS.Application.Services.Interfaces;

public interface IRequestService
{
    Task<BaseModel<GetRequestResponseModel>> GetRequests();
    Task<BaseModel<RequestResponseModel>> GetRequestById(Guid requestId);
    Task<BaseModel<RequestResponseModel, CreateRequestRequestModel>> CreateRequest(CreateRequestRequestModel request);
    Task<BaseModel<RequestResponseModel>> UpdateRequest(Guid requestId, UpdateRequestRequestModel request);
}