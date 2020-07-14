using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Articles
{
    /// <summary>
    /// tab_article
    /// </summary>
    [Validator(typeof(BulkDeleteArticlesRequestValidator))]
    public class BulkDeleteArticlesRequest
    {
        public long[] Ids {get;set;}   
    }
    
    
    public class BulkDeleteArticlesResponse
    {
    
    }
    
    
    public class BulkDeleteArticlesRequestValidator : MozValidator<BulkDeleteArticlesRequest>
    {
        public BulkDeleteArticlesRequestValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
