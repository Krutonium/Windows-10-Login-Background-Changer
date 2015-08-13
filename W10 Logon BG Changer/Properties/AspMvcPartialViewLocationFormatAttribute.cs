using System;

namespace W10_Logon_BG_Changer.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
    {
        public AspMvcPartialViewLocationFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; private set; }
    }
}