using System;

namespace W10_Logon_BG_Changer.Annotations
{
    /// <summary>
    ///     XAML attribute. Indicates the type that has <c>ItemsSource</c> property and should be treated
    ///     as <c>ItemsControl</c>-derived type, to enable inner items <c>DataContext</c> type resolve.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class XamlItemsControlAttribute : Attribute
    {
    }
}