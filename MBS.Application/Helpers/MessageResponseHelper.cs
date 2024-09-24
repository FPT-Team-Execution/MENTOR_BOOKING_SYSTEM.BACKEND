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

    public static string Login()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Login to system successfully");
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

    public static string UserNotFound()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("User was not found.");
        return stringBuilder.ToString();
    }

    public static string IncorrectEmailOrPassword()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Email or password was incorrect.");
        return stringBuilder.ToString();
    }

    public static string EmailNotConfirmed()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Email was not confirmed.");
        return stringBuilder.ToString();
    }
    
    public static string UserLocked()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("User have been locked.");
        return stringBuilder.ToString();
    }
}