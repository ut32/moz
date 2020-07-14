using System;
using System.Collections.Generic;
using Moz.Exceptions;

namespace Moz.Core.Config
{
    public class AppConfig
    {
        public AppConfig()
        {
            ErrorPage = new ErrorPageConfig
            {
                HttpErrors = new List<HttpError>()
            };
            Db = new List<DbConfig>();
            Token = new TokenConfig();
        }
        
        /// <summary>
        /// 后台路径
        /// </summary>
        public string AdminPath { get; set; }
        
        
        /// <summary>
        /// 工具箱路径
        /// </summary>
        public string ToolsPath { get; set; }
        

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
        /// 错误页面配置
        /// </summary>
        public ErrorPageConfig ErrorPage { get; } 
        
        /// <summary>
        /// JWT配置
        /// </summary>
        public TokenConfig Token { get; }
        
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