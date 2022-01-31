using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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

        public async Task<string> SetState<T>(T payload) where T : Payload
        {
            if (payload == null)
            {
                throw new Exception($"payloadType: {nameof(payload)} must be provided");
            }

            Uri url = new("https://api.lifx.com/v1/lights/all/state");

            string payloadString = payload.ToString();

            StringContent outputData = BuildRequest(payloadString);
            
            using HttpClient client = new();

            string result;
            try
            {
                SetRequestAuthorizationHeader(client);
                
                HttpResponseMessage response = await client.PutAsync(url, outputData);
            
                result = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                Console.WriteLine($"something {payload} / failed {e}");
                return $"{e}";
            }
            //Return ID of content posted
            //string contentId = JObject.Parse(result)["id"]?.ToString() ?? throw new InvalidOperationException();

            return result;
        }
        
        /// <summary>
        /// Responsible for building string content data that can be sent via the request
        /// </summary>
        /// <param name="data">string of json format which contains the data to be sent</param>
        /// <returns>returns string content data</returns>
        /// <exception cref="Exception"></exception>
        private StringContent BuildRequest(string data)
        {
            if (data == null)
                throw new Exception($"data: {nameof(data)} must be provided");
            
            JObject jsonObject = JObject.Parse(data);
            
            StringContent outputData = new(jsonObject.ToString(), Encoding.UTF8, "application/json");

            return outputData;
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