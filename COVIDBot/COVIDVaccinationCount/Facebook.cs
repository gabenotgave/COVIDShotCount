using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COVIDVaccinationCount
{
    class Facebook
    {
        private readonly string pageId;
        private readonly string accessToken;
        private const string baseAddress = "https://graph.facebook.com/";
        public Facebook(string pageId, string accessToken)
        {
            this.pageId = pageId;
            this.accessToken = accessToken;
        }

        public async Task<string> PublishText(string text)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);

                var parameters = new Dictionary<string, string>
                {
                    { "access_token", accessToken },
                    { "message", text }
                };
                var encodedContent = new FormUrlEncodedContent(parameters);

                var result = await httpClient.PostAsync($"{pageId}/feed", encodedContent);
                var msg = result.EnsureSuccessStatusCode();
                return await msg.Content.ReadAsStringAsync();
            }
        }
    }
}
