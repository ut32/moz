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
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(1000);
            Console.WriteLine("测试定时任务");
        }
    }
}