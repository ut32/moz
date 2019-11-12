using System;
using Moz.Bus.Models;
using Moz.CMS.Model;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.Biz.Models.ScheduleTasks
{
    public enum TaskRunningStatus
    {
        Pending = 1,
        Running = 2,
        Paused = 4,
        Error = 8
    }

    [SugarTable("tab_schedule_task")]
    public class ScheduleTask : BaseModel
    {
        public string Name { get; set; }

        public TaskRunningStatus Status { get; set; }

        [SugarColumn(ColumnName = "status_desc")]
        public string StatusDesc { get; set; }

        [SugarColumn(ColumnName = "job_key")] public string JobKey { get; set; }

        [SugarColumn(ColumnName = "job_group")]
        public string JobGroup { get; set; }

        [SugarColumn(ColumnName = "trigger_key")]
        public string TriggerKey { get; set; }

        [SugarColumn(ColumnName = "trigger_group")]
        public string TriggerGroup { get; set; }

        [SugarColumn(ColumnName = "is_enable")]
        public bool IsEnable { get; set; }

        public string Type { get; set; }

        public string Cron { get; set; }

        public int? Interval { get; set; }

        [SugarColumn(ColumnName = "last_start_time")]
        public DateTime? LastStartTime { get; set; }

        [SugarColumn(ColumnName = "last_end_time")]
        public DateTime? LastEndTime { get; set; }

        [SugarColumn(ColumnName = "last_success_time")]
        public DateTime? LastSuccessTime { get; set; }
    }
}