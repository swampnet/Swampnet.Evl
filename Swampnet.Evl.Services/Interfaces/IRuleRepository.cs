using Swampnet.Evl.Services.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    public interface IRuleRepository
    {
        Task<Rule[]> LoadRulesAsync();
    }
}
