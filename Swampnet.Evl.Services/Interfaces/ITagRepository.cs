using Swampnet.Evl.Services.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    interface ITagRepository
    {
        Task<TagEntity> ResolveAsync(string name);
    }
}
