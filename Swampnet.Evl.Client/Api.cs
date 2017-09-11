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


        public static async Task PostAsync(Event e)
        {
            Validate();

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
            Validate();

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

        private static void Validate()
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ArgumentNullException("ApiKey");
            }
            if (string.IsNullOrEmpty(Endpoint))
            {
                throw new ArgumentNullException("Endpoint");
            }
        }
    }
}
