using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Moz.Core;
using Moz.Exceptions;
using SqlSugar;

namespace Moz.DataBase
{
    public sealed class DbClient : SqlSugarClient
    {
        internal DbClient(DbType dbType,string masterConnectionString,List<SlaveConnectionConfig> slaveConnectionConfigs)
            : base(new ConnectionConfig
            {
                ConnectionString = masterConnectionString, 
                DbType = dbType,
                InitKeyType = InitKeyType.SystemTable,
                IsAutoCloseConnection = false,
                SlaveConnectionConfigs = slaveConnectionConfigs
            })
        {
            var hostingEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();
            if (hostingEnvironment.IsDevelopment())
            {
                Aop.OnLogExecuting = (sql, pars) =>
                {
                    var newSql = sql;
                    foreach (var sugarParameter in pars)
                    {
                        newSql = newSql.Replace(sugarParameter.ParameterName, sugarParameter.Value?.ToString());
                    }
                    Console.WriteLine(newSql);
                };
                Aop.OnError = exp =>
                {
                    Console.WriteLine("SqlSugar Exception : ",exp);
                };
            }
        }

        public T UseTran<T>(Func<SqlSugarClient, T> fun)
        {
            try
            {
                Ado.BeginTran();
                var t = fun(this);
                Ado.CommitTran();
                return t;
            }
            catch (Exception ex)
            {
                Ado.RollbackTran();
                throw new FatalException(ex.Message);
            }
        }
    }
}