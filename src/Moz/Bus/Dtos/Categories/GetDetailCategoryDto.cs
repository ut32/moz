using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Categories
{
    /// <summary>
    /// tab_category
    /// </summary>
    [Validator(typeof(GetCategoryDetailDtoValidator))]
    public class GetCategoryDetailDto
    {
        public long Id {get;set;}      
    }
    
    
    public class CategoryDetail
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
        public string Alias { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Desciption { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int OrderIndex { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public long? ParentId { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Path { get;set; } 
        
    }
    
    
    public class GetCategoryDetailDtoValidator : MozValidator<GetCategoryDetailDto>
    {
        public GetCategoryDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
