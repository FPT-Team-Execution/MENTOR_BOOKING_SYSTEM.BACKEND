using System.ComponentModel.DataAnnotations;
using MBS.Application.Models.General;
using MBS.Shared.Models.Google;

namespace MBS.Application.Models.User;

public class ExternalSignInRequestModel
{
    public GoogleAuthResponse ExternalInfo {
        get;
        set;
    }
}

public class ExternalSignInResponseModel
{
    public  JwtModel JwtModel { get; set; }
    public string AccessToken { get; set; }
}