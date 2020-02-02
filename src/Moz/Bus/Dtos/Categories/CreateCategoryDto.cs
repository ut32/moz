using System;
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
    [Validator(typeof(CreateCategoryDtoValidator))]
    public class CreateCategoryDto
    {
        #region 属性
        
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
        public string Description { get;set; }


        /// <summary>
        /// 
        /// </summary>
        public long? ParentId
        {
            get
            {

                if (ParentIdsStr.IsNullOrEmpty()) return null;
                var ids = ParentIdsStr.Split(',')
                    .Where(t => !t.IsNullOrEmpty() && t.All(char.IsDigit))
                    .Select(long.Parse).ToArray();
                if (ids.Any()) return ids.Last();
                return null;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParentIdsStr { get; set; }
        
        
        #endregion     
    }
    
    public class CreateCategoryDtoValidator : MozValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("类别名称不能为空");
        }
    }
    
}
