using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyModel;

namespace Moz.Common.Types
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

        
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static TypeInfosList GetAllTypes()
        {
            if (GenericCache<TypeInfosList>.Instance == null)
            {
                var result = new TypeInfosList();

                var dc = DependencyContext.Default;
                var libs = dc.CompileLibraries
                    .Where(lib=>
                        "moz".Equals(lib.Name, StringComparison.OrdinalIgnoreCase) 
                        || lib.Dependencies.Any(it=> "moz".Equals(it.Name, StringComparison.OrdinalIgnoreCase))).ToList();
                
                var entryReferencedAssembliesTypes = libs
                    .Select(t => Assembly.Load(t.Name))
                    .SelectMany(t => t.GetTypes())
                    .Select(o => new TypeInfo(o))
                    .ToList();

                result.AddRange(entryReferencedAssembliesTypes);

                GenericCache<TypeInfosList>.Instance = result;
            }

            return GenericCache<TypeInfosList>.Instance;
        }
    }
}