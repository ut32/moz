using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.AdminMenus
{
    /// <summary>
    /// tab_admin_menu
    /// </summary>
    [Validator(typeof(BulkDeleteAdminMenusRequestValidator))]
    public class BulkDeleteAdminMenusRequest
    {
        public long[] Ids {get;set;}   
    }
    
    
    public class BulkDeleteAdminMenusResponse
    {
    
    }
    
    
    public class BulkDeleteAdminMenusRequestValidator : MozValidator<BulkDeleteAdminMenusRequest>
    {
        public BulkDeleteAdminMenusRequestValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
