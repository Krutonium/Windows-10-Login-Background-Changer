using System;

namespace W10_Logon_BG_Changer.Annotations
{
    /// <summary>
    ///     Indicates that the marked method builds string by format pattern and (optional) arguments.
    ///     Parameter, which contains format string, should be given in constructor. The format string
    ///     should be in <see cref="string.Format(IFormatProvider,string,object[])" />-like form.
    /// </summary>
    /// <example>
    ///     <code>
    /// [StringFormatMethod("message")]
    /// public void ShowError(string message, params object[] args) { /* do something */ }
    /// public void Foo() {
    ///   ShowError("Failed: {0}"); // Warning: Non-existing argument in format string
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method |
        AttributeTargets.Property | AttributeTargets.Delegate)]
    public sealed class StringFormatMethodAttribute : Attribute
    {
        /// <param name="formatParameterName">
        ///     Specifies which parameter of an annotated method should be treated as format-string
        /// </param>
        public StringFormatMethodAttribute(string formatParameterName)
        {
            FormatParameterName = formatParameterName;
        }

        public string FormatParameterName { get; private set; }
    }
}