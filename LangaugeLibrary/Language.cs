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
        private static dynamic _default;

        public static dynamic Default
        {
            get; set;
        }

        public static void Init(string name = "en_us")
        {
            _default = new ExpandoObject();
            SetBaseLang(name);
        }

        public static void Set(string name)
        {
            var xml = GetXml(name);

            IDictionary<string, object> defaults = (IDictionary<string, object>)_default.Language;
            dynamic temp = new ExpandoObject();

            XmlToDynamic.Parse(temp, XElement.Parse(xml));

            foreach (var item in ((IDictionary<string, object>)temp.Language).Where(item => defaults.ContainsKey(item.Key)))
            {
                defaults[item.Key] = item.Value;
            }

            Default = defaults.ToExpando();
        }

        private static void SetBaseLang(string name)
        {
            var xml = GetXml(name);

            XmlToDynamic.Parse(_default, XElement.Parse(xml));
        }

        private static string GetXml(string name)
        {
            var xml = "";

            using (var stream = Assembly.GetAssembly(typeof(Language)).GetManifestResourceStream("LanguageLibrary.Langs." + name + ".xml"))
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                    {
                        xml = reader.ReadToEnd();
                    }

            return xml;
        }

        public static Dictionary<string, string> GetLangNames()
        {
            var langs = new Dictionary<string, string>();

            var embeddedResources = Assembly.GetAssembly(typeof(Language)).GetManifestResourceNames();

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