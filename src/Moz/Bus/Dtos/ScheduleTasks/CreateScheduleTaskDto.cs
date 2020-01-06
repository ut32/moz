using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(CreateScheduleTaskRequestValidator))]
    public class CreateScheduleTaskRequest
    {
        #region 属性
        
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
        
        #endregion     
    }
    
    
    public class CreateScheduleTaskResponse
    {
    
    }
    
    
    public class CreateScheduleTaskRequestValidator : MozValidator<CreateScheduleTaskRequest>
    {
        public CreateScheduleTaskRequestValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(x => x.Type).NotEmpty().WithMessage("任务不能为空");
            RuleFor(x => x.Cron).NotEmpty().WithMessage("CRON表达式不能为空");
        }
    }
    
}
