using System;
using System.ComponentModel;
using AspectCore.Extensions.Reflection;

namespace Moz.Utils.Types
{
    public class TypeInfo
    {
        public string Guid { get; internal set; }

        public string Name
        {
            get
            {
                if (Type == null) return string.Empty;
                return Type.Name;
            }
        }

        public bool IsDefined<T>(bool inherit = true)
        {
            return Type.IsDefined(typeof(T), inherit);
        }

        public bool? IsInterface
        {
            get
            {
                if (Type == null) return null;
                return Type.IsInterface;
            }
        }

        public string FullName
        {
            get
            {
                if (Type == null) return string.Empty;
                var typeFullName = Type.FullName;
                var dllName = Type.Assembly.FullName;
                return $"{typeFullName},{dllName}";
            }
        }

        public string DisplayName
        {
            get
            {
                if (Type == null) return string.Empty;
                var reflector = Type.GetReflector();
                var descAttribute = reflector.GetCustomAttribute<DescriptionAttribute>();
                return descAttribute != null ? descAttribute.Description : Type.Name;
            }
        }

        public Type Type { get; internal set; }
    }
}