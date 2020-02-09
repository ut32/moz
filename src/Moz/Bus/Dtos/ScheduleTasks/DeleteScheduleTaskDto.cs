using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(DeleteScheduleTaskDtoValidator))]
    public class DeleteScheduleTaskDto
    {
        public long Id {get;set;}     
    }
    
    public class DeleteScheduleTaskDtoValidator : MozValidator<DeleteScheduleTaskDto>
    {
        public DeleteScheduleTaskDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
