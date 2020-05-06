using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Options;
using Moz.Common;
using Moz.Core;
using Moz.Core.Config;
using Moz.Utils;
using SqlSugar;
using DbType = SqlSugar.DbType;

namespace Moz.DataBase
{
    public static class DbFactory
    {
        private static bool? _isInstalled = false;

        public static DbClient GetClient(string name = "default")
        {

            var options = EngineContext.Current.Resolve<IOptions<AppConfig>>()?.Value;
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var dbOption = options.Db.FirstOrDefault(it => it.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (dbOption == null)
                throw new ArgumentNullException(nameof(dbOption));

            var client = new DbClient(dbOption.Type,
                dbOption.MasterConnectionString,
                dbOption.SlavesConnectionStrings?.Select(t => new SlaveConnectionConfig()
                {
                    HitRate = t.Value,
                    ConnectionString = t.Key
                }).ToList());
            
            return client;
        }
        
        public static bool CheckInstalled(AppConfig appConfig) 
        {
            if(_isInstalled != null && _isInstalled.Value) return true;
            try 
            {
                var dbOption = appConfig.Db.FirstOrDefault(it => it.Name.Equals("default", StringComparison.OrdinalIgnoreCase));
                
                if (dbOption == null)
                    return false;
                
                if(dbOption.MasterConnectionString.IsNullOrEmpty())
                    return false;

                using (var db = new SqlSugarClient(new ConnectionConfig
                {
                    DbType = dbOption.Type,
                    ConnectionString = dbOption.MasterConnectionString
                }))
                {
                    _isInstalled = db.DbMaintenance.IsAnyTable("tab_member", false);
                    return _isInstalled.Value;
                }
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}