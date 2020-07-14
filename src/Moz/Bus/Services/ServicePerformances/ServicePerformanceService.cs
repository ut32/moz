using System;
using AspectCore.DynamicProxy;
using Moz.Bus.Dtos.ServicePerformances;
using Moz.Bus.Models.Common;
using Moz.DataBase;
using Moz.Domain.Dtos.ServicePerformances;
using Moz.Events;
using Moz.Exceptions;

namespace Moz.Bus.Services.ServicePerformances
{
    [NonAspect]
    public partial class ServicePerformanceService : IServicePerformanceService
    {
        #region Constants

        #endregion

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        #endregion

        #region Ctor
        public ServicePerformanceService(IEventPublisher eventPublisher)
        {
            this._eventPublisher = eventPublisher;
        }
        #endregion

        #region Methods

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateServicePerformanceResponse CreateServicePerformance(CreateServicePerformanceRequest request)
        {
            using (var client = DbFactory.CreateClient())
            {
                var servicePerformance = new ServicePerformanceMonitor();
                servicePerformance.Name = request.Name;
                servicePerformance.ElapsedMs = request.ElapsedMs;
                servicePerformance.HttpRequestId = request.HttpRequestId;
                servicePerformance.AddTime = request.AddTime;
                servicePerformance.Id = client.Insertable(servicePerformance).ExecuteReturnBigIdentity();
                
                //_cacheManager.RemoveOnEntityCreated<ServicePerformance>();
                _eventPublisher.EntityCreated(servicePerformance);
                
                return new CreateServicePerformanceResponse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PagedQueryServicePerformanceResponse PagedQueryServicePerformances(PagedQueryServicePerformanceRequest request)
        {
            var page = request.Page ?? 1;
            var pageSize = request.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<ServicePerformanceMonitor>()
                    .WhereIF(!request.Keyword.IsNullOrEmpty(), t => t.Name.Contains(request.Keyword))
                    .Select(t=>new QueryServicePerformanceItem()
                    {
                        Id = t.Id, 
                        Name = t.Name, 
                        ElapsedMs = t.ElapsedMs, 
                        HttpRequestId = t.HttpRequestId, 
                        AddTime = t.AddTime, 
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize,ref total);
                return new PagedQueryServicePerformanceResponse()
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }
        #endregion
    }
}