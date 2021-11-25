using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Bus.Dtos.AdminMenus
{
    [Validator(typeof(CreateAdminMenuDtoValidator))]
    public class CreateAdminMenuDto
    {
        public string ParentIdsStr { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public int OrderIndex { get; set; }
        
        public long? ParentId
        {
            get
            {
                if (this.ParentIdsStr.IsNullOrEmpty()) return null;
                var ids = this.ParentIdsStr.Split(',')
                    .Where(t=>!t.IsNullOrEmpty() && t.All(char.IsDigit))
                    .Select(long.Parse).ToArray();
                if (ids.Any()) return ids.Last();
                return null;
            }
        }
    }
    
    public class CreateAdminMenuDtoValidator : MozValidator<CreateAdminMenuDto>
    {
        public CreateAdminMenuDtoValidator()
        {
            RuleFor(t => t.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
            RuleFor(t => t.Link).NotNull().NotEmpty().WithMessage("链接不能为空");
        }
    }
}