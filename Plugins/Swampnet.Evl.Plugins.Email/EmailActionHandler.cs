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

namespace Swampnet.Evl.Plugins.Email
{
	/*
		Configuration:

		{
			"email": {
				"default": {
					"from": "evl",
					"address": "evl@theswamp.co.cuk"
				},
				"smtp": {
					"host": "smtp.sendgrid.net",
					"port": "465",
					"usr": "<usr>",
					"pwd": "<password>"
				}
			}
		}
		 
	*/


	class EmailActionHandler : IActionHandler
	{
		private const string _defaultFrom = "evl@theswamp.co.uk";
		private readonly IConfiguration _cfg;

		public EmailActionHandler(IConfiguration cfg)
		{
			_cfg = cfg;
		}

		// @TODO: Figure out how to register it with te DI cleanly
		// @TODO: Add some kind of audit against the event to show we sent an email and to who (and what rule triggered it).
		// @TODO: Apply should really be async
		public void Apply(Event evt, ActionDefinition actionDefinition, Rule rule)
		{
			var to = actionDefinition.Properties.StringValues("to");
			var cc = actionDefinition.Properties.StringValues("cc");
			var bcc = actionDefinition.Properties.StringValues("bcc");
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
			if (!to.Any())
			{
				throw new ArgumentException("TO Parameter missing");
			}

			var message = new MimeMessage();
			message.From.Add(string.IsNullOrEmpty(fromName) ? new MailboxAddress(from) : new MailboxAddress(fromName, from));
			foreach(var x in to)
			{
				message.To.Add(new MailboxAddress(x));
			}
			foreach(var x in cc)
			{
				message.Cc.Add(new MailboxAddress(x));
			}
			foreach (var x in bcc)
			{
				message.Bcc.Add(new MailboxAddress(x));
			}

			message.Subject = $"'{rule.Name}' triggered at {evt.TimestampUtc}";

			// @TODO: Need a way better way of doing this (especially. considering Summary might have stuff that needs encoding for html.)
			message.Body = new TextPart("html")
			{
				Text = "<html><body><h1>" + evt.Summary + "</h1></body></html>"
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

				client.Send(message);
				client.Disconnect(true);
			}
		}
	}


	public static class Extensions
	{
		public static void AddShizzleWizzle(this IServiceCollection services)
		{
			//services.AddSingleton<IActionHandler, EmailActionHandler>();
		}
	}
}
