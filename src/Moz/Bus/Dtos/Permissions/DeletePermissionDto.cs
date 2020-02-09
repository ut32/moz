using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(DeletePermissionDtoValidator))]
    public class DeletePermissionDto
    {
        public long Id {get;set;}     
    }
    
    public class DeletePermissionDtoValidator : MozValidator<DeletePermissionDto>
    {
        public DeletePermissionDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
