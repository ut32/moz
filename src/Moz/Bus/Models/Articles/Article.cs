using System;
using SqlSugar;

namespace Moz.Bus.Models.Articles
{
    [SugarTable("tab_article")]
    public class Article : BaseModel
    {
        /// <summary>
        ///     类型
        /// </summary>
        [SugarColumn(ColumnName = "article_type_id")]
        public long ArticleTypeId { get; set; }


        /// <summary>
        ///     类别ID
        /// </summary>
        [SugarColumn(ColumnName = "category_id")]
        public long? CategoryId { get; set; }


        /// <summary>
        ///     标题
        /// </summary>
        [ArticleField("标题", FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }

        /// <summary>
        ///     副标题
        /// </summary>
        [ArticleField("副标题",
            FieldTypeEnum.SingleRowTextInput
            | FieldTypeEnum.Category
            | FieldTypeEnum.Select
            | FieldTypeEnum.Radio
            | FieldTypeEnum.CheckBox
            | FieldTypeEnum.Color
            | FieldTypeEnum.Location
            | FieldTypeEnum.UploadSingleFile
            | FieldTypeEnum.UploadMultipleFiles
            | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "sub_title")]
        public string SubTitle { get; set; }

        /// <summary>
        ///     标题颜色
        /// </summary>
        [ArticleField("颜色", FieldTypeEnum.Color
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput)]
        [SugarColumn(ColumnName = "title_color")]
        public string TitleColor { get; set; }

        /// <summary>
        ///     标题粗体
        /// </summary>
        [ArticleField("标题粗体", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "title_bold")]
        public bool? TitleBold { get; set; }

        /// <summary>
        ///     摘要
        /// </summary>
        [ArticleField("摘要", FieldTypeEnum.MultipleRowsTextInput
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "summary")]
        public string Summary { get; set; }

        /// <summary>
        ///     内容
        /// </summary>
        [ArticleField("内容", FieldTypeEnum.Editor
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "content")]
        public string Content { get; set; }

        /// <summary>
        ///     标签
        /// </summary>
        [ArticleField("标签", FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "tags")]
        public string Tags { get; set; }

        /// <summary>
        ///     缩略图
        /// </summary>
        [ArticleField("缩略图", FieldTypeEnum.UploadSingleFile
                             | FieldTypeEnum.Category
                             | FieldTypeEnum.SingleRowTextInput
                             | FieldTypeEnum.Select
                             | FieldTypeEnum.Radio
                             | FieldTypeEnum.CheckBox
                             | FieldTypeEnum.Color
                             | FieldTypeEnum.Location
                             | FieldTypeEnum.UploadMultipleFiles
                             | FieldTypeEnum.MultipleRowsTextInput)]
        [SugarColumn(ColumnName = "thumb_image")]
        public string ThumbImage { get; set; }


        /// <summary>
        ///     视频
        /// </summary>
        [ArticleField("视频", FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput)]
        [SugarColumn(ColumnName = "video")]
        public string Video { get; set; }

        /// <summary>
        ///     来源
        /// </summary>
        [ArticleField("来源", FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "source")]
        public string Source { get; set; }

        /// <summary>
        ///     作者
        /// </summary>
        [ArticleField("作者", FieldTypeEnum.SingleRowTextInput
                            | FieldTypeEnum.Category
                            | FieldTypeEnum.Select
                            | FieldTypeEnum.Radio
                            | FieldTypeEnum.CheckBox
                            | FieldTypeEnum.Color
                            | FieldTypeEnum.Location
                            | FieldTypeEnum.UploadSingleFile
                            | FieldTypeEnum.UploadMultipleFiles
                            | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "author")]
        public string Author { get; set; }

        /// <summary>
        ///     点击次数
        /// </summary>
        [ArticleField("点击次数", FieldTypeEnum.SingleRowTextInput)]
        [SugarColumn(ColumnName = "hits")]
        public int? Hits { get; set; }

        /// <summary>
        ///     添加时间
        /// </summary>
        [ArticleField("添加时间", FieldTypeEnum.DateTimeInput)]
        [SugarColumn(ColumnName = "addtime")]
        public DateTime? Addtime { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [ArticleField("排序", FieldTypeEnum.SingleRowTextInput)]
        [SugarColumn(ColumnName = "order_index")]
        public int? OrderIndex { get; set; }

        /// <summary>
        ///     是否置顶
        /// </summary>
        [ArticleField("是否置顶", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "is_top")]
        public bool? IsTop { get; set; }

        /// <summary>
        ///     是否推荐
        /// </summary>
        [ArticleField("是否推荐", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "is_recommend")]
        public bool? IsRecommend { get; set; }

        [ArticleField("SeoTitle", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "seo_title")]
        public string SeoTitle { get; set; }

        [ArticleField("SeoKeyword", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "seo_keyword")]
        public string SeoKeyword { get; set; }

        [ArticleField("SeoDescription", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.MultipleRowsTextInput, true)]
        [SugarColumn(ColumnName = "seo_description")]
        public string SeoDescription { get; set; }
        
        
        [ArticleField("String1", FieldTypeEnum.SingleRowTextInput
                                 | FieldTypeEnum.Select
                                 | FieldTypeEnum.Radio
                                 | FieldTypeEnum.CheckBox
                                 | FieldTypeEnum.Category
                                 | FieldTypeEnum.Color
                                 | FieldTypeEnum.Location
                                 | FieldTypeEnum.UploadSingleFile
                                 | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "string_1")]
        public string String1 { get; set; }

        [ArticleField("String2", FieldTypeEnum.SingleRowTextInput
                                 | FieldTypeEnum.Select
                                 | FieldTypeEnum.Radio
                                 | FieldTypeEnum.CheckBox
                                 | FieldTypeEnum.Category
                                 | FieldTypeEnum.Color
                                 | FieldTypeEnum.Location
                                 | FieldTypeEnum.UploadSingleFile
                                 | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "string_2")]
        public string String2 { get; set; }

        [ArticleField("String3", FieldTypeEnum.SingleRowTextInput
                                 | FieldTypeEnum.Select
                                 | FieldTypeEnum.Radio
                                 | FieldTypeEnum.CheckBox
                                 | FieldTypeEnum.Category
                                 | FieldTypeEnum.Color
                                 | FieldTypeEnum.Location
                                 | FieldTypeEnum.UploadSingleFile
                                 | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "string_3")]
        public string String3 { get; set; }

        [ArticleField("String4", FieldTypeEnum.SingleRowTextInput
                                 | FieldTypeEnum.Select
                                 | FieldTypeEnum.Radio
                                 | FieldTypeEnum.CheckBox
                                 | FieldTypeEnum.Category
                                 | FieldTypeEnum.Color
                                 | FieldTypeEnum.Location
                                 | FieldTypeEnum.UploadSingleFile
                                 | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "string_4")]
        public string String4 { get; set; }

        [ArticleField("Int1", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "int_1")]
        public int? Int1 { get; set; }

        [ArticleField("Int2", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "int_2")]
        public int? Int2 { get; set; }

        [ArticleField("Int3", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "int_3")]
        public int? Int3 { get; set; }

        [ArticleField("Int4", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "int_4")]
        public int? Int4 { get; set; }
        
        [ArticleField("Long1", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "long_1")]
        public int? Long1 { get; set; }

        [ArticleField("Long2", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "long_2")]
        public int? Long2 { get; set; }

        [ArticleField("Long3", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "long_3")]
        public int? Long3 { get; set; }

        [ArticleField("Long4", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "long_4")]
        public int? Long4 { get; set; }

        [ArticleField("Decimal1", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "decimal_1")]
        public decimal? Decimal1 { get; set; }

        [ArticleField("Decimal2", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "decimal_2")]
        public decimal? Decimal2 { get; set; }

        [ArticleField("Decimal3", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "decimal_3")]
        public decimal? Decimal3 { get; set; }

        [ArticleField("Decimal4", FieldTypeEnum.SingleRowTextInput | FieldTypeEnum.Select | FieldTypeEnum.Radio)]
        [SugarColumn(ColumnName = "decimal_4")]
        public decimal? Decimal4 { get; set; }

        [ArticleField("DateTime1", FieldTypeEnum.DateTimeInput)]
        [SugarColumn(ColumnName = "datetime_1")]
        public DateTime? Datetime1 { get; set; }

        [ArticleField("DateTime2", FieldTypeEnum.DateTimeInput)]
        [SugarColumn(ColumnName = "datetime_2")]
        public DateTime? Datetime2 { get; set; }

        [ArticleField("DateTime3", FieldTypeEnum.DateTimeInput)]
        [SugarColumn(ColumnName = "datetime_3")]
        public DateTime? Datetime3 { get; set; }

        [ArticleField("DateTime4", FieldTypeEnum.DateTimeInput)]
        [SugarColumn(ColumnName = "datetime_4")]
        public DateTime? Datetime4 { get; set; }

        [ArticleField("Bool1", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "bool_1")]
        public bool? Bool1 { get; set; }

        [ArticleField("Bool2", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "bool_2")]
        public bool? Bool2 { get; set; }

        [ArticleField("Bool3", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "bool_3")]
        public bool? Bool3 { get; set; }

        [ArticleField("Bool4", FieldTypeEnum.CheckBox)]
        [SugarColumn(ColumnName = "bool_4")]
        public bool? Bool4 { get; set; }

        [ArticleField("Text1",
            FieldTypeEnum.Editor | FieldTypeEnum.MultipleRowsTextInput | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "text_1")]
        public string Text1 { get; set; }

        [ArticleField("Text2",
            FieldTypeEnum.Editor | FieldTypeEnum.MultipleRowsTextInput | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "text_2")]
        public string Text2 { get; set; }

        [ArticleField("Text3",
            FieldTypeEnum.Editor | FieldTypeEnum.MultipleRowsTextInput | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "text_3")]
        public string Text3 { get; set; }

        [ArticleField("Text4",
            FieldTypeEnum.Editor | FieldTypeEnum.MultipleRowsTextInput | FieldTypeEnum.UploadMultipleFiles, true)]
        [SugarColumn(ColumnName = "text_4")]
        public string Text4 { get; set; }
    }
}