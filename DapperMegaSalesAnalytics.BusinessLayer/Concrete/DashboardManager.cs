using DapperMegaSalesAnalytics.BusinessLayer.Abstract;
using DapperMegaSalesAnalytics.DataAccessLayer.Abstract;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.BusinessLayer.Concrete
{
    public class DashboardManager : IDashboardService
    {
        private readonly IDashboardDal _dashboardDal;

        public DashboardManager(IDashboardDal dashboardDal)
        {
            _dashboardDal = dashboardDal;
        }

        public async Task<DashboardSummaryDto> TGetDashboardSummaryAsync()
        {
            return await _dashboardDal.GetDashboardSummaryAsync();
        }

        public async Task<List<DashboardChartItemDto>> TGetMonthlyRevenueAsync()
        {
            return await _dashboardDal.GetMonthlyRevenueAsync();
        }

        public async Task<List<DashboardChartItemDto>> TGetCategoryRevenueAsync()
        {
            return await _dashboardDal.GetCategoryRevenueAsync();
        }

        public async Task<List<DashboardChartItemDto>> TGetOrderStatusDistributionAsync()
        {
            return await _dashboardDal.GetOrderStatusDistributionAsync();
        }

        public async Task<List<DashboardChartItemDto>> TGetPaymentMethodDistributionAsync()
        {
            return await _dashboardDal.GetPaymentMethodDistributionAsync();
        }

        public async Task<List<DashboardChartItemDto>> TGetSalesChannelDistributionAsync()
        {
            return await _dashboardDal.GetSalesChannelDistributionAsync();
        }

        public async Task<List<DashboardChartItemDto>> TGetTopCitiesAsync()
        {
            return await _dashboardDal.GetTopCitiesAsync();
        }

        public async Task<List<ResultSalesTransactionDto>> TGetRecentHighValueTransactionsAsync()
        {
            return await _dashboardDal.GetRecentHighValueTransactionsAsync();
        }
    }
}