using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Quartz;

namespace Moz.TaskSchedule.Jobs
{
    [DisallowConcurrentExecution]
    [Description("测试定时任务")]
    public class MozTestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("测试定时任务");
            return Task.CompletedTask;
        }
    }
}