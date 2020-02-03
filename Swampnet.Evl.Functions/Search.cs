using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Swampnet.Evl.Services.Interfaces;

namespace Swampnet.Evl.Functions
{
    public class Search
    {
        private readonly IEventsRepository _eventsRepository;

        public Search(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }


        [FunctionName("search")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"get",Route = null)] HttpRequest req,
            ILogger log)
        {
            // Better way of doing this? We have any kind of ModalBinder in AzFuncs?
            string id = req.Query["id"];
            string summary = req.Query["summary"];
            string tags = req.Query["tags"];
            string start = req.Query["start"];
            string end = req.Query["end"];
            string page = req.Query["page"];
            string pageSize = req.Query["pageSize"];

            var rq = new EventSearchCriteria();

            if (!string.IsNullOrEmpty(id))
            {
                rq.Id = Guid.Parse(id);
            }
            if (!string.IsNullOrEmpty(summary))
            {
                rq.Summary = summary;
            }
            if (!string.IsNullOrEmpty(tags))
            {
                rq.Tags = tags;
            }
            if (!string.IsNullOrEmpty(page))
            {
                rq.Page = Convert.ToInt32(page);
            }
            if (!string.IsNullOrEmpty(pageSize))
            {
                rq.PageSize = Convert.ToInt32(pageSize);
            }
            if (!string.IsNullOrEmpty(start) && DateTime.TryParse(start, out var dtStart))
            {
                rq.Start = dtStart;
            }
            if (!string.IsNullOrEmpty(end) && DateTime.TryParse(end, out var dtEnd))
            {
                rq.End = dtEnd;
            }

            rq.Page = rq.Page == 0 ? 1 : rq.Page;

            var events = await _eventsRepository.SearchAsync(rq);

            return new OkObjectResult(events);
        }
    }
}
