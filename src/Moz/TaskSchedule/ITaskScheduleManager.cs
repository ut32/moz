using System.Threading.Tasks;
using Moz.Bus.Models.ScheduleTasks;

namespace Moz.TaskSchedule
{
    public interface ITaskScheduleManager
    {
        Task Init();
        Task ResumeJob(ScheduleTask scheduleTask);

        Task PauseJob(ScheduleTask scheduleTask);

        Task TriggerJob(ScheduleTask scheduleTask);

        Task DisableJob(ScheduleTask scheduleTask);

        Task EnableJob(ScheduleTask scheduleTask);
    }
}