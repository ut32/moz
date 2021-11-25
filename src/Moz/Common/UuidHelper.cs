using System;
using Moz.Core;

namespace Moz.Common
{
    /// <summary>
    /// 全局唯一ID生成器
    /// </summary>
    public static class UuidHelper
    {
        /// <summary>
        /// 微软guid, 无下划线 
        /// </summary>
        public static string Guid => 
            System.Guid.NewGuid().ToString("N");

        /// <summary>
        /// 雪花ID
        /// </summary>
        public static long SnowId
        {
            get
            {
                var snowIdWorker = EngineContext.Current.Resolve<SnowIdWorker>();
                return snowIdWorker.NextId();
            }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public static long TimeStamp =>
            DateTime.Now.ToUnixTime();

    }
}