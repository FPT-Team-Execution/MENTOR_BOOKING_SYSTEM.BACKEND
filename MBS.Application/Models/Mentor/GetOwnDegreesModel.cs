using MBS.Core.Entities;

namespace MBS.Application.Models.User;

public class GetOwnDegreesRequestModel
{
}

public class GetOwnDegreesResponseModel
{
    public IEnumerable<GetOwnDegreeResponseModel?> DegreeResponseModels { get; set; }
}