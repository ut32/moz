using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Moz.Common;
using Moz.Common.Types;
using Moz.Utils;
using StackExchange.Redis;

namespace Moz.DataBase.Redis
{
    public static class StackExchangeRedisExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<HashEntry> ConvertToHashEntryList<T>(this T instance) where T : new()
        {
            if(instance==null)
                throw new ArgumentNullException(nameof(StackExchangeRedisExtensions));

            var accessor = GenericCache<TypeAccessor<T>>.Instance ?? (GenericCache<TypeAccessor<T>>.Instance = new TypeAccessor<T>());
            var propertyInfos = accessor.PropertyInfos;
            
            foreach (var property in propertyInfos)
            {
                if (property.IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    continue;

                var propertyType = property.PropertyType;
                if (!propertyType.IsValueType)
                    if(propertyType != typeof(string)) 
                        continue;

                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                var effectiveType = underlyingType ?? propertyType;

                var val = property.GetValue(instance);
                if (val == null) 
                    continue;
                
                if (effectiveType == typeof(DateTime))
                {
                    var date = (DateTime) val;
                    if (date.Kind == DateTimeKind.Utc)
                    {
                        yield return new HashEntry(property.Name, $"{date.Ticks}|UTC");
                    }
                    else
                    {
                        yield return new HashEntry(property.Name, $"{date.Ticks}|LOC");
                    }
                }
                else
                {
                    yield return new HashEntry(property.Name, val.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entries"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T ConvertFromHashEntryList<T>(this IEnumerable<HashEntry> entries) where T : new()
        {
            var instance = new T();
            var accessor = GenericCache<TypeAccessor<T>>.Instance ?? (GenericCache<TypeAccessor<T>>.Instance = new TypeAccessor<T>());
            var propertyInfos = accessor.PropertyInfos;
            var hashEntries = entries as HashEntry[] ?? entries.ToArray();
            foreach (var property in propertyInfos)
            {
                if (property.IsDefined(typeof(IgnoreDataMemberAttribute), true))
                    continue;

                var propertyType = property.PropertyType;
                if (!propertyType.IsValueType)
                {
                    if(propertyType != typeof(string)) 
                        continue;
                }
                
                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                var effectiveType = underlyingType ?? propertyType;

                var entry = hashEntries.FirstOrDefault(e => e.Name.ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry()))
                {
                    continue;
                }
                var value = entry.Value.ToString();

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (effectiveType == typeof(DateTime))
                {
                    if (value.EndsWith("|UTC"))
                    {
                        value = value.TrimEnd("|UTC".ToCharArray());
                        DateTime date;
                        
                        if (long.TryParse(value,  out var ticks))
                        {
                            date = new DateTime(ticks);
                            property.SetValue(instance,DateTime.SpecifyKind(date, DateTimeKind.Utc));
                        }
                    }
                    else
                    {
                        value = value.TrimEnd("|LOC".ToCharArray());
                        DateTime date;
                        if(long.TryParse(value, out var ticks))
                        {
                            date = new DateTime(ticks);
                            property.SetValue(instance,DateTime.SpecifyKind(date, DateTimeKind.Local));
                        }
                    }
                }
                else
                {
                    if (!TypeDescriptor.GetConverter(propertyType).CanConvertFrom(typeof(string)))
                        continue;

                    if (!TypeDescriptor.GetConverter(propertyType).IsValid(value))
                        continue;

                    var finalValue = TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(value);
                    property.SetValue(instance, finalValue);
                }
            }
            return instance;
        }
    }
}