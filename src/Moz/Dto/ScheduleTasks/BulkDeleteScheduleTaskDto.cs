using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(BulkDeleteScheduleTasksDtoValidator))]
    public class BulkDeleteScheduleTasksDto
    {
        public long[] Ids {get;set;}   
    }
    
    public class BulkDeleteScheduleTasksDtoValidator : MozValidator<BulkDeleteScheduleTasksDto>
    {
        public BulkDeleteScheduleTasksDtoValidator(ILocalizationService localizationService)
        {
             RuleFor(x => x.Ids).Must(x=>x.Any()).WithMessage("至少选择一项");
        }
    }
    
}
