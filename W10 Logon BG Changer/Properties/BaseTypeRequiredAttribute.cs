using System;

namespace W10_Logon_BG_Changer.Properties
{
    /// <summary>
    ///     When applied to a target attribute, specifies a requirement for any type marked
    ///     with the target attribute to implement or inherit specific type or types.
    /// </summary>
    /// <example>
    ///     <code>
    /// [BaseTypeRequired(typeof(IComponent)] // Specify requirement
    /// public class ComponentAttribute : Attribute { }
    /// [Component] // ComponentAttribute requires implementing IComponent interface
    /// public class MyComponent : IComponent { }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [BaseTypeRequired(typeof(Attribute))]
    public sealed class BaseTypeRequiredAttribute : Attribute
    {
        public BaseTypeRequiredAttribute([NotNull] Type baseType)
        {
            BaseType = baseType;
        }

        [NotNull]
        public Type BaseType { get; private set; }
    }
}