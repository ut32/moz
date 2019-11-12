using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Biz.Dtos.Categories;
using Moz.Biz.Models.Categories;
using Moz.Biz.Services.Categories;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;

namespace Moz.CMS.Services.Categories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CategoryService : ICategoryService
    {

        public static readonly string CACHE_CATEGORY_ALL_KEY = "CACHE_CATEGORY_ALL";
        
        private readonly IDistributedCache _distributedCache;
        private readonly IEventPublisher _eventPublisher;

        public CategoryService(  
            IDistributedCache distributedCache,
            IEventPublisher eventPublisher)
        {
            _distributedCache = distributedCache;
            _eventPublisher = eventPublisher;
        }

        public CreateResponse Create(CreateRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                ParentId = request.ParentId,
                Alias = request.Alias,
                Desciption = request.Description,
                OrderIndex = 0
            };
            using (var client = DbFactory.GetClient())
            {
                category.Id = client.Insertable(category).ExecuteReturnBigIdentity();
            }
            _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
            _eventPublisher.EntityCreated(category);
            return new CreateResponse();
        }


        public UpdateResponse Update(UpdateRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var category = client.Queryable<Category>().InSingle(request.Id);
                if (category == null)
                {
                    throw new MozException("找不到该条信息");
                }

                category.ParentId = request.ParentId;
                category.Name = request.Name;
                category.Desciption = request.Desciption;
                category.Alias = request.Alias;
                
                client.Updateable(category).ExecuteCommand();   
                _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
                return new UpdateResponse();
            }
        }

        public SetOrderIndexResponse SetOrderIndex(SetOrderIndexRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var category = client.Queryable<Category>().InSingle(request.Id);
                if (category == null)
                {
                    throw new MozException("找不到该条信息");
                }

                category.OrderIndex = int.Parse(request.OrderIndex);
                client.Updateable(category).UpdateColumns(t => new { t.OrderIndex }).ExecuteCommand();
                _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
                return new SetOrderIndexResponse();
            }
        }

        public DeleteResponse Delete(DeleteRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                client.Deleteable<Category>(request.Id).ExecuteCommand();
            }
            //_eventPublisher.EntityDeleted();
            _distributedCache.Remove(CACHE_CATEGORY_ALL_KEY);
            return new DeleteResponse();
        }

        /// <summary>
        /// 获取缓存
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

        public List<long> GetChildrenIdsByParentId(long? parentId,bool includeSelfId = true)
        {
            // ReSharper disable once RedundantAssignment
            var categoriesList = GetCategoriesListCached();
            var ids = new List<long>();
            void Func(long? pid)
            {
                var subCategories = categoriesList.Where(t => t.ParentId == pid).ToList();
                foreach (var subCategory in subCategories)
                {
                    ids.Add(subCategory.Id);
                    Func(subCategory.Id);
                }
            }
            if(includeSelfId && parentId>0)
                ids.Add(parentId.Value);
            return ids;
        }
        
        public string GetParentNameByChildId(long childId, long rootId)
        {
            try
            {
                var categoriesList = GetCategoriesListCached(); 
                var names = new List<string>();   
                void Func(long cid)
                {
                    var category = categoriesList.FirstOrDefault(t => t.Id == cid);
                    if (category != null)
                    {
                        names.Insert(0,category.Name);
                        if (category.ParentId!=null && category.ParentId.Value!=0 && category.ParentId != rootId)
                        {
                            Func(category.ParentId.Value);
                        }
                    }
                }

                Func(childId);

                return names.Join(" > ");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public QueryChildrenByParentIdResponse QueryChildrenByParentId(QueryChildrenByParentIdRequest request)
        {
            var list = GetCategoriesListCached();
            var categories = GetAllSubCategories(list,request.ParentId);
            return new QueryChildrenByParentIdResponse()
            {
                AllSubs = categories
            };
        }

        private List<SimpleCategory> GetAllSubCategories(List<Category> list, long? parentId)
        {
            var subCategories = list.Where(t => t.ParentId == parentId).ToList();
            var result = new List<SimpleCategory>();
            foreach (var subCategory in subCategories)
            {
                result.Add(new SimpleCategory()
                {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    Children = GetAllSubCategories(list,subCategory.Id)
                });   
            }
            return result;
        }


        public GetDetailByIdResponse GetDetailById(GetDetailByIdRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var category = client.Queryable<Category>().InSingle(request.Id);
                if (category == null)
                {
                    return null;
                }

                return new GetDetailByIdResponse()
                {
                    Id = request.Id,
                    Name = category.Name,
                    ParentId = category.ParentId
                };
            }
        }
        
        public QueryResponse Query(QueryRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<Category>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .OrderBy(t => t.OrderIndex)
                    .ToList();
                return new QueryResponse()
                {
                    Total = list.Count,
                    List = list
                };
            }
        }
    }
}