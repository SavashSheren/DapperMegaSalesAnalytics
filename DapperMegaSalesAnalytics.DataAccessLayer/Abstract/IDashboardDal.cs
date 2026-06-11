using DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.DataAccessLayer.Abstract
{
    public interface IDashboardDal
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();

        Task<List<DashboardChartItemDto>> GetMonthlyRevenueAsync();
        Task<List<DashboardChartItemDto>> GetCategoryRevenueAsync();
        Task<List<DashboardChartItemDto>> GetOrderStatusDistributionAsync();
        Task<List<DashboardChartItemDto>> GetPaymentMethodDistributionAsync();
        Task<List<DashboardChartItemDto>> GetSalesChannelDistributionAsync();
        Task<List<DashboardChartItemDto>> GetTopCitiesAsync();

        Task<List<ResultSalesTransactionDto>> GetRecentHighValueTransactionsAsync();
    }
}