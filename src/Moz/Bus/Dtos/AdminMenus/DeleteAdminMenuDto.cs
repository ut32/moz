using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    /// <summary>
    /// tab_admin_menu
    /// </summary>
    [Validator(typeof(DeleteAdminMenuRequestValidator))]
    public class DeleteAdminMenuRequest
    {
        public long Id {get;set;}     
    }
    
    
    public class DeleteAdminMenuResponse
    {
    
    }
    
    
    public class DeleteAdminMenuRequestValidator : MozValidator<DeleteAdminMenuRequest>
    {
        public DeleteAdminMenuRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
