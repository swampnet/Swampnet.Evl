﻿using Swampnet.Evl.Common.Contracts;
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

		public EmailActionHandler(IConfiguration cfg)
		{
			_cfg = cfg;
		}

		// @TODO: Add some kind of audit against the event to show we sent an email and to who (and what rule triggered it).
		// @TODO: Apply should really be async
		// @TODO: Actually, we really need the event ID as well (So we can reference it in the email template)
		// @TODO: Need a way of allowing ActionHandlers to set up any DI stuff required...
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

			var template = LoadResource("Swampnet.Evl.Plugins.Email.default.template.xml");


			var transformed = Transform(ToXmlString(evt), template);

			var doc = XDocument.Parse(transformed);
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

				client.Send(message);
				client.Disconnect(true);
			}
		}


		private static string LoadResource(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceStream = assembly.GetManifestResourceStream(name);

			using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
		}

		private static string Transform(string xml, string xslt)
		{
			string transformed = null;

			var transform = new XslCompiledTransform();
			using (var reader = XmlReader.Create(new StringReader(xslt)))
			{
				transform.Load(reader);
			}

			using (var results = new StringWriter())
			{
				using (var reader = XmlReader.Create(new StringReader(xml)))
				{
					transform.Transform(reader, null, results);
					transformed = results.ToString();
				}
			}

			return transformed;
		}


		/// <summary>
		/// Serialize to xml
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static string ToXmlString(object o)
		{
			try
			{
				string xml = o as string;

				if (xml == null)
				{
					if (o != null)
					{
						XmlSerializer s = new XmlSerializer(o.GetType());

						using (var sw = new StringWriter())
						{
							s.Serialize(sw, o);
							xml = sw.ToString();
						}
					}
					else
					{
						xml = "<null/>";
					}
				}

				return xml;
			}
			catch (Exception ex)
			{
				ex.AddData("o", o == null ? "null" : o.GetType().Name);
				throw;
			}
		}

	}
}