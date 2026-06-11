using DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.WebUI.Models
{
    public class DashboardViewModel
    {
        public DashboardSummaryDto Summary { get; set; } = new();

        public List<DashboardChartItemDto> MonthlyRevenue { get; set; } = new();
        public List<DashboardChartItemDto> CategoryRevenue { get; set; } = new();
        public List<DashboardChartItemDto> OrderStatusDistribution { get; set; } = new();
        public List<DashboardChartItemDto> PaymentMethodDistribution { get; set; } = new();
        public List<DashboardChartItemDto> SalesChannelDistribution { get; set; } = new();
        public List<DashboardChartItemDto> TopCities { get; set; } = new();

        public List<ResultSalesTransactionDto> RecentHighValueTransactions { get; set; } = new();
    }
}