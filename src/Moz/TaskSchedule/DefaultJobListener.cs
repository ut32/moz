using System;
using System.Threading;
using System.Threading.Tasks;
using Moz.Biz.Models.ScheduleTasks;
using Moz.DataBase;
using Quartz;

namespace Moz.TaskSchedule
{
    public class DefaultJobListener : IJobListener
    {
        public async Task JobToBeExecuted(IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var jobKey = context.JobDetail.Key.Name;
                using (var client = DbFactory.GetClient())
                {
                    var scheduleTask = client.Queryable<ScheduleTask>().Single(t => t.JobKey == jobKey);
                    if (scheduleTask != null)
                    {
                        scheduleTask.LastStartTime = DateTime.Now;
                        client.Updateable(scheduleTask).UpdateColumns(t => new
                        {
                            t.LastStartTime
                        }).ExecuteCommand();
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            await Task.Delay(0, cancellationToken);
        }

        public async Task JobExecutionVetoed(IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await Task.Delay(0, cancellationToken);
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (jobException != null)
                {
                    Console.WriteLine(jobException);
                    jobException.UnscheduleAllTriggers = true;
                }

                var jobKey = context.JobDetail.Key.Name;
                using (var client = DbFactory.GetClient())
                {
                    var scheduleTask = client.Queryable<ScheduleTask>().Single(t => t.JobKey == jobKey);
                    if (scheduleTask != null)
                    {
                        if (jobException == null)
                        {
                            scheduleTask.Status = TaskRunningStatus.Running;
                            scheduleTask.StatusDesc = "";
                            scheduleTask.LastEndTime = DateTime.Now;
                            scheduleTask.LastSuccessTime = DateTime.Now;
                        }
                        else
                        {
                            scheduleTask.Status = TaskRunningStatus.Error;
                            scheduleTask.StatusDesc = jobException.GetBaseException()?.Message ?? "";
                            scheduleTask.LastEndTime = DateTime.Now;
                            scheduleTask.IsEnable = false;
                        }

                        client.Updateable(scheduleTask).UpdateColumns(t => new
                        {
                            t.Status,
                            t.StatusDesc,
                            t.LastEndTime,
                            t.LastSuccessTime,
                            t.IsEnable
                        }).ExecuteCommand();
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            await Task.Delay(0, cancellationToken);
        }

        public string Name => "DefaultJobListener";
    }
}