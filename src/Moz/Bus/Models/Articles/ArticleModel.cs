using SqlSugar;

namespace Moz.Bus.Models.Articles
{
    [SugarTable("tab_article_model")]
    public class ArticleModel : BaseModel
    {
        public string Name { get; set; }

        [SugarColumn(ColumnName = "category_id")]
        public long? CategoryId { get; set; }

        public string Configuration { get; set; }
        
        
    }
}