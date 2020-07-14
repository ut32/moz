using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.ScheduleTasks;
using Moz.Bus.Models.ScheduleTasks;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;
using Moz.TaskSchedule;

namespace Moz.Bus.Services.ScheduleTasks
{
    public partial class ScheduleTaskService : BaseService, IScheduleTaskService
    {
        #region Constants

        #endregion

        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;
        private readonly ITaskScheduleManager _taskScheduleManager;

        #endregion

        #region Ctor

        public ScheduleTaskService(
            IEventPublisher eventPublisher,
            IDistributedCache distributedCache,
            ITaskScheduleManager taskScheduleManager)
        {
            this._eventPublisher = eventPublisher;
            this._distributedCache = distributedCache;
            this._taskScheduleManager = taskScheduleManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<ScheduleTaskDetailApo> GetScheduleTaskDetail(GetScheduleTaskDetailDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(dto.Id);
                if (scheduleTask == null)
                {
                    return Error("找不到该数据");
                }

                var resp = new ScheduleTaskDetailApo
                {
                    Id = scheduleTask.Id,
                    Name = scheduleTask.Name,
                    Status = scheduleTask.Status,
                    StatusDesc = scheduleTask.StatusDesc,
                    JobKey = scheduleTask.JobKey,
                    JobGroup = scheduleTask.JobGroup,
                    TriggerKey = scheduleTask.TriggerKey,
                    TriggerGroup = scheduleTask.TriggerGroup,
                    IsEnable = scheduleTask.IsEnable,
                    Type = scheduleTask.Type,
                    Cron = scheduleTask.Cron,
                    Interval = scheduleTask.Interval,
                    LastStartTime = scheduleTask.LastStartTime,
                    LastEndTime = scheduleTask.LastEndTime,
                    LastSuccessTime = scheduleTask.LastSuccessTime
                };
                return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreateScheduleTask(CreateScheduleTaskDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var scheduleTask = new ScheduleTask
                {
                    Name = dto.Name,
                    Status = TaskRunningStatus.Pending,
                    StatusDesc = "",
                    JobKey = Guid.NewGuid().ToString("N"),
                    JobGroup = Guid.NewGuid().ToString("N"),
                    TriggerKey = Guid.NewGuid().ToString("N"),
                    TriggerGroup = Guid.NewGuid().ToString("N"),
                    IsEnable = false,
                    Type = dto.Type,
                    Cron = dto.Cron,
                    Interval = null,
                    LastStartTime = null,
                    LastEndTime = null,
                    LastSuccessTime = null
                };
                scheduleTask.Id = client.Insertable(scheduleTask).ExecuteReturnBigIdentity();

                _eventPublisher.EntityCreated(scheduleTask);

                return Ok();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateScheduleTask(UpdateScheduleTaskDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(dto.Id);
                if (scheduleTask == null)
                {
                    return Error("找不到该条信息");
                }

                if (scheduleTask.IsEnable)
                {
                    return Error("请先关闭定时任务再删除");
                }

                scheduleTask.Name = dto.Name;
                scheduleTask.Cron = dto.Cron;
                client.Updateable(scheduleTask).UpdateColumns(t => new
                {
                    t.Name,
                    t.Cron
                }).ExecuteCommand();

                _eventPublisher.EntityUpdated(scheduleTask);

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeleteScheduleTask(DeleteScheduleTaskDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(dto.Id);
                if (scheduleTask == null)
                {
                    return Error("找不到该条信息");
                }

                if (scheduleTask.IsEnable)
                {
                    return Error("请先关闭定时任务再删除");
                }

                client.Deleteable<ScheduleTask>(dto.Id).ExecuteCommand();

                _eventPublisher.EntityDeleted(scheduleTask);

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryScheduleTaskItem>> PagedQueryScheduleTasks(
            PagedQueryScheduleTaskDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<ScheduleTask>()
                    .WhereIF(!dto.Keyword.IsNullOrEmpty(), t => t.Name.Contains(dto.Keyword))
                    .Select(t => new QueryScheduleTaskItem()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Status = t.Status,
                        StatusDesc = t.StatusDesc,
                        IsEnable = t.IsEnable,
                        Cron = t.Cron,
                        Interval = t.Interval,
                        LastStartTime = t.LastStartTime,
                        LastEndTime = t.LastEndTime,
                        LastSuccessTime = t.LastSuccessTime,
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize, ref total);
                return new PagedList<QueryScheduleTaskItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
                ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ExecuteScheduleTask(ExecuteScheduleTaskDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(dto.Id);
                if (scheduleTask == null)
                {
                    throw new Exception("找不到数据");
                }

                if (!scheduleTask.IsEnable)
                {
                    throw new Exception("需先开启任务，才能执行");
                }

                var task = _taskScheduleManager.TriggerJob(scheduleTask);
                Task.WaitAll(task);
            }

            return Ok();
        }


        public PublicResult SetIsEnableScheduleTask(SetIsEnableScheduleTaskDto dto)
        {
            ScheduleTask scheduleTask = null;
            using (var client = DbFactory.CreateClient())
            {
                scheduleTask = client.Queryable<ScheduleTask>().InSingle(dto.Id);
            }

            if (scheduleTask == null)
            {
                throw new Exception("没有找到数据");
            }

            if (dto.IsEnable)
            {
                if (scheduleTask.Type.IsNullOrEmpty())
                {
                    throw new Exception("没有找到对应的Job");
                }

                if (scheduleTask.Cron.IsNullOrEmpty())
                {
                    throw new Exception("没有找到对应的CRON表达式");
                }

                var task = _taskScheduleManager.EnableJob(scheduleTask);
                Task.WaitAll(task);
            }
            else
            {
                var task = _taskScheduleManager.DisableJob(scheduleTask);
                Task.WaitAll(task);
            }

            return Ok();
        }

        #endregion
    }
}