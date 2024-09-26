using Microsoft.AspNetCore.Http;

namespace MBS.Shared.Services.Interfaces;

public interface IFileService
{
    Task<byte[]> ConvertIFormFileToByteArrayAsync(IFormFile file);
}