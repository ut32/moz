using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Validation;

namespace Moz.Biz.Dtos.Categories
{
    [Validator(typeof(CreateRequestValidator))]
    public class CreateRequest
    {
        public string ParentIdsStr { get; set; }
        public string Name { get; set; }
        
        public string Alias { get; set; }
        
        public string Description { get; set; }

        public long? ParentId
        {
            get
            {
                if (ParentIdsStr.IsNullOrEmpty()) return null;
                var ids = ParentIdsStr.Split(',')
                    .Where(t=>!t.IsNullOrEmpty() && t.All(char.IsDigit))
                    .Select(long.Parse).ToArray();
                if (ids.Any()) return ids.Last();
                return null;
            }
        }
    } 
    
    public class CreateResponse
    {
        
    }

    public class CreateRequestValidator : MozValidator<CreateRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(t => t.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
        }
    }
}