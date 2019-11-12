using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Moz.Exceptions;

namespace Moz.Aop.Filters
{
    
    public class SearchExceptionHandlerFilter : IActionFilter
    {
        private static readonly ConcurrentDictionary<string, IExceptionHandler> AllExceptionHandlers
            = new ConcurrentDictionary<string, IExceptionHandler>();

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor action)
            {
                var displayName = action.DisplayName;
                IExceptionHandler handler = null;
                if (AllExceptionHandlers.ContainsKey(displayName))
                {
                    handler = AllExceptionHandlers[displayName];
                }
                else
                {
                    var attributes = new List<ExceptionHandlerAttribute>();
                    var types = new List<Type> {action.ControllerTypeInfo.UnderlyingSystemType};
                    GetAllTypes(action.ControllerTypeInfo.UnderlyingSystemType, types);
                    foreach (var type in types) attributes.AddRange(GetExceptionHandlerAttributes(type));

                    var attribute = attributes.FirstOrDefault();
                    if (attribute != null)
                        handler = (IExceptionHandler) Activator.CreateInstance(attribute.ExceptionHandlerType);

                    AllExceptionHandlers[displayName] = handler;
                }

                if (handler != null) context.RouteData.Values.TryAdd("custom_exception_handler", handler);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="list"></param>
        private void GetAllTypes(Type type, List<Type> list)
        {
            var baseType = type.BaseType;
            if (baseType != null)
            {
                list.Add(baseType);
                GetAllTypes(baseType, list);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private IEnumerable<ExceptionHandlerAttribute> GetExceptionHandlerAttributes(ICustomAttributeProvider memberInfo)
        {
            return memberInfo
                .GetCustomAttributes(typeof(ExceptionHandlerAttribute), false)
                .Cast<ExceptionHandlerAttribute>();
        }
    }
}