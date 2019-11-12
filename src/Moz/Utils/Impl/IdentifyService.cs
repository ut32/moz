using System;
using Moz.Bus.Models.Common;
using Moz.DataBase;

namespace Moz.Utils.Impl
{
    internal class IdentifyService:IIdentifyService
    {
        public long GetUnqId()
        {
            using (var db = DbFactory.GetClient())
            {
                return db.Insertable(new Identify()).ExecuteReturnBigIdentity();
            }
        }

        public string GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string GetSnowId()
        {
            throw new System.NotImplementedException();
        }
    }
}