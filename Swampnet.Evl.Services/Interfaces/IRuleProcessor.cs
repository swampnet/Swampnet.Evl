using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    public interface IRuleProcessor
    {
        Task ProcessEventAsync(Guid id);
    }
}
