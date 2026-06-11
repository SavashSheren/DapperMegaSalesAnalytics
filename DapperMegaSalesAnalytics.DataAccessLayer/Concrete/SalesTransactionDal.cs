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

        public async Task<List<ResultSalesTransactionDto>> GetFilteredSalesTransactionsAsync(FilterSalesTransactionDto filter)
        {
            var offset = (filter.Page - 1) * filter.PageSize;

            var filterBuilder = BuildFilterWhereClause(filter);

            var query = $@"
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
                {filterBuilder.WhereClause}
                ORDER BY SalesTransactionId DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            ";

            filterBuilder.Parameters.Add("Offset", offset);
            filterBuilder.Parameters.Add("PageSize", filter.PageSize);

            using var connection = _context.CreateConnection();

            var values = await connection.QueryAsync<ResultSalesTransactionDto>(
                query,
                filterBuilder.Parameters);

            return values.ToList();
        }

        public async Task<int> GetFilteredSalesTransactionCountAsync(FilterSalesTransactionDto filter)
        {
            var filterBuilder = BuildFilterWhereClause(filter);

            var query = $@"
                SELECT COUNT(*)
                FROM SalesTransactions
                {filterBuilder.WhereClause};
            ";

            using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                query,
                filterBuilder.Parameters);
        }

        public async Task<SalesFilterOptionsDto> GetSalesFilterOptionsAsync()
        {
            var query = @"
                SELECT City
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY City
                ORDER BY City;

                SELECT ProductCategory
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY ProductCategory
                ORDER BY ProductCategory;

                SELECT OrderStatus
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY OrderStatus
                ORDER BY OrderStatus;

                SELECT PaymentMethod
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY PaymentMethod
                ORDER BY PaymentMethod;

                SELECT SalesChannel
                FROM SalesTransactions
                WHERE IsDeleted = 0
                GROUP BY SalesChannel
                ORDER BY SalesChannel;
            ";

            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(query);

            var options = new SalesFilterOptionsDto
            {
                Cities = (await multi.ReadAsync<string>()).ToList(),
                Categories = (await multi.ReadAsync<string>()).ToList(),
                Statuses = (await multi.ReadAsync<string>()).ToList(),
                PaymentMethods = (await multi.ReadAsync<string>()).ToList(),
                SalesChannels = (await multi.ReadAsync<string>()).ToList()
            };

            return options;
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

        private static (string WhereClause, DynamicParameters Parameters) BuildFilterWhereClause(FilterSalesTransactionDto filter)
        {
            var whereConditions = new List<string>
            {
                "IsDeleted = 0"
            };

            var parameters = new DynamicParameters();

            if (filter.SearchId.HasValue)
            {
                whereConditions.Add("SalesTransactionId = @SearchId");
                parameters.Add("SearchId", filter.SearchId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                whereConditions.Add(@"
                    (
                        OrderNumber LIKE @SearchTerm OR
                        CustomerFullName LIKE @SearchTerm OR
                        CustomerEmail LIKE @SearchTerm OR
                        ProductName LIKE @SearchTerm
                    )
                ");

                parameters.Add("SearchTerm", $"%{filter.SearchTerm.Trim()}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                whereConditions.Add("City = @City");
                parameters.Add("City", filter.City);
            }

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                whereConditions.Add("ProductCategory = @Category");
                parameters.Add("Category", filter.Category);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                whereConditions.Add("OrderStatus = @Status");
                parameters.Add("Status", filter.Status);
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
            {
                whereConditions.Add("PaymentMethod = @PaymentMethod");
                parameters.Add("PaymentMethod", filter.PaymentMethod);
            }

            if (!string.IsNullOrWhiteSpace(filter.SalesChannel))
            {
                whereConditions.Add("SalesChannel = @SalesChannel");
                parameters.Add("SalesChannel", filter.SalesChannel);
            }

            if (filter.StartDate.HasValue)
            {
                whereConditions.Add("OrderDate >= @StartDate");
                parameters.Add("StartDate", filter.StartDate.Value.Date);
            }

            if (filter.EndDate.HasValue)
            {
                whereConditions.Add("OrderDate < @EndDate");
                parameters.Add("EndDate", filter.EndDate.Value.Date.AddDays(1));
            }

            if (filter.MinPrice.HasValue)
            {
                whereConditions.Add("TotalPrice >= @MinPrice");
                parameters.Add("MinPrice", filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                whereConditions.Add("TotalPrice <= @MaxPrice");
                parameters.Add("MaxPrice", filter.MaxPrice.Value);
            }

            var whereClause = "WHERE " + string.Join(" AND ", whereConditions);

            return (whereClause, parameters);
        }
    }
}