using System;
using SqlSugar;

namespace Moz.Bus.Models.Alarms
{
    [Flags]
    public enum NotifyType
    {
        None = 0,
        Email = 1,
        Sms = 2,
        WeChat = 4
    }
    
    /// <summary>
    /// 警报表
    /// </summary>
    [SugarTable("tab_alarm")]
    public class Alarm : BaseModel
    {
        /// <summary>
        /// 报警标题
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 报警描述
        /// </summary>
        public string Desc { get; set; }
        
        
        /// <summary>
        /// 发生时间
        /// </summary>
        [SugarColumn(ColumnName = "create_dt")]
        public DateTime CreateDt { get; set; } 
        
        /// <summary>
        /// 是否阅读
        /// </summary>
        [SugarColumn(ColumnName = "is_read")]
        public bool IsRead { get; set; }
        
        /// <summary>
        /// 通知类型
        /// </summary>
        [SugarColumn(ColumnName = "notify_type")]
        public NotifyType NotifyType { get; set; }
        
        /// <summary>
        /// 是否发送通知
        /// </summary>
        [SugarColumn(ColumnName = "is_notified")]
        public bool IsNotified { get; set; } 
        
        /// <summary>
        /// 通知时间
        /// </summary>
        [SugarColumn(ColumnName = "notify_dt")]
        public DateTime? NotifyDt { get; set; } 
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "notify_note")]
        public string NotifyNote { get; set; }  
    }
}