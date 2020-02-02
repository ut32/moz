using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Admin.Layui.Models.Categories;
using Moz.Auth.Attributes;
using Moz.Bus.Services.Categories;

namespace Moz.Admin.Layui.Controllers
{
    //[AdminAuth(Permissions = "admin.category")]
    public class CategoryController : AdminAuthBaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [AdminAuth(Permissions = "admin.category.index")]
        public IActionResult Index()
        {
            var model = new IndexModel();
            return View("~/Administration/Views/Category/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.category.index")]
        public IActionResult PagedList(Moz.Bus.Dtos.Categories.PagedQueryCategoryDto dto)
        {
            var list = _categoryService.PagedQueryCategories(dto);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.Data.TotalCount,
                Data = list.Data.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.category.create")]
        public IActionResult Create()
        {
            var model = new  CreateModel();
            return View("~/Administration/Views/Category/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.category.create")]
        public IActionResult Create(Moz.Bus.Dtos.Categories.CreateCategoryDto dto)
        {
            var result = _categoryService.CreateCategory(dto);
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.category.update")]
        public IActionResult Update(Moz.Bus.Dtos.Categories.GetCategoryDetailDto dto)
        {
            var result = _categoryService.GetCategoryDetail(dto);
            if (result.Code > 0)
            {
                return Json(result);
            }
            var model = new UpdateModel()
            {
                Category = result.Data
            };
            return View("~/Administration/Views/Category/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.category.update")]
        public IActionResult Update(Moz.Bus.Dtos.Categories.UpdateCategoryDto dto)
        {
            var result = _categoryService.UpdateCategory(dto); 
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.category.delete")]
        public IActionResult Delete(Moz.Bus.Dtos.Categories.DeleteCategoryDto dto)
        {
            var result = _categoryService.DeleteCategory(dto);
            return Json(result);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.category.setOrderIndex")]
        public IActionResult SetOrderIndex(Moz.Bus.Dtos.Categories.SetOrderIndexDto dto)
        {
            var result = _categoryService.SetOrderIndex(dto);
            return Json(result);
        }
        
        [HttpGet]
        public IActionResult AllSubMenus(long? parentId)
        {
            var result = _categoryService.QuerySubCategoriesByParentId(parentId);
            return Json(result.Data);
        }
    }
}
