using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Options;
using Moz.Common;
using Moz.Configuration;
using Moz.Core;
using Moz.Core.Options;
using Moz.Utils;
using SqlSugar;
using DbType = SqlSugar.DbType;

namespace Moz.DataBase
{
    public static class DbFactory
    {
        public static DbClient GetClient(string name = "default")
        {

            var options = EngineContext.Current.Resolve<IOptions<MozOptions>>()?.Value;
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

        public static bool IsInstalled(string name = "default")
        {
            using (var db = GetClient(name))
            {
                return db.DbMaintenance.IsAnyTable("tab_member", false);
            }
        }
    }
}