using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Plugins.Slack
{
	/// <summary>
	/// Decouple this from the event stuff (So we don't want to have any knowledge of Events here) as we might want
	/// to break this out into it's own package at some point.
	/// 
	/// It's currently based on the legacy slack api token: https://api.slack.com/custom-integrations/legacy-tokens
	/// </summary>
	interface ISlackApi
    {
        Task PostAsync(SlackMessage msg);
    }


    class SlackApi : ISlackApi
    {
        private const string BASE_URL = "https://slack.com/api";


        public async Task PostAsync(SlackMessage msg)
        {
            using (var client = new HttpClient())
            {
                var rs = await client.PostAsync(BASE_URL + "/chat.postMessage", new FormUrlEncodedContent(GetContent(msg)));

                rs.EnsureSuccessStatusCode();
            }
        }


        private IEnumerable<KeyValuePair<string, string>> GetContent(SlackMessage msg)
        {
            return new[]
            {
                new KeyValuePair<string, string>("token", msg.Token),
                new KeyValuePair<string, string>("channel", msg.Channel),
                new KeyValuePair<string, string>("attachments", JsonConvert.SerializeObject(Attachment.Mocked())),
                new KeyValuePair<string, string>("username", msg.UserName),
                new KeyValuePair<string, string>("icon_url", msg.Icon)
            };
        }


        class Attachment
        {
            public string title { get; set; }
            public string fallback { get; set; }
            public string color { get; set; }
            public string pretext { get; set; }
            public string text { get; set; }
            public Field[] fields { get; set; }


            public static Attachment[] Mocked()
            {
                var attachment = new Attachment();

                attachment.fallback = "fallback";
                attachment.pretext = "pretext";
                attachment.color = "good";
                attachment.title = "title";
                attachment.fields = new Field[]
                {
                    new Field()
                    {
                        title = "field-one",
                        value = "Field One",
                        @short = true
                    },
                    new Field()
                    {
                        title = "field-two",
                        value = "Field Two",
                        @short = true
                    }
                };

                return new Attachment[]
                {
                    attachment
                };
            }
        }

        class Field
        {
            public string title { get; set; }
            public string value { get; set; }
            public bool @short { get; set; }
        }
    }
}
