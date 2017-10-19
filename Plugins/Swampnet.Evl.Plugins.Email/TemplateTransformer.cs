using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Swampnet.Evl.Plugins.Email
{
    // Don't like the name. 'Looks like we've found a transfwormah'!
    interface ITemplateTransformer
    {
        // In: Event, ActionDefinition, Rule
        // Out: Subject, Html Body, Plain Body
        XDocument Transform(Event evt, Rule rule, ActionDefinition action, string template);
    }

	[XmlRoot("Data")]
	public class TemplateData
	{
		public TemplateData()
		{

		}

		public TemplateData(Event e, Rule rule, ActionDefinition action)
			: this()
		{
			Event = e;
			Rule = rule;
			Action = action;
		}

		public Event Event { get; set; }
		public Rule Rule { get; set; }
		public ActionDefinition Action { get; set; }
	}



	class TemplateTransformer : ITemplateTransformer
    {

        public XDocument Transform(Event evt, Rule rule, ActionDefinition action, string template)
        {
            string transformed = null;

            var transform = new XslCompiledTransform();
            using (var reader = XmlReader.Create(new StringReader(template)))
            {
                transform.Load(reader);
            }

            using (var results = new StringWriter())
            {
				using (var reader = XmlReader.Create(new StringReader(ToXmlString(new TemplateData(evt, rule, action)))))
				{
					transform.Transform(reader, null, results);
					transformed = results.ToString();
				}
            }

            return XDocument.Parse(transformed);
        }


        /// <summary>
        /// Serialize to xml
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ToXmlString(object o)
        {
            try
            {
                var xml = o as string;

                if (xml == null)
                {
                    if (o != null)
                    {
                        var s = new XmlSerializer(o.GetType());

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
