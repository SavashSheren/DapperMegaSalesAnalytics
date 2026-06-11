using DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.BusinessLayer.Abstract
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> TGetDashboardSummaryAsync();

        Task<List<DashboardChartItemDto>> TGetMonthlyRevenueAsync();
        Task<List<DashboardChartItemDto>> TGetCategoryRevenueAsync();
        Task<List<DashboardChartItemDto>> TGetOrderStatusDistributionAsync();
        Task<List<DashboardChartItemDto>> TGetPaymentMethodDistributionAsync();
        Task<List<DashboardChartItemDto>> TGetSalesChannelDistributionAsync();
        Task<List<DashboardChartItemDto>> TGetTopCitiesAsync();

        Task<List<ResultSalesTransactionDto>> TGetRecentHighValueTransactionsAsync();
    }
}