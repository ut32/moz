using FluentValidation;
using FluentValidation.Attributes;
using Moz.Biz.Models.ScheduleTasks;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(UpdateScheduleTaskRequestValidator))]
    public class UpdateScheduleTaskRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Cron { get;set; } 
        
        
        #endregion     
    }
    
    
    public class UpdateScheduleTaskResponse
    {
    
    }
    
    
    public class UpdateScheduleTaskRequestValidator : MozValidator<UpdateScheduleTaskRequest>
    {
        public UpdateScheduleTaskRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误");
            RuleFor(x => x.Name).NotEmpty().WithMessage("任务名称不能为空");
            RuleFor(x => x.Cron).NotEmpty().WithMessage("Cron表达式不能为空");
        }
    }
    
}
