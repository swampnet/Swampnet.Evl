using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Swampnet.Evl.Plugins.Email
{
    /// <summary>
    /// Load a template from *somewhere*
    /// </summary>
    interface ITemplateLoader
    {
        string Load();
    }


    class TemplateLoader : ITemplateLoader
    {
        public string Load()
        {
            return LoadResource("Swampnet.Evl.Plugins.Email.default.template.xml");
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
    }
}
