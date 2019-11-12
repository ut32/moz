using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(ExecuteScheduleTaskRequestValidator))]
    public class ExecuteScheduleTaskRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        #endregion     
    }
    
    
    public class ExecuteScheduleTaskResponse
    {
    
    }
    
    
    public class ExecuteScheduleTaskRequestValidator : MozValidator<ExecuteScheduleTaskRequest>
    {
        public ExecuteScheduleTaskRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误，参与不能为0");
        }
    }
    
}
