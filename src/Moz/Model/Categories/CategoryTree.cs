using System.Collections.Generic;

namespace Moz.Bus.Models.Categories
{

    public class CategoryTree
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public List<CategoryTree> Children { get; set; }
    }
}