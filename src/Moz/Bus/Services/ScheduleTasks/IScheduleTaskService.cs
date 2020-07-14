using Moz.Bus.Dtos;
using Moz.Bus.Dtos.ScheduleTasks;

namespace Moz.Bus.Services.ScheduleTasks
{
    public interface IScheduleTaskService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult CreateScheduleTask(CreateScheduleTaskDto dto);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateScheduleTask(UpdateScheduleTaskDto dto);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult DeleteScheduleTask(DeleteScheduleTaskDto dto);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<ScheduleTaskDetailApo> GetScheduleTaskDetail(GetScheduleTaskDetailDto dto);
        
        /// <summary> 
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryScheduleTaskItem>> PagedQueryScheduleTasks(PagedQueryScheduleTaskDto dto);
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult ExecuteScheduleTask(ExecuteScheduleTaskDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetIsEnableScheduleTask(SetIsEnableScheduleTaskDto dto);
    }
}