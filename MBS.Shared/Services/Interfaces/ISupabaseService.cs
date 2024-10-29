using Microsoft.AspNetCore.Http;

namespace MBS.Shared.Services.Interfaces;

public interface ISupabaseService
{
    Task<string> UploadFile(byte[] fileByte, string filePath, string bucketName);
    string RetrievePublicUrl(string bucketName, string filePath);
}