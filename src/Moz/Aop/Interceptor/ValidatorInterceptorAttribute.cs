using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Core;
using Moz.Exceptions;

namespace Moz.Aop.Interceptor
{
    /// <summary>
    /// 
    /// </summary>
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
                                throw new AlertException(error.ErrorMessage);
                            }
                        }
                    }
                }
                await next(context);
            }
            catch (MozException ex)
            {
                //throw new MozAspectInvocationException(context, ex, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                //throw new MozAspectInvocationException(context, ex, 999);
            }
        }
    }
}