using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;
using FileOptions = Supabase.Storage.FileOptions;

namespace MBS.Shared.Services.Implements;

public class SupabaseService : ISupabaseService
{
    private readonly ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject>
        _supabaseClient;

    private readonly IFileService _fileService;

    public SupabaseService(
        ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> supabaseClient,
        IFileService fileService)
    {
        _supabaseClient = supabaseClient;
        _fileService = fileService;
    }

    public async Task<string> UploadFile(IFormFile formFile, string filePath, string bucketName)
    {
        var fileByte = await _fileService.ConvertIFormFileToByteArrayAsync(formFile);

        return await _supabaseClient.Storage.From(bucketName)
            .Upload
            (
                fileByte,
                filePath,
                new FileOptions()
                {
                    Upsert = false,
                    CacheControl = "3600",
                    ContentType = "image",
                }
            );
    }
}