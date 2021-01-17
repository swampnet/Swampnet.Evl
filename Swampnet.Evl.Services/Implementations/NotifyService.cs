using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Swampnet.Evl
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            To = new List<Recipient>();
            Cc = new List<Recipient>();
            Bcc = new List<Recipient>();
        }

        public string Subject { get; set; }
        public string Body { get; set; }

        public Recipient From { get; set; }

        public List<Recipient> To { get; set; }
        public List<Recipient> Cc { get; set; }
        public List<Recipient> Bcc { get; set; }

        public class Recipient
        {
            public Recipient()
            {
            }

            public Recipient(string address, string display = null)
            {
                Display = display;
                Address = address;
            }

            public string Display { get; set; }
            public string Address { get; set; }
        }
    }
}


namespace Swampnet.Evl.Services.Implementations
{

    class NotifyService : INotify
    {
        private readonly IConfigurationRoot _configuration;

        public NotifyService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailMessage msg)
        {
            var message = new MimeMessage();

            if(msg.From == null)
            {
                message.From.Add(new MailboxAddress(_configuration["smtp:default-from"]));
            }
            else
            {
                message.From.Add(new MailboxAddress(msg.From.Display, msg.From.Address));
            }

            foreach (var r in msg.To)
            {
                message.To.Add(new MailboxAddress(r.Display, r.Address));
            }
            foreach (var r in msg.Cc)
            {
                message.Cc.Add(new MailboxAddress(r.Display, r.Address));
            }
            foreach (var r in msg.Bcc)
            {
                message.Bcc.Add(new MailboxAddress(r.Display, r.Address));
            }

            message.Subject = msg.Subject;
            message.Body = new TextPart("html")
            {
                Text = msg.Body
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_configuration["smtp:host"], Convert.ToInt32(_configuration["smtp:port"]), false);
                await client.AuthenticateAsync(_configuration["smtp:account"], _configuration["smtp:password"]);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
