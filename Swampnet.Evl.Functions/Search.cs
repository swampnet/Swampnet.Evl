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
    public class SearchFunctions
    {
        private readonly IEventsRepository _eventsRepository;

        public SearchFunctions(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        [FunctionName("details")]
        public async Task<IActionResult> Details(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];
            if(Int64.TryParse(id, out long x))
            {
                return new OkObjectResult(await  _eventsRepository.LoadAsync(x));
            }
            else if(Guid.TryParse(id, out Guid y))
            {
                return new OkObjectResult(await _eventsRepository.LoadAsync(y));
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("search")]
        public async Task<IActionResult> Search(
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
            string showDebug = req.Query["showDebug"];
            string showInfo = req.Query["showInfo"];
            string showError = req.Query["showError"];

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
            if (bool.TryParse(showDebug, out var d))
            {
                rq.ShowDebug = d;
            }
            if (bool.TryParse(showInfo, out var i))
            {
                rq.ShowInformation = i;
            }
            if (bool.TryParse(showError, out var e))
            {
                rq.ShowError = e;
            }

            rq.Page = rq.Page == 0 ? 1 : rq.Page;

            log.LogInformation(JsonConvert.SerializeObject(rq));

            var events = await _eventsRepository.SearchAsync(rq);

            return new OkObjectResult(events);
        }

        [FunctionName("source")]
        public async Task<IActionResult> Source(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var source = await _eventsRepository.SourceAsync();
            return new OkObjectResult(source);
        }


        [FunctionName("tags")]
        public async Task<IActionResult> Tags(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var tags = await _eventsRepository.TagsAsync();
            return new OkObjectResult(tags);
        }
    }
}
