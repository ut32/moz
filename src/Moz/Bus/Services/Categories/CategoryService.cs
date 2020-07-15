using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
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

        public const string CACHE_CATEGORY_ALL_KEY = "CACHE_CATEGORY_ALL_KEY";

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
                using (var client = DbFactory.CreateClient())
                {
                    return client.Queryable<Category>().OrderBy("order_index ASC , id ASC").ToList();
                }
            });
        }
        

        /// <summary>
        /// 更新路径
        /// </summary>
        /// <param name="categoryId"></param>
        private void UpdatePathByCategoryId(long categoryId)
        {
            List<Category> categories; 
            using (var db = DbFactory.CreateClient())
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

            using (var db = DbFactory.CreateClient())
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<CategoryDetail> GetCategoryDetail(GetCategoryDetailDto dto)
        {
            Category category = null;
            using (var client = DbFactory.CreateClient())
            {
                 category = client.Queryable<Category>().InSingle(dto.Id);
            }
            if(category == null)
            {
                return null;
            }

            var res = new CategoryDetail
            {
                Id = category.Id,
                Name = category.Name,
                Alias = category.Alias,
                Description = category.Description,
                OrderIndex = category.OrderIndex,
                ParentId = category.ParentId,
                Path = category.Path
            };
            return res;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreateCategory(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Alias = dto.Alias,
                Description = dto.Description,
                ParentId = dto.ParentId
            };
            using (var client = DbFactory.CreateClient())
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateCategory(UpdateCategoryDto dto)
        {
            Category category = null;
            using (var client = DbFactory.CreateClient())
            {
                category = client.Queryable<Category>().InSingle(dto.Id);
                if (category == null)
                {
                    return Error("找不到该条信息");
                }
                category.Name = dto.Name; 
                category.Alias = dto.Alias;
                category.Description = dto.Description;
                category.ParentId = dto.ParentId;
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeleteCategory(DeleteCategoryDto dto)
        {
            Category category = null;
            using (var client = DbFactory.CreateClient())
            {
                category = client.Queryable<Category>().InSingle(dto.Id);
                if (category == null)
                {
                    return Error("找不到该条信息");
                }

                client.UseTran(tran =>
                {
                    tran.Ado.ExecuteCommand(@"DELETE FROM tab_category WHERE path LIKE @path", new { path = $"{category.Path}.%"});
                    tran.Deleteable<Category>(dto.Id).ExecuteCommand();
                });
            }
            _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
            _eventPublisher.EntityDeleted(category);
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetOrderIndex(SetOrderIndexDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                client.Updateable<Category>().SetColumns(it=>new Category
                {
                    OrderIndex = dto.OrderIndex
                }).Where(it=>it.Id==dto.Id).ExecuteCommand();
            }
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryCategoryItem>> PagedQueryCategories(PagedQueryCategoryDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 1000;
            using (var client = DbFactory.CreateClient())
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
                    .OrderBy("order_index ASC, id DESC")
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
        /// 查询所有子id
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="includeParentId"></param>
        /// <returns></returns>
        public PublicResult<List<long>> QueryChildrenIdsByParentId(long? parentId, bool includeParentId = false)
        {
            var ids = new List<long>();
            GetAllSubCategoryIds(parentId,ids);
            if (includeParentId && parentId!=null)
            {
                ids.Add(parentId.Value);
            }

            return ids;
        }
        
        private void GetAllSubCategoryIds(long? parentId,List<long> ids) 
        { 
            var list = GetCategoriesListCached(); 
            var subCategories = list.Where(t => t.ParentId == parentId).ToList();
            foreach (var subCategory in subCategories)
            {
                ids.Add(subCategory.Id);
                GetAllSubCategoryIds(subCategory.Id, ids);
            }
        }

        /// <summary>
        /// 根据别名获取路径
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public PublicResult<string> GetCategoryPathByAlias(string alias)
        {
            if (alias.IsNullOrEmpty()) return null;
            var list = GetCategoriesListCached();
            return list.FirstOrDefault(it => it.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase))
                ?.Path;
        }
        
        

        /// <summary>
        /// 根据别名获取
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public PublicResult<string> GetCategoryNameByAlias(string alias)
        {
            if (alias.IsNullOrEmpty()) return null;
            var list = GetCategoriesListCached();
            return list.FirstOrDefault(it => it.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase))
                ?.Name;
        }
        

        /// <summary>
        /// 查询所有子类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public PublicResult<List<CategoryTree>> QuerySubCategoriesByParentId(long? parentId) 
        {
            return GetAllSubCategories(parentId);
        }

        private List<CategoryTree> GetAllSubCategories(long? parentId)
        {
            var list = GetCategoriesListCached(); 
            var subCategories = list.Where(t => t.ParentId == parentId).ToList();
            var result = new List<CategoryTree>();
            foreach (var subCategory in subCategories)
            {
                result.Add(new CategoryTree()
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