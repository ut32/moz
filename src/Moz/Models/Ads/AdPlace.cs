using System;
using SqlSugar;

namespace Moz.Bus.Models.Ads
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