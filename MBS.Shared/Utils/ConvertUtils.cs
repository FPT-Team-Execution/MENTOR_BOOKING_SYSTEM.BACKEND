namespace MBS.Shared.Utils;

public class ConvertUtils
{
    public string JoinAuthorizeRoles(params string[] roles)
    {
        return string.Join(',', roles);
    }
}