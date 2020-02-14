using System;
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Auth.Attributes;
using Moz.Biz.Dtos.Articles;
using Moz.Biz.Services.Articles;
using Moz.Bus.Dtos.Articles;
using Moz.Exceptions;

namespace Moz.Admin.Layui.Controllers
{
    public class ArticleController : AdminAuthBaseController
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        
        //[AdminAuthorize(Permissions = "admin.article.index")]
        public IActionResult Index(long type)
        {
            var articleModel = _articleService.GetArticleModelDetail(new Domain.Dtos.Articles.ArticleModels.GetArticleModelDetailRequest()
            {
                Id = type
            });
            if (articleModel == null)
            {
                throw new Exception("找不到文章模型");
            }
            var model = new Moz.Administration.Models.Articles.IndexModel()
            {
                Type = type,
                ArticleModel = articleModel
            };
            return View("~/Administration/Views/Article/Index.cshtml",model);
        }
        
        //[AdminAuthorize(Permissions = "admin.article.index")]
        public IActionResult PagedList(PagedQueryArticleRequest request)
        {
            var list = _articleService.PagedQueryArticles(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        //[AdminAuthorize(Permissions = "admin.article.create")]
        public IActionResult Create(long type)
        {
            var articleModel = _articleService.GetArticleModelDetail(new Domain.Dtos.Articles.ArticleModels.GetArticleModelDetailRequest()
            {
                Id = type
            });
            if (articleModel == null)
            {
                throw new Exception("找不到文章模型");
            }
            var model = new Moz.Administration.Models.Articles.CreateModel()
            {
                Type = type,
                ArticleModel = articleModel
            };
            return View("~/Administration/Views/Article/Create.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.article.create")]
        public IActionResult Create(CreateArticleRequest request)
        {
            var resp = _articleService.CreateArticle(request);
            return RespJson(resp);
        }
        
        //[AdminAuthorize(Permissions = "admin.article.update")]
        public IActionResult Update(GetArticleDetailRequest request)
        {
            var article = _articleService.GetArticleDetail(request);
            if (article == null)
                throw new AlertException("信息不存在，可能被删除");
            
            
            var articleModel = _articleService.GetArticleModelDetail(new Domain.Dtos.Articles.ArticleModels.GetArticleModelDetailRequest()
            {
                Id = article.ArticleTypeId
            });
            if (articleModel == null)
                throw new AlertException("找不到文章模型");
            
            
            var model = new  Moz.Administration.Models.Articles.UpdateModel()
            {
                Article = article,
                ArticleModel = articleModel
            };
            
            return View("~/Administration/Views/Article/Update.cshtml",model);
        }
        

        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.article.update")]
        public IActionResult Update(UpdateArticleRequest request)
        {
            var resp = _articleService.UpdateArticle(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        //[AdminAuthorize(Permissions = "admin.article.delete")]
        public IActionResult Delete(DeleteArticleRequest request)
        {
            var resp = _articleService.DeleteArticle(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.article.setisactive")]
        public IActionResult SetIsActive()
        {
            return RespJson(null);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.article.setorderindex")]
        public IActionResult SetOrderIndex()
        {
            return RespJson(null);
        }
    }
}