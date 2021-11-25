using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Categories
{
    /// <summary>
    /// tab_category
    /// </summary>
    [Validator(typeof(BulkDeleteCategoriesDtoValidator))]
    public class BulkDeleteCategoriesDto
    {
        public long[] Ids {get;set;}   
    }
    
    public class BulkDeleteCategoriesDtoValidator : MozValidator<BulkDeleteCategoriesDto>
    {
        public BulkDeleteCategoriesDtoValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
