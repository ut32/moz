using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    /// <summary>
    /// tab_admin_menu
    /// </summary>
    [Validator(typeof(DeleteAdminMenuDtoValidator))]
    public class DeleteAdminMenuDto
    {
        public long Id {get;set;}     
    }

    public class DeleteAdminMenuDtoValidator : MozValidator<DeleteAdminMenuDto>
    {
        public DeleteAdminMenuDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
