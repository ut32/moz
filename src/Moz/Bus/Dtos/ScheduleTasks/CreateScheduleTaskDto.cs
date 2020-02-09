using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(CreateScheduleTaskDtoValidator))]
    public class CreateScheduleTaskDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Type { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Cron { get;set; }      
    }
    
    public class CreateScheduleTaskDtoValidator : MozValidator<CreateScheduleTaskDto>
    {
        public CreateScheduleTaskDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(x => x.Type).NotEmpty().WithMessage("任务不能为空");
            RuleFor(x => x.Cron).NotEmpty().WithMessage("CRON表达式不能为空");
        }
    }
    
}
