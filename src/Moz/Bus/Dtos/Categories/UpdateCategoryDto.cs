using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.Categories
{
    /// <summary>
    /// tab_category
    /// </summary>
    [Validator(typeof(UpdateCategoryDtoValidator))]
    public class UpdateCategoryDto
    {
        #region 属性
        
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
        
        #endregion     
    }
    
    
    public class UpdateCategoryDtoValidator : MozValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误");
            RuleFor(x => x.Name).NotEmpty().WithMessage("类别名称不能为空");
        }
    }
    
}
