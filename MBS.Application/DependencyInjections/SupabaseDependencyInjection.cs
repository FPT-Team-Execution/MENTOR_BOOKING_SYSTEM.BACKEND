using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Supabase;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;

namespace MBS.Application.DependencyInjections
{
    public static class SupabaseDependencyInjection
    {
        private static async Task<ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject>>
            InitializeSupabaseAsync(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"]!;
            var key = configuration["Supabase:Key"]!;

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var supabase = new Supabase.Client(url, key, options);
            return await supabase.InitializeAsync();
        }

        public static void AddSupabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject>>(
                provider =>
                {
                    var config = provider.GetRequiredService<IConfiguration>();
                    return InitializeSupabaseAsync(config).GetAwaiter().GetResult();
                });
        }
    }
}