using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.Articles;
using Moz.Bus.Services.Localization;
using Moz.Validation;
using Newtonsoft.Json;

namespace Moz.Domain.Dtos.Articles.ArticleModels
{
    /// <summary>
    /// tab_article_model
    /// </summary>
    [Validator(typeof(GetArticleModelDetailRequestValidator))]
    public class GetArticleModelDetailRequest
    {
        public long Id {get;set;}      
    }
    
    
    public class GetArticleModelDetailResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool AllowAdd { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool AllowDel { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool AllowEdit { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Configuration { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public long? CategoryId { get;set; }


        public List<ArticleConfiguration> Configs
        {
            get
            {
                if (Configuration.IsNullOrEmpty()) 
                    return new List<ArticleConfiguration>();
                return JsonConvert.DeserializeObject<List<ArticleConfiguration>> (Configuration);

            }
        }

    }
    
    
    public class GetArticleModelDetailRequestValidator : MozValidator<GetArticleModelDetailRequest>
    {
        public GetArticleModelDetailRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
