using System;
using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.CMS.Model.Ad
{
    [SugarTable("tab_ad_place")]
    public class AdPlace:BaseModel
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public DateTime Addtime { get; set; } 
    }
}