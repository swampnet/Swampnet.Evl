using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.v2
{
    public class Event
    {
        public Event()
        {
            TimestampUtc = DateTime.UtcNow;
            History = new List<EventHistory>();
            Tags = new List<string>();
        }

        public Guid Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Category Category { get; set; }
        public string Source { get; set; }

        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }
        public List<string> Tags { get; set; }

        public Client.Property[] Properties { get; set; }
        public List<EventHistory> History { get; set; }



    }


    public class EventSummary
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Category Category { get; set; }
        public string Source { get; set; }
        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }
        public List<string> Tags { get; set; }
    }


    public class EventHistory
    {
        public EventHistory()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public EventHistory(string type, string details = null)
            : this()
        {
            Type = type;
            Details = details;
        }

        public DateTime TimestampUtc { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
    }


    public enum Category
    {
        debug,
        info,
        error
    }
}

namespace Swampnet.Evl
{
    public static class EventExtensionsV2
    {
        private static string _baseUrl = "https://swampnet-events.azurewebsites.net/api";
        private static HttpClient _client = new HttpClient();

        public static Task PostAsync(this v2.Event e, string apiKey)
        {
            return (new[] { e }).PostAsync(apiKey);
        }


        public static async Task PostAsync(this IEnumerable<v2.Event> events, string apiKey)
        {
            var json = JsonConvert.SerializeObject(events);

            using (var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/post-bulk"))
            {
                request.Headers.Add("x-functions-key", apiKey);
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
