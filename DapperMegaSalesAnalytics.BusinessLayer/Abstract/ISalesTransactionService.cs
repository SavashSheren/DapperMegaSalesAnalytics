using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.BusinessLayer.Abstract
{
    public interface ISalesTransactionService
    {
        Task<List<ResultSalesTransactionDto>> TGetPagedSalesTransactionsAsync(int page, int pageSize);
        Task<int> TGetTotalSalesTransactionCountAsync();

        Task<List<ResultSalesTransactionDto>> TGetFilteredSalesTransactionsAsync(FilterSalesTransactionDto filter);
        Task<int> TGetFilteredSalesTransactionCountAsync(FilterSalesTransactionDto filter);
        Task<SalesFilterOptionsDto> TGetSalesFilterOptionsAsync();

        Task<ResultSalesTransactionDto?> TGetSalesTransactionByIdAsync(int id);
        Task TUpdateSalesTransactionAsync(UpdateSalesTransactionDto updateSalesTransactionDto);
        Task TDeleteSalesTransactionAsync(int id);
    }
}