using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Administration.Models.AdminMenus;
using Moz.Biz.Dtos.Categories;
using Moz.Biz.Services.Categories;
using Moz.Exceptions;

namespace Moz.Administration.Controllers
{
    public class CategoryController : AdminAuthBaseController
    {
        private readonly ICategoryService _categoryService;
        ///private readonly IRoleService _roleService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            //this._roleService = roleService;
        }
        

        public IActionResult Index()
        {
            return View("~/Administration/Views/Category/Index.cshtml");
        }

        public IActionResult List(QueryRequest request)
        {
            var list = _categoryService.Query(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Data = list.List
            };
            return Json(result);
        }

        public IActionResult Add()
        {
            return View("~/Administration/Views/Category/Add.cshtml");
        }
        

        [HttpPost]
        public IActionResult SaveAdd(CreateRequest arg)
        {
            var resp = _categoryService.Create(arg);
            return RespJson(resp);
        }
        
        public IActionResult Update([FromQuery]long id)
        {
            var menu = _categoryService.GetDetailById(new GetDetailByIdRequest(){ Id = id});
            if (menu == null)
            {
                throw new MozException("参数不正确");
            }
            return View("~/Administration/Views/Category/Update.cshtml",menu);
        }
        

        [HttpPost]
        public IActionResult SaveUpdate(UpdateRequest arg)
        {
            var resp = _categoryService.Update(arg);
            return RespJson(resp); 
        }

        [HttpGet]
        public IActionResult AllSubMenus(long? parentId) 
        {
            var resp = _categoryService.QueryChildrenByParentId(new QueryChildrenByParentIdRequest
            {
                ParentId = parentId
            });
            return Json(resp.AllSubs);
        }

        [HttpPost]
        public IActionResult SetOrderIndex(SetOrderIndexRequest arg)
        {
            var resp = _categoryService.SetOrderIndex(arg);
            return RespJson(resp);
        }
        
        [HttpPost]
        public IActionResult Delete(DeleteRequest arg)
        {
            var resp = _categoryService.Delete(arg);
            return RespJson(resp);
        }
    }
    
}