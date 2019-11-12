using Moz.CMS.Models;
using SqlSugar;

namespace Moz.Bus.Models.Common
{
    /// <summary>
    /// tab_service_performance
    /// </summary>
    [SugarTable("tab_service_performance")]
    public class ServicePerformanceMonitor : BaseModel
    {
        #region 属性
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="elapsed_ms")]
        public int ElapsedMs { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="http_request_id")]
        public string HttpRequestId { get;set; } 
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName="add_time")]
        public System.DateTime AddTime { get;set; } 
        
        #endregion     
    }
}
