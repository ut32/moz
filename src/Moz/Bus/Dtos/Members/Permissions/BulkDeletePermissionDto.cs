using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Domain.Dtos.Members.Permissions
{
    /// <summary>
    /// tab_permission
    /// </summary>
    [Validator(typeof(BulkDeletePermissionRequestValidator))]
    public class BulkDeletePermissionsRequest
    {
        public long[] Ids {get;set;}   
    }
    
    
    public class BulkDeletePermissionsResponse
    {
    
    }
    
    
    public class BulkDeletePermissionRequestValidator : MozValidator<BulkDeletePermissionsRequest>
    {
        public BulkDeletePermissionRequestValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
