namespace Moz.Core.Configs
{
    /// <summary>
    /// 雪花ID配置
    /// </summary>
    public class SnowIdWorkerConfig
    {
        public long WorkerId { get; set; }
        public long DataCenterId { get; set; } 
    }
}