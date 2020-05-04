using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moz.Common.Types
{
    public class TypeAccessor<T>
    {
        private readonly Type _type;
        public TypeAccessor()
        {
            _type = typeof(T);
        }

        public MemberInfo[] Members => _type.GetMembers();

        public IEnumerable<PropertyInfo> PropertyInfos => _type.GetProperties();
        
        public Type Type => _type;

    }
}