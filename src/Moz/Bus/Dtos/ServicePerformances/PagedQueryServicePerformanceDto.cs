using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Moz.Bus.Dtos;
using Moz.Bus.Services.Localization;
using Moz.Domain.Dtos;
using Moz.CMS.Dtos;
using Moz.CMS.Dtos;
using Moz.Validation;

namespace Moz.Domain.Dtos.ServicePerformances
{
    /// <summary>
    /// tab_service_performance
    /// </summary>
    [Validator(typeof(PagedQueryServicePerformanceRequestValidator))]
    public class PagedQueryServicePerformanceRequest
    {
        #region 属性
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Keyword{ get;set; }
        #endregion     
    }
    
    
    public class PagedQueryServicePerformanceResponse: PagedResult<QueryServicePerformanceItem>
    {
    
    }
    
    public class QueryServicePerformanceItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public int ElapsedMs { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public string HttpRequestId { get;set; } 
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime AddTime { get;set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddTimeString => AddTime.ToString("yyyy/MM/dd HH:mm:ss");
    }
    
    public class PagedQueryServicePerformanceRequestValidator : MozValidator<PagedQueryServicePerformanceRequest>
    {
        public PagedQueryServicePerformanceRequestValidator(ILocalizationService localizationService)
        {
            
        }
    }
    
}
