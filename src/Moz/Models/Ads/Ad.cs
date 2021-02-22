using SqlSugar;

namespace Moz.Bus.Models.Ads
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