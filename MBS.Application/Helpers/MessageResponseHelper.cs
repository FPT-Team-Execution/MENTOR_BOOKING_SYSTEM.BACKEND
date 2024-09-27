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

    public static string UserNotFound(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("User with email ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was not found.");
        return stringBuilder.ToString();
    }

    public static string ConfirmEmailSucceeded(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Confirm email for user ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }

    public static string ConfirmEmailFailed(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Confirm email for user ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" failed.");
        return stringBuilder.ToString();
    }

    public static string TokenInvalid()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Token was invalid.");
        return stringBuilder.ToString();
    }

    public static string RefreshTokenSuccessfully()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Refresh token successfully.");
        return stringBuilder.ToString();
    }

    public static string GetSuccessfully(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Get ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }

    public static string AuthorizeSuccessfully()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Authorization successfully.");
        return stringBuilder.ToString();
    }
    public static string AuthorizeFail()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Authorization failed.");
        return stringBuilder.ToString();
    }
    public static string UploadSuccessfully(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Upload ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }
    public static string InvalidInputParameter()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Invalid input parameters.");
        return stringBuilder.ToString();
    }
    public static string Successfully(string objectName)
    {
        var stringBuilder = new StringBuilder();
        //stringBuilder.Append("Get ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }

    public static string Fail(string objectName)
    {
        var stringBuilder = new StringBuilder();
        //stringBuilder.Append("Get ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" Fail.");
        return stringBuilder.ToString();
    }
}