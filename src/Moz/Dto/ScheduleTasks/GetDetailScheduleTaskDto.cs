using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Models.ScheduleTasks;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(GetScheduleTaskDetailDtoValidator))]
    public class GetScheduleTaskDetailDto
    {
        public long Id {get;set;}      
    }
    
    
    public class ScheduleTaskDetailApo
    {
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
        public TaskRunningStatus Status { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string StatusDesc { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string JobKey { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string JobGroup { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string TriggerKey { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string TriggerGroup { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnable { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Type { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public string Cron { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? Interval { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastStartTime { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastEndTime { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastSuccessTime { get;set; }  
        
    }
    
    
    public class GetScheduleTaskDetailDtoValidator : MozValidator<GetScheduleTaskDetailDto>
    {
        public GetScheduleTaskDetailDtoValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("参数错误");
        }
    }
    
}
