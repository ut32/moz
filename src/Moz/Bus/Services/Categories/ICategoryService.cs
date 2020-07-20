using System.Collections.Generic;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Categories;
using Moz.Bus.Models.Categories;

namespace Moz.Bus.Services.Categories
{
    public interface ICategoryService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult CreateCategory(CreateCategoryDto dto);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateCategory(UpdateCategoryDto dto);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult DeleteCategory(DeleteCategoryDto dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult SetOrderIndex(SetOrderIndexDto dto);
        
        /*
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PublicResult BulkDeleteCategories(BulkDeleteCategoriesDto dto);
        */
        
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<CategoryDetail> GetCategoryDetail(GetCategoryDetailDto dto);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryCategoryItem>> PagedQueryCategories(PagedQueryCategoryDto dto);

        /// <summary>
        /// 查询所有子分类
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        PublicResult<List<CategoryTree>> QuerySubCategoriesByParentId(long? parentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="includeParentId"></param>
        /// <returns></returns>
        PublicResult<List<long>> QueryChildrenIdsByParentId(long? parentId, bool includeParentId = false);
        
        /// <summary> 
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        PublicResult<string> GetCategoryPathByAlias(string alias); 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        PublicResult<string> GetCategoryNameByAlias(string alias);
    }
}