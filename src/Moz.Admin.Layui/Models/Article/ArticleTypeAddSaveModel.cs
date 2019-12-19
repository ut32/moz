using System.Collections.Generic;
using Moz.Bus.Models.Articles;

namespace Moz.Presentation.Administration.Models.Articles
{
    public class ArticleTypeAddSaveModel
    {
        public string Name { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowDel { get; set; }
        public List<ArticleConfiguration> List { get; set; }
    }
}