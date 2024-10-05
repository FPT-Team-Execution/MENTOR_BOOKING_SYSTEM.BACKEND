using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;

namespace MBS.Application.Services.Interfaces;

public interface IRequestService
{
    Task<BaseModel<Pagination<Request>>> GetRequests(int page, int size);
    Task<BaseModel<RequestResponseModel>> GetRequestById(Guid requestId);
    Task<BaseModel<RequestResponseModel, CreateRequestRequestModel>> CreateRequest(CreateRequestRequestModel request);
    Task<BaseModel<RequestResponseModel>> UpdateRequest(Guid requestId, UpdateRequestRequestModel request);
}