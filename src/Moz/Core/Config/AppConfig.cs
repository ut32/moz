using System;
using System.Collections.Generic;
using Moz.Exceptions;

namespace Moz.Core.Config
{
    public class AppConfig
    {
        public AppConfig()
        {
            Admin = new AdminConfig();
            ErrorPage = new ErrorPageConfig();
            Db = new List<DbConfig>();
            Token = new TokenConfig();
        }


        /// <summary>
        /// 是否开启定时任务 默认不开启
        /// </summary>
        public bool IsEnableScheduling { get; set; } 
        
        /// <summary>
        /// 是否开启性能监测
        /// </summary>
        public bool IsEnablePerformanceMonitor { get; set; }
          
        /// <summary>
        /// AppSecret, 要求为16-32位 ，关系到应用安全，非常重要
        /// 在线生成 https://ut32.com/tool/pwd
        /// </summary>
        public string AppSecret { get; set; } 

        /// <summary>
        /// 数据库相关配置
        /// </summary>
        public List<DbConfig> Db { get; }  
        
        /// <summary>
        /// 后台配置
        /// </summary>
        public AdminConfig Admin { get; }
        
        /// <summary>
        /// 错误页面配置
        /// </summary>
        public ErrorPageConfig ErrorPage { get; } 
        
        /// <summary>
        /// JWT配置
        /// </summary>
        public TokenConfig Token { get; set; }
        
        /// <summary>
        /// 异常Http Code处理
        /// </summary>
        internal Type StatusCodePageHandlerType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterStatusCodePageHandler<T>()
            where T:IStatusCodePageHandler
        { 
            StatusCodePageHandlerType = typeof(T);
        }
        
    }
}