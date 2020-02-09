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
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult CreateCategory(ServRequest<CreateCategoryDto> request);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult UpdateCategory(ServRequest<UpdateCategoryDto> request);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult DeleteCategory(ServRequest<DeleteCategoryDto> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult SetOrderIndex(ServRequest<SetOrderIndexDto> request);
        
        /*
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult BulkDeleteCategories(ServRequest<BulkDeleteCategoriesDto> request);
        */
        
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<CategoryDetail> GetCategoryDetail(ServRequest<GetCategoryDetailDto> request);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<PagedList<QueryCategoryItem>> PagedQueryCategories(ServRequest<PagedQueryCategoryDto> request);

        /// <summary>
        /// 查询所有子分类
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<List<CategoryTree>> QuerySubCategoriesByParentId(ServRequest<long?> request);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServRequest<string> GetCategoryPathByAlias(ServRequest<string> request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<string> GetCategoryNameByAlias(ServResult<string> request);
    }
}