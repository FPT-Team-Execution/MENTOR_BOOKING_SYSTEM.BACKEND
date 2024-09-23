using System.Text;

namespace MBS.Application.Helpers;

public static class MessageResponseHelper
{
    public static string Register(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Register new ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }

    public static string UserWasExisted(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("User with email ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was already existed.");
        return stringBuilder.ToString();
    }
}