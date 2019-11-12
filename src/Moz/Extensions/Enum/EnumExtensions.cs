using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace System
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var enumMember = enumValue.GetType().GetMember(enumValue.ToString());

            DisplayAttribute displayAttrib = null;
            if (enumMember.Any())
                displayAttrib = enumMember
                    .First()
                    .GetCustomAttribute<DisplayAttribute>();

            string name = null;
            Type resource = null;

            if (displayAttrib != null)
            {
                name = displayAttrib.Name;
                resource = displayAttrib.ResourceType;
            }

            return string.IsNullOrEmpty(name) ? enumValue.ToString()
                : resource == null ? name
                : new ResourceManager(resource).GetString(name);
        }
    }
}