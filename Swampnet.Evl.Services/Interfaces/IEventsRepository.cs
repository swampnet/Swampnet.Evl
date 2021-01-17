﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// Possibly roll all the DAL stuff into this service (and loose the DAL project)
    /// 
    /// </summary>
    public interface IEventsRepository
    {
        // should be search result with paging info etc, and should be an EventSummary
        Task<EventSearchResult> SearchAsync(EventSearchCriteria criteria);
        Task<Event> LoadAsync(long id);
        Task<Event> LoadAsync(Guid reference);

        Task<string[]> TagsAsync();
        Task<string[]> SourceAsync();

        /// <summary>
        /// Save event to backend
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Task SaveAsync(Event e);
    }
}