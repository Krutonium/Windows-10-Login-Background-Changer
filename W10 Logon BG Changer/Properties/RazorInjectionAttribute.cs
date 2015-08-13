using System;

namespace W10_Logon_BG_Changer.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RazorInjectionAttribute : Attribute
    {
        public RazorInjectionAttribute(string type, string fieldName)
        {
            Type = type;
            FieldName = fieldName;
        }

        public string Type { get; private set; }
        public string FieldName { get; private set; }
    }
}