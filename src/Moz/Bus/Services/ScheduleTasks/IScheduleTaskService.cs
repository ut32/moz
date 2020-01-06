using Moz.Biz.Dtos.ScheduleTasks;
using Moz.Bus.Dtos.ScheduleTasks;

namespace Moz.CMS.Services.ScheduleTasks
{
    public interface IScheduleTaskService
    {
        CreateScheduleTaskResponse CreateScheduleTask(CreateScheduleTaskRequest request);
        UpdateScheduleTaskResponse UpdateScheduleTask(UpdateScheduleTaskRequest request);
        DeleteScheduleTaskResponse DeleteScheduleTask(DeleteScheduleTaskRequest request);
        GetScheduleTaskDetailResponse GetScheduleTaskDetail(GetScheduleTaskDetailRequest request);
        PagedQueryScheduleTaskResponse PagedQueryScheduleTasks(PagedQueryScheduleTaskRequest request);

        ExecuteScheduleTaskResponse ExecuteScheduleTask(ExecuteScheduleTaskRequest request);

        SetIsEnableScheduleTaskResponse SetIsEnableScheduleTask(SetIsEnableScheduleTaskRequest request);
    }
}