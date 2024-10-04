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

    public static string Invalid(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("The ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was invalid.");
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

    #region CRUD Message Helper
    public static string GetSuccessfully(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Get ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }
    public static string GetFailed(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Get ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" failed.");
        return stringBuilder.ToString();
    }
    public static string CreateSuccessfully(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Create ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }
    public static string CreateFailed(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Create ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" failed.");
        return stringBuilder.ToString();
    }
    public static string UpdateSuccessfully(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Update ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" successfully.");
        return stringBuilder.ToString();
    }
    public static string UpdateFailed(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Update ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" failed.");
        return stringBuilder.ToString();
    }
    public static string DeleteFailed(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Delete ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" failed.");
        return stringBuilder.ToString();
    }
    

    #endregion
    

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

    #region Meeting Message Helper
    public static string MeetingNotFound(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Meeting with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was not found.");
        return stringBuilder.ToString();
    }
    public static string InvalidMeetingSatus(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Meeting with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" has invalid.");
        return stringBuilder.ToString();
    }
    #endregion
    #region Calendar Event Message Helper
    public static string CalendarNotFound(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Calendar with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was not found.");
        return stringBuilder.ToString();
    }
    public static string CalendarInThePast(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Calendar with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was in the past.");
        return stringBuilder.ToString();
    }
    public static string BusyCalendar(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Calendar with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" had another meeting.");
        return stringBuilder.ToString();
    }
    #endregion
    #region Project Message Helper
    public static string ProjectNotFound(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Project with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was not found.");
        return stringBuilder.ToString();
    }
    public static string ProjectClosed(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Project with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was closed.");
        return stringBuilder.ToString();
    }
    public static string ProjectNotActivated(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Project with Id ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" was not activated.");
        return stringBuilder.ToString();
    }
    
    #endregion

    #region Request Message Helper
    public static string RequestNotFound(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("request with Id");
        stringBuilder.Append($" {objectName}");
        stringBuilder.Append(" not found.");
        return stringBuilder.ToString();
    }
    public static string InvalidRequestStatus(string objectName, string status)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("request with Id");
        stringBuilder.Append($" {objectName}");
        stringBuilder.Append($" not {status}.");
        return stringBuilder.ToString();
    }
    #endregion

}