using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Models;
using SqlSugar;

namespace Moz.CMS.Model.Ad
{
    [SugarTable("tab_ad")]
    public class Ad:BaseModel
    {
        [SugarColumn(ColumnName = "ad_place_id")]
        public long AdPlaceId { get; set; }
        
        public string Title { get; set; }
        
        [SugarColumn(ColumnName = "image_path")]
        public string ImagePath { get; set; }
        
        [SugarColumn(ColumnName = "target_url")]
        public string TargetUrl { get; set; }
        
        [SugarColumn(ColumnName = "order_index")]
        public int OrderIndex { get; set; } 
        
        [SugarColumn(ColumnName = "is_show")]
        public bool IsShow { get; set; }
    }
}