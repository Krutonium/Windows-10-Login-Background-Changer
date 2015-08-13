using System;

namespace W10_Logon_BG_Changer.Properties
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class HtmlElementAttributesAttribute : Attribute
    {
        public HtmlElementAttributesAttribute()
        {
        }

        public HtmlElementAttributesAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}