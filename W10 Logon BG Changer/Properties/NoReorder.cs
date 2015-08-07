using System;

namespace W10_Logon_BG_Changer.Annotations
{
    /// <summary>
    ///     Prevents the Member Reordering feature from tossing members of the marked class.
    /// </summary>
    /// <remarks>
    ///     The attribute must be mentioned in your member reordering patterns
    /// </remarks>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class NoReorder : Attribute
    {
    }
}