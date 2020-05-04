using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AspectCore.Extensions.Reflection;

namespace Moz.Common.Types
{
    public class TypeInfo
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Constructor
        public TypeInfo(Type type)
        {
            _type = type;
            
            var name = _type?.Name;
            var typeFullName = _type?.FullName;
            var dllName = _type?.Assembly.FullName;
            var fullName = $"{typeFullName},{dllName}";

            Type = _type;
            TypeName = name;
            IsInterface = _type?.IsInterface ?? false;
            FullName = fullName;
            UniqueId = Utils.EncryptHelper.MD5(fullName);
            DisplayName = GetDisplayName(_type);
            Order = GetOrder(_type);
        }
        
        #endregion

        #region Properties

        public string UniqueId { get; }

        public string TypeName { get; }

        public bool IsDefined<T>(bool inherit = true)
        {
            return _type.IsDefined(typeof(T), inherit);
        }

        public bool? IsInterface { get; }

        public string FullName { get; }

        public string DisplayName { get; }

        

        public Type Type { get;  }
        
        public int Order { get; }
        
        #endregion 

        #region Methods 
 
        private string GetDisplayName(Type type) 
        {
            if (type == null) 
                return string.Empty;
            
            var reflector = type.GetReflector();
            if (reflector == null) 
                return string.Empty;
            
            var displayAttribute = reflector.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                var name = displayAttribute.Name;
                return string.IsNullOrEmpty(name) ? type.Name : name;
            }
            
            var descAttribute = reflector.GetCustomAttribute<DescriptionAttribute>();
            var desc = descAttribute?.Description;
            return  string.IsNullOrEmpty(desc) ? type.Name : desc;
        }
        
        public int GetOrder(Type type)
        {
            if (type == null) 
                return 0;
            
            var reflector = type.GetReflector();
            var displayAttribute = reflector?.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute == null)
            {
                return 0;
            }

            try
            {
                return displayAttribute.Order;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        

        #endregion
    }
}