namespace Moz.Biz.Dtos.Categories
{
    public class QueryRequest
    {
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    
    public class QueryResponse
    {
        public int Total { get; set; }
        public dynamic List { get; set; }
    }
}