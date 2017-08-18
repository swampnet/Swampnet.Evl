using Newtonsoft.Json;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    // Bloody awful name.
    public static class Api
    {
        private static string _defaultEndpoint = "http://localhost:53830/api/events";
        private static string _apiKey = "29016692-9A8D-47CC-82A0-75C6BDB7D0DE";

        public static async Task PostAsync(Event e)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", _apiKey);

                var rs = await client.PostAsync(
                    _defaultEndpoint,
                    new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json"));
            }
        }


        public static async Task PostAsync(IEnumerable<Event> e)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", _apiKey);

                var rs = await client.PostAsync(
                    _defaultEndpoint + "/bulk",
                    new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json"));
            }
        }
    }
}
