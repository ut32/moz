using Moz.Bus.Dtos.ServicePerformances;
using Moz.Domain.Dtos.ServicePerformances;

namespace Moz.Bus.Services.ServicePerformances
{
    public interface IServicePerformanceService
    {
        CreateServicePerformanceResponse CreateServicePerformance(CreateServicePerformanceRequest request);
        PagedQueryServicePerformanceResponse PagedQueryServicePerformances(PagedQueryServicePerformanceRequest request);
    }
}