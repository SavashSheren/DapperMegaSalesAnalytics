using Dapper;
using DapperMegaSalesAnalytics.DataAccessLayer.Abstract;
using DapperMegaSalesAnalytics.DataAccessLayer.Context;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.DataAccessLayer.Concrete
{
    public class DashboardDal : IDashboardDal
    {
        private readonly DapperContext _context;

        public DashboardDal(DapperContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var summaryQuery = @"
                SELECT
                    CAST(COUNT(*) AS BIGINT) AS TotalTransactions,
                    CAST(COUNT(DISTINCT CustomerEmail) AS BIGINT) AS TotalCustomers,

                    ISNULL(SUM(CASE WHEN OrderStatus = 'Completed' THEN TotalPrice ELSE 0 END), 0) AS TotalRevenue,
                    ISNULL(AVG(CASE WHEN OrderStatus = 'Completed' THEN TotalPrice END), 0) AS AverageOrderValue,

                    CAST(100.0 * SUM(CASE WHEN OrderStatus = 'Completed' THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS CompletedRate,
                    CAST(100.0 * SUM(CASE WHEN OrderStatus = 'Cancelled' THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS CancelledRate,
                    CAST(100.0 * SUM(CASE WHEN OrderStatus = 'Returned' THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS ReturnedRate,
                    CAST(100.0 * SUM(CASE WHEN SalesChannel = 'Website' THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS WebsiteRate,
                    CAST(100.0 * SUM(CASE WHEN PaymentMethod = 'Credit Card' THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS CreditCardRate
                FROM SalesTransactions
                WHERE IsDeleted = 0;
            ";

            var topCityQuery = @"
                SELECT TOP 1 City
                FROM SalesTransactions
                WHERE IsDeleted = 0 AND OrderStatus = 'Completed'
                GROUP BY City
                ORDER BY SUM(TotalPrice) DESC;
            ";

            var topCategoryQuery = @"
                SELECT TOP 1 ProductCategory
                FROM SalesTransactions
                WHERE IsDeleted = 0 AND OrderStatus = 'Completed'
                GROUP BY ProductCategory
                ORDER BY SUM(TotalPrice) DESC;
            ";

            var topChannelQuery = @"
                SELECT TOP 1 SalesChannel
                FROM SalesTransactions
                WHERE IsDeleted = 0 AND OrderStatus = 'Completed'
                GROUP BY SalesChannel
                ORDER BY SUM(TotalPrice) DESC;
            ";

            using var connection = _context.CreateConnection();

            var summary = await connection.QueryFirstOrDefaultAsync<DashboardSummaryDto>(summaryQuery)
                          ?? new DashboardSummaryDto();

            summary.TopCity = await connection.QueryFirstOrDefaultAsync<string>(topCityQuery) ?? "N/A";
            summary.TopCategory = await connection.QueryFirstOrDefaultAsync<string>(topCategoryQuery) ?? "N/A";
            summary.TopSalesChannel = await connection.QueryFirstOrDefaultAsync<string>(topChannelQuery) ?? "N/A";

            return summary;
        }

        public async Task<List<DashboardChartItemDto>> GetMonthlyRevenueAsync()
        {
            var query = @"
                SELECT TOP 12
                    CONVERT(CHAR(7), OrderDate, 120) AS Label,
                    ISNULL(SUM(TotalPrice), 0) AS Value,
                    COUNT(*) AS Count
                FROM SalesTransactions
                WHERE IsDeleted = 0 AND OrderStatus = 'Completed'
                GROUP BY CONVERT(CHAR(7), OrderDate, 120)
                ORDER BY Label DESC;
            ";

            using var connection = _context.CreateConnection();

            var values = (await connection.QueryAsync<DashboardChartItemDto>(query)).ToList();
            values.Reverse();

            return values;
        }

        public async Task<List<DashboardChartItemDto>> GetCategoryRevenueAsync()
        {
            var query = @"
                SELECT TOP 8
                    ProductCategory AS Label,
                    ISNULL(SUM(TotalPrice), 0) AS Value,
                    COUNT(*) AS Count
                FROM SalesTransactions
                WHERE IsDeleted = 0 AND OrderStatus = 'Completed'
                GROUP BY ProductCategory
                ORDER BY Value DESC;
            ";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<DashboardChartItemDto>(query)).ToList();
        }

        public async Task<List<DashboardChartItemDto>> GetOrderStatusDistributionAsync()
        {
            var query = @"
                SELECT
                    OrderStatus AS Label,
                    CAST(COUNT(*) AS DECIMAL(18,2)) AS Value,
                    COUNT(*) AS Count
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY OrderStatus
                ORDER BY Count DESC;
            ";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<DashboardChartItemDto>(query)).ToList();
        }

        public async Task<List<DashboardChartItemDto>> GetPaymentMethodDistributionAsync()
        {
            var query = @"
                SELECT
                    PaymentMethod AS Label,
                    CAST(COUNT(*) AS DECIMAL(18,2)) AS Value,
                    COUNT(*) AS Count
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY PaymentMethod
                ORDER BY Count DESC;
            ";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<DashboardChartItemDto>(query)).ToList();
        }

        public async Task<List<DashboardChartItemDto>> GetSalesChannelDistributionAsync()
        {
            var query = @"
                SELECT
                    SalesChannel AS Label,
                    CAST(COUNT(*) AS DECIMAL(18,2)) AS Value,
                    COUNT(*) AS Count
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY SalesChannel
                ORDER BY Count DESC;
            ";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<DashboardChartItemDto>(query)).ToList();
        }

        public async Task<List<DashboardChartItemDto>> GetTopCitiesAsync()
        {
            var query = @"
                SELECT TOP 10
                    City AS Label,
                    ISNULL(SUM(TotalPrice), 0) AS Value,
                    COUNT(*) AS Count
                FROM SalesTransactions
                WHERE IsDeleted = 0 AND OrderStatus = 'Completed'
                GROUP BY City
                ORDER BY Value DESC;
            ";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<DashboardChartItemDto>(query)).ToList();
        }

        public async Task<List<ResultSalesTransactionDto>> GetRecentHighValueTransactionsAsync()
        {
            var query = @"
                SELECT TOP 8
                    SalesTransactionId,
                    OrderNumber,
                    CustomerFullName,
                    CustomerEmail,
                    City,
                    Country,
                    ProductName,
                    ProductCategory,
                    Quantity,
                    UnitPrice,
                    TotalPrice,
                    OrderStatus,
                    PaymentMethod,
                    SalesChannel,
                    OrderDate,
                    DeliveryDay,
                    CustomerAge
                FROM SalesTransactions
                WHERE IsDeleted = 0
                ORDER BY TotalPrice DESC;
            ";

            using var connection = _context.CreateConnection();

            return (await connection.QueryAsync<ResultSalesTransactionDto>(query)).ToList();
        }
    }
}