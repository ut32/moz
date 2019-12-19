namespace Moz.Administration.Models.Tasks
{
    public class QueryModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string Keyword { get; set; }
    }
}