using System.Dynamic;
using System.IO;
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

            using (Stream stream = Assembly.GetAssembly(typeof(Language)).GetManifestResourceStream("LangaugeLibrary.Langs." + name + ".xml"))
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        xml = reader.ReadToEnd();
                    }

            XmlToDynamic.Parse(temp, XElement.Parse(xml));

            Default = temp.Language;
        }
    }
}
