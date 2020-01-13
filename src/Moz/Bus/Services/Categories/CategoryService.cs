using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Biz.Dtos.Categories;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Categories;
using Moz.Bus.Models.Categories;
using Moz.DataBase;
using Moz.Events;

namespace Moz.Bus.Services.Categories
{
    public partial class CategoryService : BaseService,ICategoryService
    {
        #region Constants

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly string CACHE_CATEGORY_ALL_KEY = "CACHE_CATEGORY_ALL_KEY";
        
        #endregion

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Ctor
        public CategoryService(
            IEventPublisher eventPublisher,
            IDistributedCache distributedCache)
        {
            _eventPublisher = eventPublisher;
            _distributedCache = distributedCache;
        }
        #endregion

        #region Utils
        
        /// <summary>
        /// 获取缓存表
        /// </summary>
        /// <returns></returns>
        private List<Category> GetCategoriesListCached()
        {
            return _distributedCache.GetOrSet(CACHE_CATEGORY_ALL_KEY, () =>
            {
                using (var client = DbFactory.GetClient())
                {
                    return client.Queryable<Category>().OrderBy("order_index ASC , id ASC").ToList();
                }
            });
        }
        

        /// <summary>
        /// 更新路径
        /// </summary>
        /// <param name="categoryId"></param>
        private static void UpdatePathByCategoryId(long categoryId)
        {
            List<Category> categories; 
            using (var db = DbFactory.GetClient())
            {
                categories = db.Queryable<Category>().ToList();
            }

            if (!categories.Any())
            {
                return;
            }
            
            string GetPath(long currentId)
            {
                var curCategory = categories.FirstOrDefault(it => it.Id == currentId);
                var parentCategoryId = curCategory?.ParentId;
                if (parentCategoryId == null || 0 == parentCategoryId)
                {
                    return currentId.ToString();
                }
                return GetPath(parentCategoryId.Value)+"."+currentId;
            }

            var updateCategoriesList = new List<Category>(); 
            void UpdatePath(long currentId)
            {
                var curCategory = categories.FirstOrDefault(it => it.Id == currentId);
                if (curCategory == null)
                {
                    return;
                }
                
                curCategory.Path = GetPath(currentId);
                
                updateCategoriesList.Add(curCategory);

                var subCategories = categories.Where(it => it.ParentId == currentId).ToList();
                if (!subCategories.Any())
                {
                    return;
                }

                foreach (var subCategory in subCategories)
                {
                    UpdatePath(subCategory.Id);
                }
            }
            
            UpdatePath(categoryId);
            if (!updateCategoriesList.Any())
            {
                return;
            }

            using (var db = DbFactory.GetClient())
            {
                db.Updateable(updateCategoriesList)
                    .UpdateColumns(it => new { it.Path })
                    .ExecuteCommand();
            }
            
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<CategoryDetail> GetCategoryDetail(ServRequest<GetCategoryDetailDto> request)
        {
            Category category = null;
            using (var client = DbFactory.GetClient())
            {
                 category = client.Queryable<Category>().InSingle(request.Data.Id);
            }
            if(category == null)
            {
                return null;
            }
            var res = new CategoryDetail();
            res.Id = category.Id;
            res.Name = category.Name;
            res.Alias = category.Alias;
            res.Desciption = category.Description;
            res.OrderIndex = category.OrderIndex;
            res.ParentId = category.ParentId;
            res.Path = category.Path;
            return res;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult CreateCategory(ServRequest<CreateCategoryDto> request)
        {
            var category = new Category
            {
                Name = request.Data.Name,
                Alias = request.Data.Alias,
                Description = request.Data.Description,
                ParentId = request.Data.ParentId
            };
            using (var client = DbFactory.GetClient())
            {
                category.Id = client.Insertable(category).ExecuteReturnBigIdentity();
            }
            UpdatePathByCategoryId(category.Id);
            _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
            _eventPublisher.EntityCreated(category);
            return Ok();
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult UpdateCategory(ServRequest<UpdateCategoryDto> request)
        {
            Category category = null;
            using (var client = DbFactory.GetClient())
            {
                category = client.Queryable<Category>().InSingle(request.Data.Id);
                if (category == null)
                {
                    return Error("找不到该条信息");
                }
                category.Name = request.Data.Name;
                category.Alias = request.Data.Alias;
                category.Description = request.Data.Desciption;
                category.ParentId = request.Data.ParentId;
                client.Updateable(category).ExecuteCommand();
            }
            UpdatePathByCategoryId(category.Id);
            _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
            _eventPublisher.EntityUpdated(category);
            return Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult DeleteCategory(ServRequest<DeleteCategoryDto> request)
        {
            Category category = null;
            using (var client = DbFactory.GetClient())
            {
                category = client.Queryable<Category>().InSingle(request.Data.Id);
                if (category == null)
                {
                    return Error("找不到该条信息");
                }
                client.Deleteable<Category>(request.Data.Id).ExecuteCommand();
            }
            _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
            _eventPublisher.EntityDeleted(category);
            return Ok();
        }
        
        
        /*
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult BulkDeleteCategories(ServRequest<BulkDeleteCategoriesDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<Category>().In(request.Data.Ids).ExecuteCommand();
            }
            _eventPublisher.EntitiesDeleted<Category>(request.Data.Ids);
            return Ok();
        }
        */

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<PagedList<QueryCategoryItem>> PagedQueryCategories(ServRequest<PagedQueryCategoryDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<Category>()
                    //.WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryCategoryItem()
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        Alias = t.Alias, 
                        Desciption = t.Description, 
                        OrderIndex = t.OrderIndex, 
                        ParentId = t.ParentId, 
                        Path = t.Path, 
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedList<QueryCategoryItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }
        
        /// <summary>
        /// 查询所有子类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<List<SimpleCategory>> QuerySubCategoriesByParentId(ServRequest<long?> request)
        {
            return GetAllSubCategories(request.Data);
        }

        private List<SimpleCategory> GetAllSubCategories(long? parentId)
        {
            var list = GetCategoriesListCached(); 
            var subCategories = list.Where(t => t.ParentId == parentId).ToList();
            var result = new List<SimpleCategory>();
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var subCategory in subCategories)
            {
                result.Add(new SimpleCategory()
                {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    Alias = subCategory.Alias,
                    Children = GetAllSubCategories(subCategory.Id)
                });   
            }
            return result;
        }

        #endregion
    }
}