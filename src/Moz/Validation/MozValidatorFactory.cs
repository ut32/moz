using System;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Core;

namespace Moz.Validation
{
    public class MozValidatorFactory:AttributedValidatorFactory
    {
        public override IValidator GetValidator(Type type)
        {
            if (type == null)
                return null;

            var validatorAttribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));
            if (validatorAttribute == null || validatorAttribute.ValidatorType == null)
                return null;

            var instance = EngineContext.Current.ResolveUnregistered(validatorAttribute.ValidatorType);

            return instance as IValidator;
        }
    }
}