using FluentValidation.Attributes;
using Moz.Bus.Models.ScheduleTasks;
using Moz.Bus.Services.Localization;
using Moz.Validation;

namespace Moz.Bus.Dtos.ScheduleTasks
{
    /// <summary>
    /// tab_schedule_task
    /// </summary>
    [Validator(typeof(PagedQueryScheduleTaskDtoValidator))]
    public class PagedQueryScheduleTaskDto
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    public class QueryScheduleTaskItem
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
        public bool IsEnable { get;set; } 

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

        /// <summary>
        /// 
        /// </summary>
        public string LastStartTimeString => LastStartTime?.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 
        /// </summary>
        public string LastEndTimeString => LastEndTime?.ToString("HH:mm:ss");
        /// <summary>
        /// 
        /// </summary>
        public string LastSuccessTimeString => LastSuccessTime?.ToString("yyyy-MM-dd HH:mm:ss");

    }
    
    public class PagedQueryScheduleTaskDtoValidator : MozValidator<PagedQueryScheduleTaskDto>
    {
        public PagedQueryScheduleTaskDtoValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
