using MBS.Application.Models.General;
using MBS.Application.Models.Request;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;

namespace MBS.Application.Services.Interfaces;

public interface IRequestService
{
    Task<BaseModel<Pagination<RequestResponseDto>>> GetRequestsByUserId(GetRequestByUserIdPaginationRequest request);
    Task<BaseModel<Pagination<RequestResponseDto>>> GetRequests(GetRequestsPaginationRequest request);
    Task<BaseModel<RequestResponseModel>> GetRequestById(Guid requestId);
    Task<BaseModel<CreateRequestResponseModel, CreateRequestRequestModel>> CreateRequest(CreateRequestRequestModel request);
    Task<BaseModel<RequestResponseModel>> UpdateRequest(Guid requestId, UpdateRequestRequestModel request);
}