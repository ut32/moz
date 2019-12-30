using System;﻿
using Microsoft.AspNetCore.Mvc;
using Moz.Admin.Layui.Common;
using Moz.Exceptions;
using Moz.Auth;
using Moz.Auth.Attributes;
using Moz.Biz.Dtos.Articles.ArticleModels;
using Moz.Biz.Services.Articles;

namespace Moz.Administration.Controllers
{
    [AdminAuth(Permissions = "admin.article.model")]
    public class ArticleModelController : AdminAuthBaseController
    {
        private readonly IArticleService _articleModelService;
        public ArticleModelController(IArticleService articleModelService)
        {
            this._articleModelService = articleModelService;
        }

        [AdminAuth(Permissions = "admin.article.model.index")]
        public IActionResult Index()
        {
            var model = new Moz.Administration.Models.ArticleModels.IndexModel();
            return View("~/Administration/Views/ArticleModel/Index.cshtml",model);
        }
        
        [AdminAuth(Permissions = "admin.article.model.index")]
        public IActionResult PagedList(Moz.Domain.Dtos.Articles.ArticleModels.PagedQueryArticleModelRequest request)
        {
            var list = _articleModelService.PagedQueryArticleModels(request);
            var result = new
            {
                Code = 0,
                Message = "",
                Total = list.TotalCount,
                Data = list.List
            };
            return Json(result);
        }
        
        [AdminAuth(Permissions = "admin.article.model.create")]
        public IActionResult Create()
        {
            var model = new  Moz.Administration.Models.ArticleModels.CreateModel();
            return View("~/Administration/Views/ArticleModel/Create.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.article.model.create")]
        public IActionResult Create(CreateArticleModelRequest request)
        {
            var resp = _articleModelService.CreateArticleModel(request);
            return RespJson(resp);
        }
        
        [AdminAuth(Permissions = "admin.article.model.update")]
        public IActionResult Update(Moz.Domain.Dtos.Articles.ArticleModels.GetArticleModelDetailRequest request)
        {
            var articleModel = _articleModelService.GetArticleModelDetail(request);
            if (articleModel == null)
            {
                throw new MozException("信息不存在，可能被删除");
            }
            var model = new  Moz.Administration.Models.ArticleModels.UpdateModel()
            {
                ArticleModel = articleModel
            };
            return View("~/Administration/Views/ArticleModel/Update.cshtml",model);
        }
        

        [HttpPost]
        [AdminAuth(Permissions = "admin.article.model.update")]
        public IActionResult Update(UpdateArticleModelRequest request)
        {
            var resp = _articleModelService.UpdateArticleModel(request); 
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.article.model.delete")]
        public IActionResult Delete(Moz.Domain.Dtos.Articles.ArticleModels.DeleteArticleModelRequest request)
        {
            var resp = _articleModelService.DeleteArticleModel(request);
            return RespJson(resp);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.article.model.setisactive")]
        public IActionResult SetIsActive()
        {
            return RespJson(null);
        }
        
        [HttpPost]
        [AdminAuth(Permissions = "admin.article.model.setorderindex")]
        public IActionResult SetOrderIndex()
        {
            return RespJson(null);
        }
    }
}
