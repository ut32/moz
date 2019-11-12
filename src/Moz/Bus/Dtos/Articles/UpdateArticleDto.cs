using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.Articles
{
    /// <summary>
    /// tab_article
    /// </summary>
    [Validator(typeof(UpdateArticleRequestValidator))]
    public class UpdateArticleRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public long ArticleTypeId { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public string ParentIdsStr { get; set; }

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

        
        /// <summary>
        /// 
        /// </summary>
        public string Title { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string SubTitle { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string TitleColor { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? TitleBold { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Summary { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Content { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Tags { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string ThumbImage { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Video { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Source { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Author { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? Hits { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Addtime { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? OrderIndex { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? IsTop { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? IsRecommend { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string SeoTitle { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string SeoKeyword { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string SeoDescription { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string String1 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string String2 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string String3 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string String4 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? Int1 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? Int2 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? Int3 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? Int4 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public decimal? Decimal1 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public decimal? Decimal2 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public decimal? Decimal3 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public decimal? Decimal4 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Datetime1 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Datetime2 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Datetime3 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? Datetime4 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? Bool1 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? Bool2 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? Bool3 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool? Bool4 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Text1 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Text2 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Text3 { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Text4 { get;set; } 
        
        #endregion     
    }
    
    
    public class UpdateArticleResponse
    {
    
    }
    
    
    public class UpdateArticleRequestValidator : MozValidator<UpdateArticleRequest>
    {
        public UpdateArticleRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数不正确");
        }
    }
    
}
