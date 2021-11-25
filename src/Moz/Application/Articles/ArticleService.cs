using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Biz.Dtos.Articles;
using Moz.Biz.Dtos.Articles.ArticleModels;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Articles;
using Moz.Bus.Dtos.Articles.ArticleModels;
using Moz.Bus.Dtos.Categories;
using Moz.Bus.Models.Articles;
using Moz.Bus.Services.Categories;
using Moz.Common;
using Moz.DataBase;
using Moz.Domain.Dtos.Articles.ArticleModels;
using Moz.Events;
using Moz.Exceptions;
using Moz.Utils;
using Newtonsoft.Json;
using SqlSugar;

namespace Moz.Bus.Services.Articles
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ArticleService : BaseService,IArticleService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICategoryService _categoryService;

        private readonly string CACHE_KEY_ARTICLE_TYPE_ID = "CACHE_KEY_ARTICLE_TYPE_ID_{0}";


        public ArticleService(
            IDistributedCache distributedCache,
            IEventPublisher eventPublisher,
            ICategoryService categoryService)
        {
            this._distributedCache = distributedCache;
            this._eventPublisher = eventPublisher;
            _categoryService = categoryService;
        }

        #region 文章

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetArticleDetailResponse GetArticleDetail(GetArticleDetailRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                 var article = client.Queryable<Article>().InSingle(request.Id);
                 if(article == null)
                 {
                    return null;
                 }
                 var resp = new GetArticleDetailResponse();
                 resp.Id = article.Id;
                 resp.ArticleTypeId = article.ArticleTypeId;
                 resp.CategoryId = article.CategoryId;
                 resp.Title = article.Title;
                 resp.SubTitle = article.SubTitle;
                 resp.TitleColor = article.TitleColor;
                 resp.TitleBold = article.TitleBold;
                 resp.Summary = article.Summary;
                 resp.Content = article.Content;
                 resp.Tags = article.Tags;
                 resp.ThumbImage = article.ThumbImage;
                 resp.Video = article.Video;
                 resp.Source = article.Source;
                 resp.Author = article.Author;
                 resp.Hits = article.Hits;
                 resp.Addtime = article.Addtime;
                 resp.OrderIndex = article.OrderIndex;
                 resp.IsTop = article.IsTop;
                 resp.IsRecommend = article.IsRecommend;
                 resp.SeoTitle = article.SeoTitle;
                 resp.SeoKeyword = article.SeoKeyword;
                 resp.SeoDescription = article.SeoDescription;
                 resp.String1 = article.String1;
                 resp.String2 = article.String2;
                 resp.String3 = article.String3;
                 resp.String4 = article.String4;
                 resp.Int1 = article.Int1;
                 resp.Int2 = article.Int2;
                 resp.Int3 = article.Int3;
                 resp.Int4 = article.Int4;
                 resp.Decimal1 = article.Decimal1;
                 resp.Decimal2 = article.Decimal2;
                 resp.Decimal3 = article.Decimal3;
                 resp.Decimal4 = article.Decimal4;
                 resp.Datetime1 = article.Datetime1;
                 resp.Datetime2 = article.Datetime2;
                 resp.Datetime3 = article.Datetime3;
                 resp.Datetime4 = article.Datetime4;
                 resp.Bool1 = article.Bool1;
                 resp.Bool2 = article.Bool2;
                 resp.Bool3 = article.Bool3;
                 resp.Bool4 = article.Bool4;
                 resp.Text1 = article.Text1;
                 resp.Text2 = article.Text2;
                 resp.Text3 = article.Text3;
                 resp.Text4 = article.Text4;
                 return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateArticleResponse CreateArticle(CreateArticleRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var article = new Article
                {
                    ArticleTypeId = request.ArticleTypeId,
                    CategoryId = request.CategoryId,
                    Title = request.Title,
                    SubTitle = request.SubTitle,
                    TitleColor = request.TitleColor,
                    TitleBold = request.TitleBold,
                    Summary = request.Summary,
                    Content = request.Content,
                    Tags = request.Tags,
                    ThumbImage = request.ThumbImage,
                    Video = request.Video,
                    Source = request.Source,
                    Author = request.Author,
                    Hits = request.Hits,
                    Addtime = request.Addtime,
                    OrderIndex = request.OrderIndex,
                    IsTop = request.IsTop,
                    IsRecommend = request.IsRecommend,
                    SeoTitle = request.SeoTitle,
                    SeoKeyword = request.SeoKeyword,
                    SeoDescription = request.SeoDescription,
                    String1 = request.String1,
                    String2 = request.String2,
                    String3 = request.String3,
                    String4 = request.String4,
                    Int1 = request.Int1,
                    Int2 = request.Int2,
                    Int3 = request.Int3,
                    Int4 = request.Int4,
                    Decimal1 = request.Decimal1,
                    Decimal2 = request.Decimal2,
                    Decimal3 = request.Decimal3,
                    Decimal4 = request.Decimal4,
                    Datetime1 = request.Datetime1,
                    Datetime2 = request.Datetime2,
                    Datetime3 = request.Datetime3,
                    Datetime4 = request.Datetime4,
                    Bool1 = request.Bool1,
                    Bool2 = request.Bool2,
                    Bool3 = request.Bool3,
                    Bool4 = request.Bool4,
                    Text1 = request.Text1,
                    Text2 = request.Text2,
                    Text3 = request.Text3,
                    Text4 = request.Text4
                };
                article.Id = client.Insertable(article).ExecuteReturnBigIdentity();
                
                _eventPublisher.EntityCreated(article);
                
                return new CreateArticleResponse();
            }
        }
        
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpdateArticleResponse UpdateArticle(UpdateArticleRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var article = client.Queryable<Article>().InSingle(request.Id);
                if (article == null)
                {
                    throw new AlertException("找不到该条信息");
                }
                article.CategoryId = request.CategoryId;
                article.Title = request.Title;
                article.SubTitle = request.SubTitle;
                article.TitleColor = request.TitleColor;
                article.TitleBold = request.TitleBold;
                article.Summary = request.Summary;
                article.Content = request.Content;
                article.Tags = request.Tags;
                article.ThumbImage = request.ThumbImage;
                article.Video = request.Video;
                article.Source = request.Source;
                article.Author = request.Author;
                article.Hits = request.Hits;
                article.Addtime = request.Addtime;
                article.OrderIndex = request.OrderIndex;
                article.IsTop = request.IsTop;
                article.IsRecommend = request.IsRecommend;
                article.SeoTitle = request.SeoTitle;
                article.SeoKeyword = request.SeoKeyword;
                article.SeoDescription = request.SeoDescription;
                article.String1 = request.String1;
                article.String2 = request.String2;
                article.String3 = request.String3;
                article.String4 = request.String4;
                article.Int1 = request.Int1;
                article.Int2 = request.Int2;
                article.Int3 = request.Int3;
                article.Int4 = request.Int4;
                article.Decimal1 = request.Decimal1;
                article.Decimal2 = request.Decimal2;
                article.Decimal3 = request.Decimal3;
                article.Decimal4 = request.Decimal4;
                article.Datetime1 = request.Datetime1;
                article.Datetime2 = request.Datetime2;
                article.Datetime3 = request.Datetime3;
                article.Datetime4 = request.Datetime4;
                article.Bool1 = request.Bool1;
                article.Bool2 = request.Bool2;
                article.Bool3 = request.Bool3;
                article.Bool4 = request.Bool4;
                article.Text1 = request.Text1;
                article.Text2 = request.Text2;
                article.Text3 = request.Text3;
                article.Text4 = request.Text4;
                client.Updateable( article).ExecuteCommand();    
                
                _eventPublisher.EntityUpdated(article);
                
                return new UpdateArticleResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteArticleResponse DeleteArticle(DeleteArticleRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var article = client.Queryable<Article>().InSingle(request.Id);
                if (article == null)
                {
                    throw new AlertException("找不到该条信息");
                }

                client.Deleteable<Article>(request.Id).ExecuteCommand();
                
                _eventPublisher.EntityDeleted(article);
                
                return new DeleteArticleResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BulkDeleteArticlesResponse BulkDeleteArticles(BulkDeleteArticlesRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                client.Deleteable<Article>().In(request.Ids).ExecuteCommand();
                
                request.Ids.ToList().ForEach(id =>
                {
                    //_cacheManager.RemoveOnEntityDeleted<Article>(id);
                });
                _eventPublisher.EntitiesDeleted<Article>(request.Ids);
                
                return new BulkDeleteArticlesResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedQueryArticles> PagedQueryArticles(PagedQueryArticleDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
 
            ArticleModel model = null;
            using (var client = DbFactory.CreateClient())
            {
                model = client.Queryable<ArticleModel>().InSingle(dto.ArticleModelId);
            }

            if (model == null)
                return Error("找不到文章模型");

            //设置字段
            var articleProperties = GenericCache<ArticleTypeInfo>
                .GetOrSet(() => new ArticleTypeInfo { PropertyInfos = typeof(Article).GetProperties() })
                .PropertyInfos;
            var configs = JsonConvert.DeserializeObject<List<ArticleConfiguration>>(model.Configuration);
            var selectFields = configs.Where(it => it.IsEnable && it.IsShowedInList)
                .Select(it =>
                {
                    var sugarColumn = articleProperties
                        .FirstOrDefault(x => x.Name.Equals(it.FiledName, StringComparison.OrdinalIgnoreCase))
                        ?.GetCustomAttribute<SugarColumn>();
                    return sugarColumn == null ? it.FiledName : sugarColumn.ColumnName;
                }).ToArray().Join(",");
            selectFields += selectFields.IsNullOrEmpty() ? "id,category_id" : ",id,category_id";
            
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;

                var query = client.Queryable<Article>()
                    .Where(it => it.ArticleTypeId == dto.ArticleModelId);

                if (dto.CategoryId > 0)
                {
                    var queryChildrenIdsByParentIdResult = _categoryService.QueryChildrenIdsByParentId(dto.CategoryId, true);
                    if (queryChildrenIdsByParentIdResult.Code==0 
                        && queryChildrenIdsByParentIdResult.Data!=null
                        && queryChildrenIdsByParentIdResult.Data.Any())
                    {
                        query = query.Where(it => queryChildrenIdsByParentIdResult.Data.Contains(it.CategoryId.Value));
                    }
                }

                query = query.Select(selectFields);

                var list = query
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize, ref total);
                
                return new PagedQueryArticles()
                {
                    List = list.Select(it => new QueryArticleItem
                    {
                        Id = it.Id,
                        CategoryId = it.CategoryId,
                        Title = it.Title,
                        SubTitle = it.SubTitle,
                        TitleColor = it.TitleColor,
                        TitleBold = it.TitleBold,
                        Summary = it.Summary,
                        Content = it.Content,
                        Tags = it.Tags,
                        ThumbImage = it.ThumbImage.GetFullPath(),
                        Video = it.Video,
                        Source = it.Source,
                        Author = it.Author,
                        Hits = it.Hits,
                        Addtime = it.Addtime,
                        OrderIndex = it.OrderIndex,
                        IsTop = it.IsTop,
                        IsRecommend = it.IsRecommend,
                        SeoTitle = it.SeoTitle,
                        SeoKeyword = it.SeoKeyword,
                        SeoDescription = it.SeoDescription,
                        String1 = it.String1,
                        String2 = it.String2,
                        String3 = it.String3,
                        String4 = it.String4,
                        Int1 = it.Int1,
                        Int2 = it.Int2,
                        Int3 = it.Int3,
                        Int4 = it.Int4,
                        Decimal1 = it.Decimal1,
                        Decimal2 = it.Decimal2,
                        Decimal3 = it.Decimal3,
                        Decimal4 = it.Decimal4,
                        Datetime1 = it.Datetime1,
                        Datetime2 = it.Datetime2,
                        Datetime3 = it.Datetime3,
                        Datetime4 = it.Datetime4,
                        Bool1 = it.Bool1,
                        Bool2 = it.Bool2,
                        Bool3 = it.Bool3,
                        Bool4 = it.Bool4,
                        Text1 = it.Text1,
                        Text2 = it.Text2,
                        Text3 = it.Text3,
                        Text4 = it.Text4
                    }).ToList(),
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        #endregion
        
        #region 文章模型

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetArticleModelDetailResponse GetArticleModelDetail(GetArticleModelDetailRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                 var articleModel = client.Queryable<ArticleModel>().InSingle(request.Id);
                 if(articleModel == null)
                 {
                    return null;
                 }
                 var resp = new GetArticleModelDetailResponse();
                 resp.Id = articleModel.Id;
                 resp.Name = articleModel.Name;
                 
                 resp.Configuration = articleModel.Configuration;
                 resp.CategoryId = articleModel.CategoryId;
                 return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateArticleModelResponse CreateArticleModel(CreateArticleModelRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var articleModel = new ArticleModel
                {
                    Name = request.Name, 
                    Configuration = request.Configuration, 
                    CategoryId = request.CategoryId,
                };

                articleModel.Id = client.Insertable(articleModel).ExecuteReturnBigIdentity();
                
                //_cacheManager.RemoveOnEntityCreated<ArticleModel>();
                _eventPublisher.EntityCreated(articleModel);
                
                return new CreateArticleModelResponse();
            }
        }
        
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpdateArticleModelResponse UpdateArticleModel(UpdateArticleModelRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var articleModel = client.Queryable<ArticleModel>().InSingle(request.Id);
                if (articleModel == null)
                {
                    throw new AlertException("找不到该条信息");
                }

                articleModel.Name = request.Name;
                articleModel.Configuration = request.Configuration;
                articleModel.CategoryId = request.CategoryId;
                
                client.Updateable( articleModel).ExecuteCommand();    
                
                //_cacheManager.RemoveOnEntityUpdated<ArticleModel>(request.Id);
                _eventPublisher.EntityUpdated(articleModel);
                
                return new UpdateArticleModelResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeleteArticleModelResponse DeleteArticleModel(DeleteArticleModelRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var articleModel = client.Queryable<ArticleModel>().InSingle(request.Id);
                if (articleModel == null)
                {
                    throw new AlertException("找不到该条信息");
                }

                client.Deleteable<ArticleModel>(request.Id).ExecuteCommand();
                
                //_cacheManager.RemoveOnEntityDeleted<ArticleModel>(request.Id);
                _eventPublisher.EntityDeleted(articleModel);
                
                return new DeleteArticleModelResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryArticleModelResponse PagedQueryArticleModels(PagedQueryArticleModelRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<ArticleModel>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryArticleModelItem()
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        Configuration = t.Configuration, 
                        CategoryId = t.CategoryId, 
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedQueryArticleModelResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }
        #endregion
    }
}