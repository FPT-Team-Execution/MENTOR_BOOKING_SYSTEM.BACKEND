using System.Text;

namespace MBS.Application.Exceptions;

public class DatabaseInsertException(string objectName) : Exception(GenerateMessage(objectName))
{
    private static string GenerateMessage(string objectName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("An error occur when insert ");
        stringBuilder.Append(objectName);
        stringBuilder.Append(" into database.");
        return stringBuilder.ToString();
    }
}