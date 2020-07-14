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

    /// <summary>
    /// 
    /// </summary>
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult CreateAdminMenu(CreateAdminMenuDto dto)
        {
            var adminMenu = new AdminMenu
            {
                Name = dto.Name,
                ParentId = dto.ParentId,
                Link = dto.Link,
                OrderIndex = dto.OrderIndex,
                Icon = dto.Icon
            };
            using (var client = DbFactory.CreateClient())
            {
                adminMenu.Id = client.Insertable(adminMenu).ExecuteReturnBigIdentity();
            }

            _eventPublisher.EntityCreated(adminMenu);
            
            return Ok();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateAdminMenu(UpdateAdminMenuDto dto)
        {
            AdminMenu adminMenu;
            using (var client = DbFactory.CreateClient())
            {
                adminMenu = client.Queryable<AdminMenu>().InSingle(dto.Id);
                if (adminMenu == null)
                {
                    return Error("找不到该条信息");
                }

                if (adminMenu.IsSystem)
                {
                    return Error("不能编辑内置菜单");
                }

                adminMenu.Name = dto.Name;
                adminMenu.ParentId = dto.ParentId;
                adminMenu.Link = dto.Link;
                adminMenu.Icon = dto.Icon;
                
                client.Updateable(adminMenu).ExecuteCommand();
            }
            _eventPublisher.EntityUpdated(adminMenu);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult DeleteAdminMenu(DeleteAdminMenuDto dto)
        {
            AdminMenu adminMenu;
            using (var client = DbFactory.CreateClient())
            {
                adminMenu = client.Queryable<AdminMenu>().InSingle(dto.Id);
                if (adminMenu == null)
                {
                    return Error("找不到该条信息");
                }

                if (adminMenu.IsSystem)
                {
                    return Error("不能删除内置菜单");
                }

                client.Deleteable<AdminMenu>(dto.Id).ExecuteCommand();
            }

            _eventPublisher.EntityDeleted(adminMenu);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult SetAdminMenuOrderIndex(SetAdminMenuOrderIndexDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var menu = client.Queryable<AdminMenu>().InSingle(dto.Id);
                if (menu == null)
                {
                    return Error("找不到该条信息");
                }
                menu.OrderIndex = int.Parse(dto.OrderIndex);
                client.Updateable(menu).UpdateColumns(t => new {t.OrderIndex}).ExecuteCommand();
                return Ok();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<QueryChildrenByParentIdApo> QueryChildrenByParentId(QueryChildrenByParentIdDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var list = client.Queryable<AdminMenu>().ToList();
                var menus = GetAllSubAdminMenus(list, dto.ParentId);
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetAdminMenuDetailInfo> GetAdminMenuDetail(GetAdminMenuDetailDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var adminMenu = client.Queryable<AdminMenu>().InSingle(dto.Id);
                if (adminMenu == null)
                {
                    return Error("找不到信息");
                }

                var resp = new GetAdminMenuDetailInfo
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
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryAdminMenuItem>> PagedQueryAdminMenus(PagedQueryAdminMenusDto dto)
        {
            var page = 1;
            var pageSize = 1000;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<AdminMenu>()
                    .WhereIF(!dto.Keyword.IsNullOrEmpty(), t => t.Name.Contains(dto.Keyword))
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
    }
}