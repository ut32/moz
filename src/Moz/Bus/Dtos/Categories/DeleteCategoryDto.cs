using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Categories
{
    /// <summary>
    /// tab_category
    /// </summary>
    [Validator(typeof(DeleteCategoryDtoValidator))]
    public class DeleteCategoryDto
    {
        public long Id {get;set;}     
    }
    
    public class DeleteCategoryDtoValidator : MozValidator<DeleteCategoryDto>
    {
        public DeleteCategoryDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
