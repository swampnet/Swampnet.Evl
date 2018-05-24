using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTests.Mocks
{
    class MockedEventDataAccess : IEventDataAccess
    {
        private object _lock = new object();

        public int CreateCount { get; private set; }
        public int UpdateCount { get; private set; }

        public MockedEventDataAccess()
        {
            CreateCount = 0;
            UpdateCount = 0;
        }

        public Task<Guid> CreateAsync(Guid orgid, EventDetails evt)
        {
            lock (_lock)
            {
                CreateCount++;

                return Task.FromResult(Guid.Empty);
            }
        }

        public Task<IEnumerable<string>> GetSources(Organisation org)
        {
            return Task.FromResult(_events.Select(e => e.Source).Distinct());
        }

        public Task<IEnumerable<string>> GetTags(Organisation org)
        {
            return Task.FromResult(_events.SelectMany(e => e.Tags).Distinct());
        }

        public Task<long> GetTotalEventCountAsync(Organisation org)
        {
            return Task.FromResult(_events.LongCount());
        }

        public Task<EventDetails> ReadAsync(Organisation org, Guid id)
        {
            return Task.FromResult(_events.SingleOrDefault(e => e.Id == id));
        }

        public Task<IEnumerable<EventSummary>> SearchAsync(Organisation org, EventSearchCriteria criteria)
        {
            return Task.FromResult(_events.Select(e => new EventSummary() {
                Id = e.Id,
                Category = e.Category,
                Source = e.Source,
                Summary = e.Summary,
                TimestampUtc = e.TimestampUtc
            } ));
        }


        public Task UpdateAsync(Organisation org, Guid id, EventDetails evt)
        {
            lock (_lock)
            {
                UpdateCount++;

                return Task.CompletedTask;
            }
        }


        private static IEnumerable<EventDetails> _events = new[]
        {
            new EventDetails()
            {
                Id = Guid.Parse("6B625284-DD40-4AD5-95FF-5F6AEF6C214F"),
                Source = "MOCKED-SOURCE-01",
                Tags = new List<string>()
            },
            new EventDetails()
            {
                Id = Guid.Parse("2BD876EC-B9B3-4BDB-BBAB-DFC770BD835F"),
                Source = "MOCKED-SOURCE-02",
                Tags = new List<string>()
                {
                    "MOCKED-TAG-01",
                    "MOCKED-TAG-02",
                }
            },
            new EventDetails()
            {
                Id = Guid.Parse("6FF4E97E-3BDB-486F-B747-6FBDC2989BF8"),
                Source = "MOCKED-SOURCE-01",
                Tags = new List<string>()
                {
                    "MOCKED-TAG-03",
                }
            },
            new EventDetails()
            {
                Id = Guid.Parse("B5918833-60AD-4806-BEDB-09CDD35E9303"),
                Source = "MOCKED-SOURCE-01",
                Tags = new List<string>()
                {
                    "MOCKED-TAG-03",
                }
            }
        };
    }
}
