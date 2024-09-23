using Microsoft.AspNetCore.Http;

namespace MBS.Application.Models.General;

public class BaseResponseModel<TResponseModel, TRequestModel> where TResponseModel : class where TRequestModel : class
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public int StatusCode { get; set; }
    public TResponseModel? ResponseModel { get; set; }
    public TRequestModel? RequestModel { get; set; }
}