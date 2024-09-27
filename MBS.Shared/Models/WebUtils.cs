using System.Net;
using System.Web;
using MBS.Shared.Models.Google.Payload.Response;
using Newtonsoft.Json;

namespace MBS.Shared.Models
{
    public class WebUtils
    {
        public static async Task<GoogleCalendarEventResponse?> GetAsync(string url, string accessToken, DateTime timeMin, DateTime timeMax)
        {
            try
            {
                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["timeMin"] = timeMin.ToString("yyyy-MM-ddTHH:mm:ssK");
                query["timeMax"] = timeMax.ToString("yyyy-MM-ddTHH:mm:ssK");
                uriBuilder.Query = query.ToString();
                string finalUrl = uriBuilder.ToString();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                    HttpResponseMessage response = await client.GetAsync(finalUrl);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return null;
                    }
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    var calendarEventResponse = JsonConvert.DeserializeObject<GoogleCalendarEventResponse>(result);
                    return calendarEventResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GoogleCalendarEventResponse
                {
                    Items = []
                };
            }
        }


    }
}
