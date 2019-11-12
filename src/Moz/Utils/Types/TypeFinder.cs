using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Moz.Utils.Types
{
    public static class TypeFinder
    {
        public static IEnumerable<TypeInfo> FindClassesOfType(Type type, bool onlyCreateClass = true)
        {
            var result = new List<TypeInfo>();
            var types = GetAllTypes();
            foreach (var typeItem in types)
            {
                if (typeItem.Type.IsInterface) continue;
                if (type.IsAssignableFrom(typeItem.Type))
                {
                    if (typeItem.Type.IsAbstract)
                    {
                        if (!onlyCreateClass) result.Add(typeItem);
                    }
                    else
                    {
                        result.Add(typeItem);
                    }
                }
                else
                {
                    if (typeItem.Type.IsClass && 
                        typeItem.Type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == type))
                    {
                        result.Add(typeItem);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TypeInfo> FindClassesOfType<T>(bool onlyCreateClass = true)
        {
            return FindClassesOfType(typeof(T), onlyCreateClass);
        }

        internal static TypeInfosList GetAllTypes()
        {
            if (GenericCache<TypeInfosList>.Instance == null)
            {
                var result = new TypeInfosList();

                var entryReferencedAssembliesTypes = DependencyContext.Default.RuntimeLibraries
                    .Where(t=>t.Dependencies.Any(x=>x.Name.Equals("Moz", StringComparison.OrdinalIgnoreCase)))
                    .Select(t => Assembly.Load(t.Name))
                    .SelectMany(t => t.GetTypes())
                    .Select(o => new TypeInfo {Guid = o.FullName.GetHashCode().ToString(), Type = o})
                    .ToList();

                //var ut = entryReferencedAssembliesTypes.Where(it => it.FullName.Contains("MingShiHui.Service")).ToList();

                var types = Assembly.Load("Moz")
                    .GetTypes()
                    .Select(o => new TypeInfo {Guid = o.FullName.GetHashCode().ToString(), Type = o})
                    .ToList();

                result.AddRange(entryReferencedAssembliesTypes);
                result.AddRange(types);

                GenericCache<TypeInfosList>.Instance = result;
            }

            return GenericCache<TypeInfosList>.Instance;
        }
    }
}