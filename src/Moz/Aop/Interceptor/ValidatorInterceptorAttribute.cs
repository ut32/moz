using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.CMS.Dtos;
using Moz.Core;
using Moz.Exceptions;

namespace Moz.Aop.Interceptor
{
    /// <inheritdoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ValidateInterceptorAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                if (context.Parameters.Any() && context.Parameters[0]?.GetType().GetCustomAttribute<ValidatorAttribute>() != null)
                {
                    var parameter = context.Parameters[0];
                    var validatorAttr = parameter.GetType().GetCustomAttribute<ValidatorAttribute>();
                    if (validatorAttr != null)
                    {
                        if (EngineContext.Current.ResolveUnregistered(validatorAttr.ValidatorType) is IValidator validator)
                        {
                            var validationResult = validator.Validate(parameter);
                            if (!validationResult.IsValid && validationResult.Errors.Any())
                            {
                                var error = validationResult.Errors.First();
                                throw new MozException(error.ErrorMessage, 777);
                            }
                        }
                    }
                }
                await next(context);
            }
            catch (MozException ex)
            {
                throw new MozAspectInvocationException(context, ex, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                throw new MozAspectInvocationException(context, ex, 999);
            }
        }
    }
}