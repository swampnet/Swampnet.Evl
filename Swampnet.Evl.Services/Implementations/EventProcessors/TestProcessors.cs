using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations.EventProcessors
{
    class TestProcessor01 : IEventProcessor
    {
        public int Priority => 0;

        public Task ProcessAsync(EventEntity entity)
        {
            entity.Properties.Add(new EventPropertyEntity() { 
                Category = "test",
                Name = "processor",
                Value = this.GetType().Name
            });

            return Task.CompletedTask;
        }
    }


    class TestProcessor02 : IEventProcessor
    {
        public int Priority => 0;

        public Task ProcessAsync(EventEntity entity)
        {
            entity.Properties.Add(new EventPropertyEntity()
            {
                Category = "test",
                Name = "processor",
                Value = this.GetType().Name
            });

            return Task.CompletedTask;
        }
    }

}
