using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moz.Exceptions;

namespace Moz.Core.Engine
{
    public sealed class Engine : IEngine
    {
        private readonly IServiceProvider _serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Methods

        public T Resolve<T>() where T : class
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }
        
        public object Resolve(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)_serviceProvider.GetServices(typeof(T));
        }
        
        public IEnumerable<object> ResolveAll(Type type)
        {
            return _serviceProvider.GetServices(type);
        }

        public object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
                try
                {
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new FatalException("Unknown dependency");
                        return service;
                    });

                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }

            throw new FatalException("No constructor was found that had all the dependencies satisfied.");
        }

        #endregion
    }
}