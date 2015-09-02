using System;
using System.IO;
using System.Reflection;

namespace W10_Logon_BG_Changer.Tools
{
    public class AssemblyInfo
    {
        private readonly Assembly _assembly;

        public AssemblyInfo(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            _assembly = assembly;
        }

        /// <summary>
        ///     Gets the title property
        /// </summary>
        public string ProductTitle => GetAttributeValue<AssemblyTitleAttribute>(a => a.Title,
            Path.GetFileNameWithoutExtension(_assembly.CodeBase));

        /// <summary>
        ///     Gets the application's version
        /// </summary>
        public string Version
        {
            get
            {
                var version = _assembly.GetName().Version;
                return version?.ToString() ?? "1.0.0.0";
            }
        }

        /// <summary>
        ///     Gets the description about the application.
        /// </summary>
        public string Description => GetAttributeValue<AssemblyDescriptionAttribute>(a => a.Description);

        /// <summary>
        ///     Gets the product's full name.
        /// </summary>
        public string Product => GetAttributeValue<AssemblyProductAttribute>(a => a.Product);

        /// <summary>
        ///     Gets the copyright information for the product.
        /// </summary>
        public string Copyright => GetAttributeValue<AssemblyCopyrightAttribute>(a => a.Copyright);

        /// <summary>
        ///     Gets the company information for the product.
        /// </summary>
        public string Company => GetAttributeValue<AssemblyCompanyAttribute>(a => a.Company);

        protected string GetAttributeValue<TAttr>(Func<TAttr,
            string> resolveFunc, string defaultResult = null) where TAttr : Attribute
        {
            var attributes = _assembly.GetCustomAttributes(typeof(TAttr), false);
            return attributes.Length > 0 ? resolveFunc((TAttr)attributes[0]) : defaultResult;
        }
    }
}