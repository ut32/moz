using System.Collections.Generic;
using SqlSugar;

namespace Moz.Core.Config
{
    public class DbConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "default";

        /// <summary>
        /// 数据库类型，默认为mysql
        /// </summary>
        public SqlSugar.DbType Type { get; set; } = DbType.MySql;
        
        /// <summary>
        /// 主数据库
        /// </summary>
        public string MasterConnectionString { get; set; }
        
        /// <summary>
        /// 从数据库
        /// key   : 链接字符串
        /// value : 权重
        /// </summary>
        public Dictionary<string,int> SlavesConnectionStrings { get; } = new Dictionary<string, int>();
    }
}