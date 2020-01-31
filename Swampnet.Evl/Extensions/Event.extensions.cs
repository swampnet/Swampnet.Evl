using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    public static class EventExtensions
    {
        private static string _baseUrl = "https://swampnet-events.azurewebsites.net/api";
        private static string _key = "== API-KEY ==";
        private static HttpClient _client = new HttpClient();

        public static Task PostAsync(this Event e)
        {
            return (new[] { e }).PostAsync();
        }


        public static async Task PostAsync(this IEnumerable<Event> events)
        {
            var json = JsonConvert.SerializeObject(events);

            using (var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl+"/post-bulk"))
            {
                request.Headers.Add("x-functions-key", _key);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await _client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();

                        //var rs = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}
