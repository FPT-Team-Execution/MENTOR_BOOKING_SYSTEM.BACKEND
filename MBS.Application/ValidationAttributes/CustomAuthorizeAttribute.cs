using MBS.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Supabase.Gotrue;

namespace MBS.Application.ValidationAttributes;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public CustomAuthorizeAttribute(params UserRoleEnum[] roleEnums)
    {
        var allowedRolesAsString = roleEnums.Select(x => x.ToString());
        Roles = string.Join(",", allowedRolesAsString);
    }
}