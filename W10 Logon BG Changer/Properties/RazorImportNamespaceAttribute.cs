using System;

namespace W10_Logon_BG_Changer.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RazorImportNamespaceAttribute : Attribute
    {
        public RazorImportNamespaceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}