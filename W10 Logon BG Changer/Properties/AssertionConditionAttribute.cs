using System;

namespace W10_Logon_BG_Changer.Annotations
{
    /// <summary>
    ///     Indicates the condition parameter of the assertion method. The method itself should be
    ///     marked by <see cref="AssertionMethodAttribute" /> attribute. The mandatory argument of
    ///     the attribute is the assertion type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AssertionConditionAttribute : Attribute
    {
        public AssertionConditionAttribute(AssertionConditionType conditionType)
        {
            ConditionType = conditionType;
        }

        public AssertionConditionType ConditionType { get; private set; }
    }
}