using System;

namespace W10_Logon_BG_Changer.Properties
{
    /// <summary>
    ///     Indicates that collection or enumerable value can contain null elements.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field)]
    public sealed class ItemCanBeNullAttribute : Attribute
    {
    }
}