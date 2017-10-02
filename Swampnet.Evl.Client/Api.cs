using Newtonsoft.Json;
using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    /// <summary>
    /// Helper class to aid calling the web api
    /// </summary>
    /// <remarks>
    /// @TODO: Not a great name.
    /// </remarks>
    public static class Api
    {
        public static string ApiKey { get; set; }
        public static string Endpoint { get; set; }

        public static async Task PostAsync(Event e, string apiKey, string endpoint)
        {
            Validate(apiKey, endpoint);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var rs = await client.PostAsync(
                    endpoint,
                    new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json"));

                rs.EnsureSuccessStatusCode();
            }
        }


        public static Task PostAsync(Event e)
        {
            return PostAsync(e, ApiKey, Endpoint);
        }


        public static async Task PostAsync(IEnumerable<Event> e, string apiKey, string endpoint)
        {
            Validate(apiKey, endpoint);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var rs = await client.PostAsync(
                    endpoint + "/bulk",
                    new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json"));

                rs.EnsureSuccessStatusCode();
            }
        }

        public static Task PostAsync(IEnumerable<Event> e)
        {
            return PostAsync(e, ApiKey, Endpoint);
        }


        private static void Validate(string apiKey, string endpoint)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("ApiKey");
            }
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("Endpoint");
            }
        }
    }
}
