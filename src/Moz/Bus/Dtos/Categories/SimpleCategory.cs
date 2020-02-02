using System.Collections.Generic;

namespace Moz.Bus.Dtos.Categories
{

    public class SimpleCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public List<SimpleCategory> Children { get; set; }
    }
}