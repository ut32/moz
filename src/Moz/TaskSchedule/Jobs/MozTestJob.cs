using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Moz.TaskSchedule.Jobs
{
    [DisallowConcurrentExecution]
    [Description("测试定时任务")]
    public class MozTestJob : IJob
    {
        private readonly IConfiguration _configuration;

        public MozTestJob(IConfiguration configuration)
        {
            Console.WriteLine("MozTestJob >..");
            _configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("测试定时任务");
            return Task.CompletedTask;
        }
    }
}