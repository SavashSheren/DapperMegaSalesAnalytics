using Dapper;
using DapperMegaSalesAnalytics.DataAccessLayer.Abstract;
using DapperMegaSalesAnalytics.DataAccessLayer.Context;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.DataAccessLayer.Concrete
{
    public class SalesTransactionDal : ISalesTransactionDal
    {
        private readonly DapperContext _context;

        public SalesTransactionDal(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<ResultSalesTransactionDto>> GetPagedSalesTransactionsAsync(int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;

            var query = @"
                SELECT 
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
                ORDER BY SalesTransactionId DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            ";

            using var connection = _context.CreateConnection();

            var values = await connection.QueryAsync<ResultSalesTransactionDto>(
                query,
                new
                {
                    Offset = offset,
                    PageSize = pageSize
                });

            return values.ToList();
        }

        public async Task<int> GetTotalSalesTransactionCountAsync()
        {
            var query = "SELECT COUNT(*) FROM SalesTransactions WHERE IsDeleted = 0;";

            using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<ResultSalesTransactionDto?> GetSalesTransactionByIdAsync(int id)
        {
            var query = @"
                SELECT 
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
                WHERE SalesTransactionId = @SalesTransactionId AND IsDeleted = 0;
            ";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<ResultSalesTransactionDto>(
                query,
                new
                {
                    SalesTransactionId = id
                });
        }

        public async Task UpdateSalesTransactionAsync(UpdateSalesTransactionDto updateSalesTransactionDto)
        {
            var query = @"
                UPDATE SalesTransactions
                SET 
                    CustomerFullName = @CustomerFullName,
                    CustomerEmail = @CustomerEmail,
                    City = @City,
                    ProductName = @ProductName,
                    ProductCategory = @ProductCategory,
                    Quantity = @Quantity,
                    UnitPrice = @UnitPrice,
                    TotalPrice = @TotalPrice,
                    OrderStatus = @OrderStatus,
                    PaymentMethod = @PaymentMethod,
                    SalesChannel = @SalesChannel,
                    DeliveryDay = @DeliveryDay,
                    CustomerAge = @CustomerAge
                WHERE SalesTransactionId = @SalesTransactionId;
            ";

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(query, updateSalesTransactionDto);
        }

        public async Task DeleteSalesTransactionAsync(int id)
        {
            var query = @"
                UPDATE SalesTransactions
                SET IsDeleted = 1
                WHERE SalesTransactionId = @SalesTransactionId;
            ";

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                query,
                new
                {
                    SalesTransactionId = id
                });
        }
    }
}