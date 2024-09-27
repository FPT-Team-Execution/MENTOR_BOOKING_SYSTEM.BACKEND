using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Shared.Models
{
    public class WebUtils
    {
        //public static string GetResult(string url, string accessToken)
        //{
        //    string result = "";
        //    try
        //    {
        //        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //        myHttpWebRequest.ServicePoint.ConnectionLimit = 100;
        //        WebHeaderCollection headers = myHttpWebRequest.Headers;
        //        headers.Set("Accept-Charset", "utf-8");
        //        headers.Set("Authorization", "Bearer " + accessToken);
        //        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
        //        Stream receiveStream = myHttpWebResponse.GetResponseStream();
        //        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
        //        result = readStream.ReadToEnd();
        //        myHttpWebResponse.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }
        //    return result;
        //}

        public static async Task<string> GetResultAsync(string url, string accessToken)
        {
            string result = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {    
                    //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

    }
}
