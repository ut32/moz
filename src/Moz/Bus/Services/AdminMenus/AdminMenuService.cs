using System;
using System.Collections.Generic;
using System.Linq;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.AdminMenus;
using Moz.Bus.Models.AdminMenus;
using Moz.DataBase;
using Moz.Events;
using Moz.Exceptions;
using SqlSugar;

namespace Moz.Bus.Services.AdminMenus
{

    public class AdminMenuService : BaseService,IAdminMenuService
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
        public ServResult CreateAdminMenu(ServRequest<CreateAdminMenuDto> request)
        {
            var adminMenu = new AdminMenu
            {
                Name = request.Data.Name,
                ParentId = request.Data.ParentId,
                Link = request.Data.Link,
                OrderIndex = request.Data.OrderIndex,
                Icon = request.Data.Icon
            };
            using (var client = DbFactory.GetClient())
            {
                adminMenu.Id = client.Insertable(adminMenu).ExecuteReturnBigIdentity();
            }

            _eventPublisher.EntityCreated(adminMenu);
            
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ServResult UpdateAdminMenu(ServRequest<UpdateAdminMenuDto> request)
        {
            AdminMenu adminMenu;
            using (var client = DbFactory.GetClient())
            {
                adminMenu = client.Queryable<AdminMenu>().InSingle(request.Data.Id);
                if (adminMenu == null)
                {
                    return Error("找不到该条信息");
                }

                if (adminMenu.IsSystem)
                {
                    return Error("不能编辑内置菜单");
                }

                adminMenu.Name = request.Data.Name;
                adminMenu.ParentId = request.Data.ParentId;
                adminMenu.Link = request.Data.Link;
                adminMenu.OrderIndex = request.Data.OrderIndex;
                adminMenu.Icon = request.Data.Icon;
                client.Updateable(adminMenu).ExecuteCommand();
            }

            _eventPublisher.EntityUpdated(adminMenu);
            
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ServResult DeleteAdminMenu(ServRequest<DeleteAdminMenuDto> request)
        {
            AdminMenu adminMenu;
            using (var client = DbFactory.GetClient())
            {
                adminMenu = client.Queryable<AdminMenu>().InSingle(request.Data.Id);
                if (adminMenu == null)
                {
                    return Error("找不到该条信息");
                }

                if (adminMenu.IsSystem)
                {
                    return Error("不能编辑内置菜单");
                }

                client.Deleteable<AdminMenu>(request.Data.Id).ExecuteCommand();
            }

            _eventPublisher.EntityDeleted(adminMenu);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="MozException"></exception>
        public ServResult SetAdminMenuOrderIndex(ServRequest<SetAdminMenuOrderIndexDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var menu = client.Queryable<AdminMenu>().InSingle(request.Data.Id);
                if (menu == null)
                {
                    return Error("找不到该条信息");
                }

                menu.OrderIndex = int.Parse(request.Data.OrderIndex);
                client.Updateable(menu).UpdateColumns(t => new {t.OrderIndex}).ExecuteCommand();
                return Ok();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<QueryChildrenByParentIdApo> QueryChildrenByParentId(ServRequest<QueryChildrenByParentIdDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var list = client.Queryable<AdminMenu>().ToList();
                var menus = GetAllSubAdminMenus(list, request.Data.ParentId);
                return new QueryChildrenByParentIdApo()
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
        private List<AdminMenuTree> GetAllSubAdminMenus(List<AdminMenu> list, long? parentId)
        {
            var selectedMenus = list.Where(t => t.ParentId == parentId).ToList();
            var simpleAdminMenus = new List<AdminMenuTree>();
            foreach (var adminMenu in selectedMenus)
            {
                simpleAdminMenus.Add(new AdminMenuTree()
                {
                    Id = adminMenu.Id,
                    Name = adminMenu.Name,
                    Children = GetAllSubAdminMenus(list, adminMenu.Id)
                });
            }
            return simpleAdminMenus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServResult<GetAdminMenuDetailApo> GetAdminMenuDetail(ServRequest<GetAdminMenuDetailDto> request)
        {
            using (var client = DbFactory.GetClient())
            {
                var adminMenu = client.Queryable<AdminMenu>().InSingle(request.Data.Id);
                if (adminMenu == null)
                {
                    return Error("找不到信息");
                }

                var resp = new GetAdminMenuDetailApo
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
        public ServResult<PagedList<QueryAdminMenuItem>> PagedQueryAdminMenus(ServRequest<PagedQueryAdminMenusDto> request)
        {
            var page = request.Data.Page ?? 1;
            var pageSize = request.Data.PageSize ?? 20;
            using (var client = DbFactory.GetClient())
            {
                var total = 0;
                var list = client.Queryable<AdminMenu>()
                    .WhereIF(!request.Data.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Data.Keyword))
                    .Select(t => new QueryAdminMenuItem()
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
                    .ToPageList(page, pageSize, ref total);
                return new PagedList<QueryAdminMenuItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetMenusByRoleApo GetMenusByRole(GetMenusByRoleDto request)
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
                
                return new GetMenusByRoleApo()
                {
                    Menus = list
                };
            }
        }
        */
    }
}