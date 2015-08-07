using System;

namespace W10_Logon_BG_Changer.Annotations
{
    /// <summary>
    ///     Indicates that the function argument should be string literal and match one
    ///     of the parameters of the caller function. For example, ReSharper annotates
    ///     the parameter of <see cref="System.ArgumentNullException" />.
    /// </summary>
    /// <example>
    ///     <code>
    /// public void Foo(string param) {
    ///   if (param == null)
    ///     throw new ArgumentNullException("par"); // Warning: Cannot resolve symbol
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InvokerParameterNameAttribute : Attribute
    {
    }
}