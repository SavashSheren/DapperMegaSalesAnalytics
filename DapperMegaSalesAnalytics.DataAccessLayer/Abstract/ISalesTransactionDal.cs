using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.DataAccessLayer.Abstract
{
    public interface ISalesTransactionDal
    {
        Task<List<ResultSalesTransactionDto>> GetPagedSalesTransactionsAsync(int page, int pageSize);
        Task<int> GetTotalSalesTransactionCountAsync();

        Task<List<ResultSalesTransactionDto>> GetFilteredSalesTransactionsAsync(FilterSalesTransactionDto filter);
        Task<int> GetFilteredSalesTransactionCountAsync(FilterSalesTransactionDto filter);
        Task<SalesFilterOptionsDto> GetSalesFilterOptionsAsync();

        Task<ResultSalesTransactionDto?> GetSalesTransactionByIdAsync(int id);
        Task UpdateSalesTransactionAsync(UpdateSalesTransactionDto updateSalesTransactionDto);
        Task DeleteSalesTransactionAsync(int id);
    }
}