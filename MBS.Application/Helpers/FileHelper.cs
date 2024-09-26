using Microsoft.AspNetCore.Http;

namespace MBS.Application.Helpers;

public static class FileHelper
{
    public static async Task<byte[]> ConvertIFormFileToByteArrayAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}