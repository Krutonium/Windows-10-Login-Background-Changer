using System;

namespace W10_Logon_BG_Changer.Properties
{
    /// <summary>
    ///     An extension method marked with this attribute is processed by ReSharper code completion
    ///     as a 'Source Template'. When extension method is completed over some expression, it's source code
    ///     is automatically expanded like a template at call site.
    /// </summary>
    /// <remarks>
    ///     Template method body can contain valid source code and/or special comments starting with '$'.
    ///     Text inside these comments is added as source code when the template is applied. Template parameters
    ///     can be used either as additional method parameters or as identifiers wrapped in two '$' signs.
    ///     Use the <see cref="MacroAttribute" /> attribute to specify macros for parameters.
    /// </remarks>
    /// <example>
    ///     In this example, the 'forEach' method is a source template available over all values
    ///     of enumerable types, producing ordinary C# 'foreach' statement and placing caret inside block:
    ///     <code>
    /// [SourceTemplate]
    /// public static void forEach&lt;T&gt;(this IEnumerable&lt;T&gt; xs) {
    ///   foreach (var x in xs) {
    ///      //$ $END$
    ///   }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SourceTemplateAttribute : Attribute
    {
    }
}