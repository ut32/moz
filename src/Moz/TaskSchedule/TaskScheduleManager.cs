using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Moz.Bus.Models.ScheduleTasks;
using Moz.DataBase;
using Quartz;
using Quartz.Impl;

namespace Moz.TaskSchedule
{
    internal class TaskScheduleManager:ITaskScheduleManager
    {
        private readonly IScheduler _scheduler;
        public TaskScheduleManager()
        {
            var props = new NameValueCollection
            {
                {"quartz.serializer.type", "binary"}
            };
            var factory = new StdSchedulerFactory(props);
            _scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            _scheduler.ListenerManager.AddJobListener(new DefaultJobListener());
        }
        
        public async Task Init()
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<ScheduleTask>().Where(t => t.IsEnable).ToList();
                if (list.Any())
                {
                    foreach (var scheduleTask in list)
                    {
                        scheduleTask.Status = TaskRunningStatus.Running;
                        scheduleTask.StatusDesc = "";
                        scheduleTask.JobKey = Guid.NewGuid().ToString("N");
                        scheduleTask.JobGroup = Guid.NewGuid().ToString("N");
                        scheduleTask.TriggerKey = Guid.NewGuid().ToString("N");
                        scheduleTask.TriggerGroup = Guid.NewGuid().ToString("N");
                        scheduleTask.LastEndTime = null;
                        scheduleTask.LastStartTime = null;
                        scheduleTask.LastSuccessTime = null;

                        var type = Type.GetType(scheduleTask.Type, false, true);
                        if (type == null)
                        {
                            scheduleTask.Status = TaskRunningStatus.Error;
                            scheduleTask.StatusDesc = "反射类型失败";
                            continue;
                        }

                        var job = JobBuilder.Create(type)
                            .WithIdentity(scheduleTask.JobKey, scheduleTask.JobGroup)
                            .Build();

                        ITrigger trigger = null;
                        if (!string.IsNullOrEmpty(scheduleTask.Cron))
                        {
                            trigger = TriggerBuilder.Create()
                                .WithIdentity(scheduleTask.TriggerKey, scheduleTask.TriggerGroup)
                                .StartNow()
                                .WithCronSchedule(scheduleTask.Cron)
                                .Build();
                        }
                        else if (scheduleTask.Interval != null && scheduleTask.Interval.Value > 0)
                        {
                            trigger = TriggerBuilder.Create()
                                .WithIdentity(scheduleTask.TriggerKey, scheduleTask.TriggerGroup)
                                .StartNow()
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInSeconds(scheduleTask.Interval.Value)
                                    .RepeatForever())
                                .Build();
                        }
                        else
                        {
                            scheduleTask.Status = TaskRunningStatus.Error;
                            scheduleTask.StatusDesc = "找不到运行策略";
                            continue;
                        }

                        await _scheduler.ScheduleJob(job, trigger);
                    }

                    client.Updateable(list).ExecuteCommand();
                }

                await _scheduler.Start();
            }
        }
        public async Task ResumeJob(ScheduleTask scheduleTask)
        {
            await _scheduler.ResumeJob(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
        }
        public async Task PauseJob(ScheduleTask scheduleTask) 
        {
            await _scheduler.PauseJob(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
        }
        public async Task TriggerJob(ScheduleTask scheduleTask)
        {
            await _scheduler.TriggerJob(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
        }
        public async Task DisableJob(ScheduleTask scheduleTask)
        {
            if (!scheduleTask.TriggerKey.IsNullOrEmpty() && !scheduleTask.JobGroup.IsNullOrEmpty())
            {
                var isExist = await _scheduler.CheckExists(new TriggerKey(scheduleTask.TriggerKey, scheduleTask.TriggerGroup));
                if (isExist)
                {
                    await _scheduler.UnscheduleJob(new TriggerKey(scheduleTask.TriggerKey, scheduleTask.TriggerGroup));
                }
            }

            if (!scheduleTask.JobKey.IsNullOrEmpty() && !scheduleTask.JobGroup.IsNullOrEmpty())
            {
                var isExist = await _scheduler.CheckExists(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
                if (isExist)
                {
                    await _scheduler.DeleteJob(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
                }
            }
            
            scheduleTask.IsEnable = false;
            scheduleTask.Status = TaskRunningStatus.Pending;
            scheduleTask.StatusDesc = "";
            scheduleTask.TriggerKey = "";
            scheduleTask.TriggerGroup = "";
            scheduleTask.JobKey = "";
            scheduleTask.JobGroup = "";
            
            using (var client = DbFactory.GetClient())
            {
                client.Updateable(scheduleTask).ExecuteCommand();
            }
        }
        public async Task EnableJob(ScheduleTask scheduleTask)
        {
            if (!scheduleTask.TriggerKey.IsNullOrEmpty() && !scheduleTask.JobGroup.IsNullOrEmpty())
            {
                var isExist = await _scheduler.CheckExists(new TriggerKey(scheduleTask.TriggerKey, scheduleTask.TriggerGroup));
                if (isExist)
                {
                    await _scheduler.UnscheduleJob(new TriggerKey(scheduleTask.TriggerKey, scheduleTask.TriggerGroup));
                }
            }

            if (!scheduleTask.JobKey.IsNullOrEmpty() && !scheduleTask.JobGroup.IsNullOrEmpty())
            {
                var isExist = await _scheduler.CheckExists(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
                if (isExist)
                {
                    await _scheduler.DeleteJob(new JobKey(scheduleTask.JobKey, scheduleTask.JobGroup));
                }
            }

            scheduleTask.IsEnable = true;
            scheduleTask.Status = TaskRunningStatus.Running;
            scheduleTask.StatusDesc = "";
            scheduleTask.JobKey = Guid.NewGuid().ToString("N");
            scheduleTask.JobGroup = Guid.NewGuid().ToString("N");
            scheduleTask.TriggerKey = Guid.NewGuid().ToString("N");
            scheduleTask.TriggerGroup = Guid.NewGuid().ToString("N");

            var type = Type.GetType(scheduleTask.Type, false, true);
            if (type == null)
            {
                scheduleTask.IsEnable = false;
                scheduleTask.Status = TaskRunningStatus.Error;
                scheduleTask.StatusDesc = "反射类型失败";
                goto end;
            }

            var job = JobBuilder.Create(type)
                .WithIdentity(scheduleTask.JobKey, scheduleTask.JobGroup)
                .Build();

            ITrigger trigger = null;
            if (!string.IsNullOrEmpty(scheduleTask.Cron))
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity(scheduleTask.TriggerKey, scheduleTask.TriggerGroup)
                    .StartNow()
                    .WithCronSchedule(scheduleTask.Cron)
                    .Build();
            }
            else if (scheduleTask.Interval != null && scheduleTask.Interval.Value > 0)
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity(scheduleTask.TriggerKey, scheduleTask.TriggerGroup)
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(scheduleTask.Interval.Value)
                        .RepeatForever())
                    .Build();
            }
            else
            {
                scheduleTask.IsEnable = false;
                scheduleTask.Status = TaskRunningStatus.Error;
                scheduleTask.StatusDesc = "找不到运行策略";
                goto end;
            }

            await _scheduler.ScheduleJob(job, trigger);

            end:
            {
                using (var client = DbFactory.GetClient())
                {
                    client.Updateable(scheduleTask).ExecuteCommand();
                }

                if (scheduleTask.Status == TaskRunningStatus.Error)
                    throw new Exception(scheduleTask.StatusDesc);
            }
        }
        
    }
}