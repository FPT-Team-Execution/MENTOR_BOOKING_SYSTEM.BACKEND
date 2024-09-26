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

    public SupabaseService(
        ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<string> UploadFile(byte[] fileByte, string filePath, string bucketName)
    {
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

    public string RetrievePublicUrl(string bucketName, string filePath)
    {
        return _supabaseClient.Storage.From(bucketName).GetPublicUrl(filePath);
    }
}