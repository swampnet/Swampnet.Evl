using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Contracts
{
    interface IExpressionEvaluator
    {
        bool Evaluate(Expression expression, Event evt);
    }
}
