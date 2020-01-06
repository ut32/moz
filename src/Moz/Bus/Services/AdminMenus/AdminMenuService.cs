using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Bus.Models.AdminMenus;
using Moz.DataBase;
using Moz.Domain.Services.AdminMenus;
using Moz.Events;
using Moz.Exceptions;
using SqlSugar;

namespace Moz.CMS.Services.AdminMenus
{
    
    public class AdminMenuService : IAdminMenuService
    {
        private readonly IEventPublisher _eventPublisher;

        public AdminMenuService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateAdminMenuResponse CreateAdminMenu(CreateAdminMenuRequest request)
        {
            var adminMenu = new AdminMenu
            {
                Name = request.Name,
                ParentId = request.ParentId,
                Link = request.Link,
                OrderIndex = request.OrderIndex,
                Icon = request.Icon
            };
            using (var client = DbFactory.GetClient())
            {
                adminMenu.Id = client.Insertable(adminMenu).ExecuteReturnBigIdentity();
            }
            _eventPublisher.EntityCreated(adminMenu);
            return new CreateAdminMenuResponse();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public UpdateAdminMenuResponse UpdateAdminMenu(UpdateAdminMenuRequest request)
        {
            AdminMenu adminMenu;
            using (var client = DbFactory.GetClient())
            {
                adminMenu = client.Queryable<AdminMenu>().InSingle(request.Id);
                if (adminMenu == null)
                {
                    throw new MozException("找不到该条信息");
                }

                if (adminMenu.IsSystem)
                {
                    throw new MozException("不能编辑内置菜单");
                }

                adminMenu.Name = request.Name;
                adminMenu.ParentId = request.ParentId;
                adminMenu.Link = request.Link;
                adminMenu.OrderIndex = request.OrderIndex;
                adminMenu.Icon = request.Icon;
                client.Updateable( adminMenu).ExecuteCommand();
            }
            _eventPublisher.EntityUpdated(adminMenu);
            return new UpdateAdminMenuResponse();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public DeleteAdminMenuResponse DeleteAdminMenu(DeleteAdminMenuRequest request)
        {
            AdminMenu adminMenu;
            using (var client = DbFactory.GetClient())
            {
                adminMenu = client.Queryable<AdminMenu>().InSingle(request.Id);
                if (adminMenu == null)
                {
                    throw new MozException("找不到该条信息");
                }
                if (adminMenu.IsSystem)
                {
                    throw new MozException("不能编辑内置菜单");
                }
                client.Deleteable<AdminMenu>(request.Id).ExecuteCommand();
            }
            _eventPublisher.EntityDeleted(adminMenu);
            return new DeleteAdminMenuResponse();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public SetAdminMenuOrderIndexResponse SetAdminMenuOrderIndex(SetAdminMenuOrderIndexRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var menu = client.Queryable<AdminMenu>().InSingle(request.Id);
                if (menu == null)
                {
                    throw new MozException("找不到该条信息");
                }

                menu.OrderIndex = int.Parse(request.OrderIndex);
                client.Updateable(menu).UpdateColumns(t => new { t.OrderIndex }).ExecuteCommand();
                return new SetAdminMenuOrderIndexResponse();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryChildrenByParentIdResponse QueryChildrenByParentId(QueryChildrenByParentIdRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<AdminMenu>().ToList();
                var menus = GetAllSubAdminMenus(list,request.ParentId);
                return new QueryChildrenByParentIdResponse()
                {
                    AllSubs = menus
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<SimpleAdminMenu> GetAllSubAdminMenus(List<AdminMenu> list, long? parentId)
        {
            var selectedMenus = list.Where(t => t.ParentId == parentId).ToList();
            var simpleAdminMenus = new List<SimpleAdminMenu>();
            foreach (var adminMenu in selectedMenus)
            {
                simpleAdminMenus.Add(new SimpleAdminMenu()
                {
                    Id = adminMenu.Id,
                    Name = adminMenu.Name,
                    Children = GetAllSubAdminMenus(list,adminMenu.Id)
                });   
            }
            return simpleAdminMenus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetAdminMenuDetailResponse GetAdminMenuDetail(GetAdminMenuDetailRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adminMenu = client.Queryable<AdminMenu>().InSingle(request.Id);
                if(adminMenu == null)
                {
                    return null;
                }

                var resp = new GetAdminMenuDetailResponse
                {
                    Id = adminMenu.Id,
                    Name = adminMenu.Name,
                    ParentId = adminMenu.ParentId,
                    Link = adminMenu.Link,
                    OrderIndex = adminMenu.OrderIndex,
                    Icon = adminMenu.Icon,
                    IsSystem = adminMenu.IsSystem
                };
                return resp;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryAdminMenuResponse PagedQueryAdminMenus(PagedQueryAdminMenuRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<AdminMenu>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryAdminMenuItem()
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        ParentId = t.ParentId, 
                        Link = t.Link, 
                        OrderIndex = t.OrderIndex, 
                        Icon = t.Icon,
                        IsSystem = t.IsSystem
                    })
                    .OrderBy("order_index ASC, id ASC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedQueryAdminMenuResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetMenusByRoleResponse GetMenusByRole(GetMenusByRoleRequest request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<RoleMenu, AdminMenu>((rm, m) => new object[]
                    {
                        JoinType.Left, rm.MenuId==m.Id
                    })
                    .Where((rm, m) => rm.RoleId == request.RoleId)
                    .Select((rm, m) => new AdminMenu()
                    {
                        Icon = m.Icon,
                        Id = m.Id,
                        IsSystem = m.IsSystem,
                        Link = m.Link,
                        Name = m.Name,
                        OrderIndex = m.OrderIndex,
                        ParentId = m.ParentId
                    })
                    .OrderBy("order_index ASC, id ASC")
                    .ToList();
                
                return new GetMenusByRoleResponse()
                {
                    Menus = list
                };
            }
        }
    }
}