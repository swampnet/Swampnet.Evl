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

		// @TODO: Add some kind of audit against the event to show we sent an email and to who (and what rule triggered it).
		// @TODO: Apply should really be async
		// @TODO: Actually, we really need the event ID as well (So we can reference it in the email template)
        //          - Sooner or later you're just going to have to have the ID on the event itself...
		public async Task ApplyAsync(Event evt, ActionDefinition actionDefinition, Rule rule)
		{
			var to = actionDefinition.Properties.StringValue("to");
			var cc = actionDefinition.Properties.StringValue("cc");
			var bcc = actionDefinition.Properties.StringValue("bcc");
			var from = actionDefinition.Properties.StringValue("from-address", _cfg["email:default:address"]);
			var fromName = actionDefinition.Properties.StringValue("from-name", _cfg["email:default:from"]);
			var host = actionDefinition.Properties.StringValue("host", _cfg["email:smtp:host"]);
			var port = actionDefinition.Properties.StringValue("port", _cfg["email:smtp:port"]);
			var usr = actionDefinition.Properties.StringValue("usr", _cfg["email:smtp:usr"]);
			var pwd = actionDefinition.Properties.StringValue("pwd", _cfg["email:smtp:pwd"]);

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
				client.Connect(host, Convert.ToInt32(port), true);
				client.AuthenticationMechanisms.Remove("XOAUTH2");

				if(!string.IsNullOrEmpty(usr) && !string.IsNullOrEmpty(pwd))
				{
					client.Authenticate(usr, _cfg["email:smtp:pwd"]);
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
                    IsRequired = true,
                    Options = new[]
                    {
                        new Option(null, ".*@.*")
                    }
                },
                new MetaDataCapture()
                {
                    Name = "cc",
                    IsRequired = false,
                    Options = new[]
                    {
                        new Option(null, ".*@.*")
                    }
                },
                new MetaDataCapture()
                {
                    Name = "bcc",
                    IsRequired = false,
                    Options = new[]
                    {
                        new Option(null, ".*@.*")
                    }
                },
            };
        }
    }
}
