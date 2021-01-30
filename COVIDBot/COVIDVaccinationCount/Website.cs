using COVIDVaccinationCount.WebsiteObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COVIDVaccinationCount
{
    static class Website
    {
        public static async Task<string> ClearCache(string key)
        {
            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "cache_key", key },
                    { "password", Credentials.GetValue("Website_POST_Req_PW") }
                };

                var encodedContent = new FormUrlEncodedContent(parameters);

                var result = await httpClient.PostAsync("https://covidshotcount.org/clear-cache", encodedContent);
                //var result = await httpClient.PostAsync("http://192.168.4.58:5000/clear-cache", encodedContent);

                var status = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ClearCacheStatus>(status).msg;
            }
        }
    }
}
