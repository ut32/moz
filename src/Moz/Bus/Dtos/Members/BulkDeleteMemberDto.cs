using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.Members
{
    /// <summary>
    /// tab_member
    /// </summary>
    [Validator(typeof(BulkDeleteMembersRequestValidator))]
    public class BulkDeleteMembersRequest
    {
        public long[] Ids {get;set;}   
    }
    
    
    public class BulkDeleteMembersResponse
    {
    
    }
    
    
    public class BulkDeleteMembersRequestValidator : MozValidator<BulkDeleteMembersRequest>
    {
        public BulkDeleteMembersRequestValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
