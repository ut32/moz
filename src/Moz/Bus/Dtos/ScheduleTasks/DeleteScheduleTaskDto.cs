using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Biz.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(DeleteScheduleTaskRequestValidator))]
    public class DeleteScheduleTaskRequest
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        
        #endregion     
    }
    
    
    public class DeleteScheduleTaskResponse
    {
    
    }
    
    
    public class DeleteScheduleTaskRequestValidator : MozValidator<DeleteScheduleTaskRequest>
    {
        public DeleteScheduleTaskRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("发生错误");
        }
    }
    
}
