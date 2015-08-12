using System;

namespace W10_Logon_BG_Changer.Properties
{
    /// <summary>
    ///     For a parameter that is expected to be one of the limited set of values.
    ///     Specify fields of which type should be used as values for this parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ValueProviderAttribute : Attribute
    {
        public ValueProviderAttribute(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; private set; }
    }
}