using System;

namespace W10_Logon_BG_Changer.Properties
{
    /// <summary>
    ///     ASP.NET MVC attribute. Indicates that a parameter is an MVC area.
    ///     Use this attribute for custom wrappers similar to
    ///     <c>System.Web.Mvc.Html.ChildActionExtensions.RenderAction(HtmlHelper, String)</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcAreaAttribute : Attribute
    {
        public AspMvcAreaAttribute()
        {
        }

        public AspMvcAreaAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }

        public string AnonymousProperty { get; private set; }
    }
}