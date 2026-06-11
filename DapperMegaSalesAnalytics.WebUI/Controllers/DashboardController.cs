using DapperMegaSalesAnalytics.BusinessLayer.Abstract;
using DapperMegaSalesAnalytics.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperMegaSalesAnalytics.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                Summary = await _dashboardService.TGetDashboardSummaryAsync(),
                MonthlyRevenue = await _dashboardService.TGetMonthlyRevenueAsync(),
                CategoryRevenue = await _dashboardService.TGetCategoryRevenueAsync(),
                OrderStatusDistribution = await _dashboardService.TGetOrderStatusDistributionAsync(),
                PaymentMethodDistribution = await _dashboardService.TGetPaymentMethodDistributionAsync(),
                SalesChannelDistribution = await _dashboardService.TGetSalesChannelDistributionAsync(),
                TopCities = await _dashboardService.TGetTopCitiesAsync(),
                RecentHighValueTransactions = await _dashboardService.TGetRecentHighValueTransactionsAsync()
            };

            return View(model);
        }
    }
}