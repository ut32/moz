using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Articles.ArticleModels
{
    /// <summary>
    /// tab_article_model
    /// </summary>
    [Validator(typeof(DeleteArticleModelRequestValidator))]
    public class DeleteArticleModelRequest
    {
        public long Id {get;set;}     
    }
    
    
    public class DeleteArticleModelResponse
    {
    
    }
    
    
    public class DeleteArticleModelRequestValidator : MozValidator<DeleteArticleModelRequest>
    {
        public DeleteArticleModelRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
