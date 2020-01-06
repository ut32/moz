using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Biz.Dtos.ScheduleTasks;
using Moz.Bus.Dtos.ScheduleTasks;
using Moz.Bus.Models.ScheduleTasks;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;
using Moz.TaskSchedule;

namespace Moz.CMS.Services.ScheduleTasks
{
   public partial class ScheduleTaskService : IScheduleTaskService
    {
        #region Constants

        #endregion

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Ctor
        public ScheduleTaskService(
            IEventPublisher eventPublisher,
            IDistributedCache distributedCache)
        {
            this._eventPublisher = eventPublisher;
            this._distributedCache = distributedCache;
        }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetScheduleTaskDetailResponse GetScheduleTaskDetail(GetScheduleTaskDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                 var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Id);
                 if(scheduleTask == null)
                 {
                    return null;
                 }
                 var resp = new GetScheduleTaskDetailResponse();
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
        public CreateScheduleTaskResponse CreateScheduleTask(CreateScheduleTaskRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = new ScheduleTask
                {
                    Name = request.Name,
                    Status = TaskRunningStatus.Pending,
                    StatusDesc = "",
                    JobKey = Guid.NewGuid().ToString("N"),
                    JobGroup = Guid.NewGuid().ToString("N"),
                    TriggerKey = Guid.NewGuid().ToString("N"),
                    TriggerGroup = Guid.NewGuid().ToString("N"),
                    IsEnable = false,
                    Type = request.Type,
                    Cron = request.Cron,
                    Interval = null,
                    LastStartTime = null,
                    LastEndTime = null,
                    LastSuccessTime = null
                };
                scheduleTask.Id = client.Insertable(scheduleTask).ExecuteReturnBigIdentity();
                
                //_cacheManager.RemoveOnEntityCreated<ScheduleTask>();
                _eventPublisher.EntityCreated(scheduleTask);
                
                return new CreateScheduleTaskResponse();
            }
        }
        
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpdateScheduleTaskResponse UpdateScheduleTask(UpdateScheduleTaskRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Id);
                if (scheduleTask == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (scheduleTask.IsEnable)
                {
                    throw new MozException("请先关闭定时任务再删除");
                }

                scheduleTask.Name = request.Name;
                scheduleTask.Cron = request.Cron;
                client.Updateable( scheduleTask).UpdateColumns(t=>new
                {
                    t.Name,
                    t.Cron
                }).ExecuteCommand();
                
                //_cacheManager.RemoveOnEntityUpdated<ScheduleTask>(request.Id);
                _eventPublisher.EntityUpdated(scheduleTask);
                
                return new UpdateScheduleTaskResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteScheduleTaskResponse DeleteScheduleTask(DeleteScheduleTaskRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Id);
                if (scheduleTask == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (scheduleTask.IsEnable)
                {
                    throw new MozException("请先关闭定时任务再删除");
                }

                client.Deleteable<ScheduleTask>(request.Id).ExecuteCommand();
                
                //_cacheManager.RemoveOnEntityDeleted<ScheduleTask>(request.Id);
                _eventPublisher.EntityDeleted(scheduleTask);
                
                return new DeleteScheduleTaskResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryScheduleTaskResponse PagedQueryScheduleTasks(PagedQueryScheduleTaskRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<ScheduleTask>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryScheduleTaskItem()
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
                    .ToPageList(page, pageSize,ref total);
                return new PagedQueryScheduleTaskResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteScheduleTaskResponse ExecuteScheduleTask(ExecuteScheduleTaskRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Id);
                if(scheduleTask == null)
                {
                    throw new Exception("找不到数据");
                }

                if (!scheduleTask.IsEnable)
                {
                    throw new Exception("需先开启任务，才能执行");
                }

                var task = TaskScheduleManager.Instance.TriggerJob(scheduleTask);
                Task.WaitAll(task);
            }
            return new ExecuteScheduleTaskResponse();
        }


        public SetIsEnableScheduleTaskResponse SetIsEnableScheduleTask(SetIsEnableScheduleTaskRequest request)
        {
            ScheduleTask scheduleTask = null;
            using (var client = DbFactory.GetClient())
            {
                scheduleTask = client.Queryable<ScheduleTask>().InSingle(request.Id);
            }

            if (scheduleTask == null)
            {
                throw new Exception("没有找到数据");
            }

            if (request.IsEnable)
            {
                if (scheduleTask.Type.IsNullOrEmpty())
                {
                    throw new Exception("没有找到对应的Job");
                }

                if (scheduleTask.Cron.IsNullOrEmpty())
                {
                    throw new Exception("没有找到对应的CRON表达式");
                }

                var task = TaskScheduleManager.Instance.EnableJob(scheduleTask);
                Task.WaitAll(task);
            }
            else
            {
                var task = TaskScheduleManager.Instance.DisableJob(scheduleTask);
                Task.WaitAll(task);
            }
            return new SetIsEnableScheduleTaskResponse();
        }
        
        #endregion
    }
}