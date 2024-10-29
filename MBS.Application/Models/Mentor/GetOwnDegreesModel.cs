using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.Application.Models.User;

public class GetOwnDegreesRequestModel
{
}

public class GetOwnDegreesResponseModel
{
    public Pagination<GetOwnDegreeResponseModel> degrees;
}