using Microsoft.AspNetCore.Http;

namespace MBS.Application.Models.General;

public class BaseModel<TResponseModel, TRequestModel> where TResponseModel : class where TRequestModel : class
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public int StatusCode { get; set; }
    public TResponseModel? ResponseModel { get; set; }
    public TRequestModel? RequestModel { get; set; }
}

public class BaseModel<TResponseRequestModel> where TResponseRequestModel : class
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public int StatusCode { get; set; }
    public TResponseRequestModel? ResponseRequestModel { get; set; }
};

public class BaseModel
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public int StatusCode { get; set; }
};