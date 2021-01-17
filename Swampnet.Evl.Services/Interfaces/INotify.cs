using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace Swampnet.Evl.Services.Interfaces
{
    public interface INotify
    {
        Task SendEmailAsync(EmailMessage msg);
    }
}
