using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace LanguageLibrary
{
    public class Language
    {
        public static dynamic Default { get; private set; }

        public static void Set(string name)
        {
            dynamic temp = new ExpandoObject();
            string xml = "";

            using (Stream stream = Assembly.GetAssembly(typeof(Language)).GetManifestResourceStream("LanguageLibrary.Langs." + name + ".xml"))
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        xml = reader.ReadToEnd();
                    }

            XmlToDynamic.Parse(temp, XElement.Parse(xml));

            Default = temp.Language;
        }

        public static Dictionary<string, string> GetLangNames()
        {
            Dictionary<string, string> langs = new Dictionary<string, string>();

            string[] embeddedResources = Assembly.GetAssembly(typeof(Language)).GetManifestResourceNames();

            foreach (var lang in embeddedResources.Where(lang => lang.ToLower().EndsWith(".xml")))
            {
                var name = lang.Split('.')[2];
                string xml = string.Empty;

                using (Stream stream = Assembly.GetAssembly(typeof(Language)).GetManifestResourceStream("LanguageLibrary.Langs." + name + ".xml"))
                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            xml = reader.ReadToEnd();
                        }

                if (xml == string.Empty) continue;
                var langName = XElement.Parse(xml).Elements().First().Value;

                langs.Add(langName, name);
            }

            return langs;
        }
    }
}