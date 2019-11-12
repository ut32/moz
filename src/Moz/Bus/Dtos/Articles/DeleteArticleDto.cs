using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.Articles
{
    /// <summary>
    /// tab_article
    /// </summary>
    [Validator(typeof(DeleteArticleRequestValidator))]
    public class DeleteArticleRequest
    {
        public long Id {get;set;}     
    }
    
    
    public class DeleteArticleResponse
    {
    
    }
    
    
    public class DeleteArticleRequestValidator : MozValidator<DeleteArticleRequest>
    {
        public DeleteArticleRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
