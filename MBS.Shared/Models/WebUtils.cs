using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using MBS.Shared.Models.Google;
using MBS.Shared.Models.Google.Payload.Response;
using Newtonsoft.Json;

namespace MBS.Shared.Models
{
    public class WebUtils
    {
        public static async Task<GetGoogleCalendarEventsResponse?> GetAsync(string url, string accessToken, DateTime timeMin, DateTime timeMax)
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
                    var calendarEventResponse = JsonConvert.DeserializeObject<GetGoogleCalendarEventsResponse>(result);
                    return calendarEventResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GetGoogleCalendarEventsResponse
                {
                    Items = []
                };
            }
        }
        
        public static async Task<GoogleCalendarEvent?> PostAsync(string url, string accessToken, EventTime start, EventTime end)
        {
            try
            { 
                var body = new
                {
                    //TODO: update timezone
                    start = new
                    {
                        dateTime = start.DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        timeZone = "Asia/Ho_Chi_Minh"
                    },
                    end = new
                    {
                        dateTime = end.DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        timeZone = "Asia/Ho_Chi_Minh"
                    }
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return null;
                    }
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    var calendarEventResponse = JsonConvert.DeserializeObject<GoogleCalendarEvent>(result);
                    return calendarEventResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GoogleCalendarEvent();
            }
        }
    public static async Task<GoogleCalendarEvent?> PostAsync(string url, string accessToken, EventTime start, EventTime end)
            {
                try
                { 
                    var body = new
                    {
                        //TODO: update timezone
                        start = new
                        {
                            dateTime = start.DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                            timeZone = "Asia/Ho_Chi_Minh"
                        },
                        end = new
                        {
                            dateTime = end.DateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                            timeZone = "Asia/Ho_Chi_Minh"
                        }
                    };
    
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    
                        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
    
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return null;
                        }
                        response.EnsureSuccessStatusCode();
                        string result = await response.Content.ReadAsStringAsync();
                        var calendarEventResponse = JsonConvert.DeserializeObject<GoogleCalendarEvent>(result);
                        return calendarEventResponse;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new GoogleCalendarEvent();
                }
            }


    }
}
