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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<ScheduleTaskDetailApo> GetScheduleTaskDetail(ServRequest<GetScheduleTaskDetailDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Data.Id);
                if (scheduleTask == null)
                {
                    return Error("找不到该数据");
                }

                var resp = new ScheduleTaskDetailApo();
                resp.Id = scheduleTask.Id;
                resp.Name = scheduleTask.Name;
                resp.Status = scheduleTask.Status;
                resp.StatusDesc = scheduleTask.StatusDesc;
                resp.JobKey = scheduleTask.JobKey;
                resp.JobGroup = scheduleTask.JobGroup;
                resp.TriggerKey = scheduleTask.TriggerKey;
                resp.TriggerGroup = scheduleTask.TriggerGroup;
                resp.IsEnable = scheduleTask.IsEnable;
                resp.Type = scheduleTask.Type;
                resp.Cron = scheduleTask.Cron;
                resp.Interval = scheduleTask.Interval;
                resp.LastStartTime = scheduleTask.LastStartTime;
                resp.LastEndTime = scheduleTask.LastEndTime;
                resp.LastSuccessTime = scheduleTask.LastSuccessTime;
                return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult CreateScheduleTask(ServRequest<CreateScheduleTaskDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = new ScheduleTask
                {
                    Name = request.Data.Name,
                    Status = TaskRunningStatus.Pending,
                    StatusDesc = "",
                    JobKey = Guid.NewGuid().ToString("N"),
                    JobGroup = Guid.NewGuid().ToString("N"),
                    TriggerKey = Guid.NewGuid().ToString("N"),
                    TriggerGroup = Guid.NewGuid().ToString("N"),
                    IsEnable = false,
                    Type = request.Data.Type,
                    Cron = request.Data.Cron,
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult UpdateScheduleTask(ServRequest<UpdateScheduleTaskDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Data.Id);
                if (scheduleTask == null)
                {
                    return Error("找不到该条信息");
                }

                if (scheduleTask.IsEnable)
                {
                    return Error("请先关闭定时任务再删除");
                }

                scheduleTask.Name = request.Data.Name;
                scheduleTask.Cron = request.Data.Cron;
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult DeleteScheduleTask(ServRequest<DeleteScheduleTaskDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Data.Id);
                if (scheduleTask == null)
                {
                    return Error("找不到该条信息");
                }

                if (scheduleTask.IsEnable)
                {
                    return Error("请先关闭定时任务再删除");
                }

                client.Deleteable<ScheduleTask>(request.Data.Id).ExecuteCommand();

                _eventPublisher.EntityDeleted(scheduleTask);

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<PagedList<QueryScheduleTaskItem>> PagedQueryScheduleTasks(
            ServRequest<PagedQueryScheduleTaskDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<ScheduleTask>()
                    .WhereIF(!request.Data.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Data.Keyword))
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
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult ExecuteScheduleTask(ServRequest<ExecuteScheduleTaskDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Data.Id);
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


        public ServResult SetIsEnableScheduleTask(ServRequest<SetIsEnableScheduleTaskDto> request)
        {
            ScheduleTask scheduleTask = null;
            using (var client = DbFactory.GetClient())
            {
                scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Data.Id);
            }

            if (scheduleTask == null)
            {
                throw new Exception("没有找到数据");
            }

            if (request.Data.IsEnable)
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