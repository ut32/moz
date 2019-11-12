using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(SetIsEnableScheduleTaskRequestValidator))]
    public class SetIsEnableScheduleTaskRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        public bool IsEnable { get; set; }
        
        #endregion     
    }
    
    
    public class SetIsEnableScheduleTaskResponse
    {
     
    }
    
    
    public class SetIsEnableScheduleTaskRequestValidator : MozValidator<SetIsEnableScheduleTaskRequest>
    {
        public SetIsEnableScheduleTaskRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误，参与不能为0");
        }
    }
    
}
