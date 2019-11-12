
using System;
using System.Reflection;

namespace Moz.Utils.Types
{
    public class TypeAccessor<T>
    {
        private readonly Type _type;
        public TypeAccessor()
        {
            _type = typeof(T);
        }

        public MemberInfo[] Members => _type.GetMembers();

        public PropertyInfo[] PropertyInfos => _type.GetProperties();
        
        public Type Type => _type;

    }
}