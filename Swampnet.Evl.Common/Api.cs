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
        private static string _apiKey = "29016692-9A8D-47CC-82A0-75C6BDB7D0DE";
        private static string _endpoint = "http://localhost:53831/api/events";

        public static string ApiKey { get => _apiKey; set => _apiKey = value; }
        public static string Endpoint { get => _endpoint; set => _endpoint = value; }

        public static async Task PostAsync(Event e)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

                var rs = await client.PostAsync(
                    Endpoint,
                    new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json"));

                rs.EnsureSuccessStatusCode();
            }
        }


        public static async Task PostAsync(IEnumerable<Event> e)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", ApiKey);

                var rs = await client.PostAsync(
                    Endpoint + "/bulk",
                    new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json"));

                rs.EnsureSuccessStatusCode();
            }
        }
    }
}
