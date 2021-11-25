using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.Articles;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.Articles.ArticleModels
{
    /// <summary>
    /// tab_article_model
    /// </summary>
    [Validator(typeof(CreateArticleModelRequestValidator))]
    public class CreateArticleModelRequest
    {
        #region 属性
        
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
        public List<ArticleConfiguration> Configs { get; set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string ParentIdsStr { get; set; }
        
        
        /// <summary>
        /// 
        /// </summary>
        public string Configuration => Newtonsoft.Json.JsonConvert.SerializeObject(Configs); 
        
        /// <summary>
        /// 
        /// </summary>
        public long? CategoryId {
            get
            {
                if (ParentIdsStr.IsNullOrEmpty()) return null;
                var lastCategoryId = ParentIdsStr.Split(',').Last(it => !it.IsNullOrEmpty());
                long.TryParse(lastCategoryId, out var id);
                if (id == 0) 
                    return null;
                return id;
            }
            
        }
        
        #endregion     
    }
    
    
    public class CreateArticleModelResponse
    {
    
    }
    
    
    public class CreateArticleModelRequestValidator : MozValidator<CreateArticleModelRequest>
    {
        public CreateArticleModelRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("模型名称不能为空");
            RuleFor(x => x.Configs).Must(it=>it.Any(x=>x.IsEnable)).WithMessage("配置不能为空");
        }
    }
    
}
