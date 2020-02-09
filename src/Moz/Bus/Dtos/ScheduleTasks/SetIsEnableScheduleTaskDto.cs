using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(SetIsEnableScheduleTaskDtoValidator))]
    public class SetIsEnableScheduleTaskDto
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        public bool IsEnable { get; set; }
        
        #endregion     
    }


    public class SetIsEnableScheduleTaskDtoValidator : MozValidator<SetIsEnableScheduleTaskDto>
    {
        public SetIsEnableScheduleTaskDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误，参与不能为0");
        }
    }
    
}
