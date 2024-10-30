using MBS.Core.Entities;
using MBS.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Request;

public class RequestResponseModel
{
    public RequestResponseDto Request { get; set; }
}

