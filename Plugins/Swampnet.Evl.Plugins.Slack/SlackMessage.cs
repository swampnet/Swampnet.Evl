using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Plugins.Slack
{
    class SlackMessage
    {
        public string Token { get; set; }
        public string Channel { get; set; }
        public string UserName { get; set; }
        public string Icon { get; set; }
    }
}
