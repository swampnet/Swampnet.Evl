using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;
using Swampnet.Evl.Client;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using MailKit.Net.Smtp;
using Swampnet.Evl.Common.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Swampnet.Evl.Plugins.Email
{
	/*
		Configuration:

		- Application settings
		{
			"email": {
				"default": {
					"from": "evl",
					"address": "evl@theswamp.co.uk"
				},
				"smtp": {
					"host": "smtp.sendgrid.net",
					"port": "465",
					"usr": "<usr>",
					"pwd": "<password>"
				}
			}
		}
		 

		- Rule properties:
		to
		cc
		bcc
		from-address
		from-name
		host
		port
		usr
		pwd
	*/

	class EmailActionHandler : IActionHandler
	{
		private readonly IConfiguration _cfg;
        private readonly ITemplateLoader _templateLoader;
        private readonly ITemplateTransformer _transformer;

        public EmailActionHandler(IConfiguration cfg, ITemplateLoader templateLoader, ITemplateTransformer transformer)
		{
			_cfg = cfg;
            _templateLoader = templateLoader;
            _transformer = transformer;
		}

        public string Type => "email";


        public async Task ApplyAsync(EventDetails evt, ActionDefinition actionDefinition, Rule rule)
		{
			var to = actionDefinition.Properties.StringValue("to");
			var cc = actionDefinition.Properties.StringValue("cc");
			var bcc = actionDefinition.Properties.StringValue("bcc");

            var from = evt.GetConfigValue("email:from-address", actionDefinition.Properties, _cfg);
            var fromName = evt.GetConfigValue("email:from-name", actionDefinition.Properties, _cfg);
            var host = evt.GetConfigValue("email:smtp:host", actionDefinition.Properties, _cfg);
            var port = evt.GetConfigValue("email:smtp:port", actionDefinition.Properties, _cfg);
            var usr = evt.GetConfigValue("email:smtp:usr", actionDefinition.Properties, _cfg);
            var pwd = evt.GetConfigValue("email:smtp:pwd", actionDefinition.Properties, _cfg);

			if (string.IsNullOrEmpty(host))
			{
				throw new ArgumentException("SMTP HOST parameter missing");
			}
			if (string.IsNullOrEmpty(port))
			{
				throw new ArgumentException("SMTP PORT parameter missing");
			}
			if (string.IsNullOrEmpty(from))
			{
				throw new ArgumentException("FROM parameter missing");
			}
			if (string.IsNullOrEmpty(to))
			{
				throw new ArgumentException("TO Parameter missing");
			}
			
			var message = new MimeMessage();
			message.From.Add(string.IsNullOrEmpty(fromName) ? new MailboxAddress(from) : new MailboxAddress(fromName, from));
			foreach(var x in to.Split(";"))
			{
				message.To.Add(new MailboxAddress(x));
			}
			if (!string.IsNullOrEmpty(cc))
			{
				foreach (var x in cc.Split(";"))
				{
					message.Cc.Add(new MailboxAddress(x));
				}
			}
			if (!string.IsNullOrEmpty(bcc))
			{
				foreach (var x in bcc.Split(";"))
				{
					message.Bcc.Add(new MailboxAddress(x));
				}
			}

			var template = _templateLoader.Load();
            var doc = _transformer.Transform(evt, rule, actionDefinition, template);

			var subject = doc.Element("email").Element("subject");
			var html = doc.Element("email").Element("html");

			message.Subject = subject.Value.Trim();

			message.Body = new TextPart("html")
			{
				Text = html.ToString()
			};

			using (var client = new SmtpClient())
			{
				client.ServerCertificateValidationCallback = (s, c, h, e) => true;
				client.Connect(host, System.Convert.ToInt32(port), true);
				client.AuthenticationMechanisms.Remove("XOAUTH2");

				if(!string.IsNullOrEmpty(usr) && !string.IsNullOrEmpty(pwd))
				{
					client.Authenticate(usr, pwd);
				}

				await client.SendAsync(message);

				client.Disconnect(true);
			}
		}


        public MetaDataCapture[] GetPropertyMetaData()
        {
            return new[]
            {
                new MetaDataCapture()
                {
                    Name = "to",
                    Description = "To",
                    IsRequired = true,
                    Options = new[]
                    {
                        new Option(null, ".*@.*")
                    }
                },
                new MetaDataCapture()
                {
                    Name = "cc",
                    Description = "CC",
                    IsRequired = false,
                    Options = new[]
                    {
                        new Option(null, ".*@.*")
                    }
                },
                new MetaDataCapture()
                {
                    Name = "bcc",
                    Description = "BCC",
                    IsRequired = false,
                    Options = new[]
                    {
                        new Option(null, ".*@.*")
                    }
                },

                new MetaDataCapture()
                {
                    Name = "email:from-address",
                    Description = "From (email)",
                    IsRequired = false
                },
                new MetaDataCapture()
                {
                    Name = "email:from-name",
                    Description = "From (name)",
                    IsRequired = false
                },
                new MetaDataCapture()
                {
                    Name = "email:smtp:host",
                    Description = "SMTP Host",
                    IsRequired = false
                },
                new MetaDataCapture()
                {
                    Name = "email:smtp:port",
                    Description = "SMTP Port",
                    IsRequired = false
                },
                new MetaDataCapture()
                {
                    Name = "email:smtp:usr",
                    Description = "SMTP Username",
                    IsRequired = false
                },
                new MetaDataCapture()
                {
                    Name = "email:smtp:pwd",
                    Description = "SMTP Password",
                    IsRequired = false
                }
            };
        }
    }
}
