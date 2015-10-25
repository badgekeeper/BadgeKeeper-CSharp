using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BadgeKeeper.Network
{
    class BadgeKeeperApiService : IBadgeKeeperApi
    {
        private HttpClient webClient;

        public BadgeKeeperApiService()
        {
            webClient = MakeHttpClient();
        }

        public async Task<string> MakeGetRequest(string path)
        {
            try
            {
                string response = await WebClient.GetStringAsync(path);
                return response;
            }
            catch (System.Exception e)
            {
                string error = e.Message + (e.InnerException == null ? "" : e.InnerException.Message);
                return MakeErrorResponse(error);
            }
        }

        public async Task<string> MakePostRequest(string path, string body)
        {
            try
            {
                string result = string.Empty;
                var response = await WebClient.PostAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode && response.Content != null)
                {
                    result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    string error = "Error occurred, the status code is: " + response.StatusCode;
                    return MakeErrorResponse(error);
                }
            }
            catch (System.Exception e)
            {
                string error = e.Message + (e.InnerException == null ? "" : e.InnerException.Message);
                return MakeErrorResponse(error);
            }
        }

        private HttpClient MakeHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private string MakeErrorResponse(string error)
        {
            string result = "{\"Error\":{\"Code\":-1,\"Message\":\"" + error + "\"},\"Result\":null}";
            return result;
        }
    }
}
