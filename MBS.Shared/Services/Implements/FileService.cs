using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MBS.Shared.Services.Implements;

public class FileService : IFileService
{
    public async Task<byte[]?> ConvertIFormFileToByteArrayAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}