using Moz.Bus.Dtos;
using Moz.Bus.Dtos.ScheduleTasks;

namespace Moz.Bus.Services.ScheduleTasks
{
    public interface IScheduleTaskService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult CreateScheduleTask(ServRequest<CreateScheduleTaskDto> request);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult UpdateScheduleTask(ServRequest<UpdateScheduleTaskDto> request);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult DeleteScheduleTask(ServRequest<DeleteScheduleTaskDto> request);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<ScheduleTaskDetailApo> GetScheduleTaskDetail(ServRequest<GetScheduleTaskDetailDto> request);
        
        /// <summary> 
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<PagedList<QueryScheduleTaskItem>> PagedQueryScheduleTasks(ServRequest<PagedQueryScheduleTaskDto> request);
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult ExecuteScheduleTask(ServRequest<ExecuteScheduleTaskDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetIsEnableScheduleTask(ServRequest<SetIsEnableScheduleTaskDto> request);
    }
}