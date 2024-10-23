using System.Net;
using System.Net.Http.Headers;
using System.Text;
using MBS.Shared.Models.Google.GoogleCalendar;
using Newtonsoft.Json;

namespace MBS.Shared.Utils
{
    public class WebUtils
    {
        // GET Request
        public static async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null, string? token = null, Dictionary<string, string?>? queryParams = null)
        {
            if (queryParams != null && queryParams.Any())
            {
                var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                url = $"{url}?{query}";
            }
    
            using var httpClient = new HttpClient();
    
            // Add headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
    
            // Add authorization token if provided
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
    
            return await httpClient.GetAsync(url);
        }
    
        // POST Request
        public static async Task<HttpResponseMessage> PostAsync(string url, object? data = null, Dictionary<string, string>? headers = null, string? token = null, Dictionary<string, string>? queryParams = null)
        {
            if (queryParams != null && queryParams.Any())
            {
                var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                url = $"{url}?{query}";
            }
    
            var json = data != null ? System.Text.Json.JsonSerializer.Serialize(data) : string.Empty;
            var content = new StringContent(json, Encoding.UTF8, "application/json");
    
            using var httpClient = new HttpClient();
    
            // Add headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
    
            // Add authorization token if provided
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
    
            return await httpClient.PostAsync(url, content);
        }
    
        // PUT Request
        public static async Task<HttpResponseMessage> PutAsync(string url, object? data = null, Dictionary<string, string>? headers = null, string? token = null, Dictionary<string, string>? queryParams = null)
        {
            if (queryParams != null && queryParams.Any())
            {
                var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                url = $"{url}?{query}";
            }
    
            var json = data != null ? System.Text.Json.JsonSerializer.Serialize(data) : string.Empty;
            var content = new StringContent(json, Encoding.UTF8, "application/json");
    
            using var httpClient = new HttpClient();
    
            // Add headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
    
            // Add authorization token if provided
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
    
            return await httpClient.PutAsync(url, content);
        }
    
        // DELETE Request
        public static async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string>? headers = null, string? token = null, Dictionary<string, string>? queryParams = null)
        {
            if (queryParams != null && queryParams.Any())
            {
                var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                url = $"{url}?{query}";
            }
    
            using var httpClient = new HttpClient();
    
            // Add headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
    
            // Add authorization token if provided
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
    
            return await httpClient.DeleteAsync(url);
        }
        
        //Handle Response
        public static T HandleResponse<T>(HttpResponseMessage response)
        {
            try
            {
                //response.EnsureSuccessStatusCode();

                var content = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(content))
                {
                    throw new InvalidOperationException("Response content is empty.");
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            // catch (TaskCanceledException ex)
            // {
            //     Console.WriteLine("Task canceled: " + ex.Message);
            //     throw;
            // }
            // catch (OperationCanceledException ex)
            // {
            //     Console.WriteLine("Operation canceled: " + ex.Message);
            //     throw;
            // }
            // catch (JsonSerializationException ex)
            // {
            //     Console.WriteLine("JSON deserialization error: " + ex.Message);
            //     throw;
            // }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling response: " + ex.Message);
                throw;
            }
        }
    }
}
