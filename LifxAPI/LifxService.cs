using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solarflare.LifxAPI
{
    public class LifxService
    {
        private string Token { get; set; }
        private string testUrl = "hey";

        public LifxService(string token)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
            Token = token;
        }

        public async Task<string>GetDevices()
        {
            using HttpClient client = new();

            Uri url = new("https://api.lifx.com/v1/lights/all");
            
            string result;
            try
            {
                using HttpRequestMessage request = new (HttpMethod.Get, url);
                SetRequestAuthorizationHeader(client);
                
                HttpResponseMessage response = await client.SendAsync(request);
                result = response.Content.ReadAsStringAsync().Result;
            }
            
            catch (Exception e)
            {
                Console.WriteLine($"Getting devices failed:  {e}");
                throw;
            }

            return result;
        }
        
        /// <summary>
        /// Responsible for assigning Authorization header to each request
        /// </summary>
        /// <param name="client"></param>
        private void SetRequestAuthorizationHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        }
    }
}