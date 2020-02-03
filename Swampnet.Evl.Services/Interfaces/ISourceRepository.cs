using Swampnet.Evl.Services.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    interface ISourceRepository
    {
        Task<SourceEntity> ResolveAsync(string name);
    }
}
