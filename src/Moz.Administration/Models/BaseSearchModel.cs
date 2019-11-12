namespace Moz.Presentation.Administration.Models
{
    public class BaseSearchModel
    {
        public int? Page { get; set; }

        public int? NumPerPage { get; set; }

        public string OrderField { get; set; }

        public string OrderDirection { get; set; }

        /// <summary>
        ///     搜索类型，如果为lookup ， 表示查找返回
        /// </summary>
        public OpenTypeEnum OpenType { get; set; }
    }

    public enum OpenTypeEnum
    {
        Default = 0,
        Dialog = 1
    }
}