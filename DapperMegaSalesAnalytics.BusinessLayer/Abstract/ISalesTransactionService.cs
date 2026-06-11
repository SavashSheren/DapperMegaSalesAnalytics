using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.BusinessLayer.Abstract
{
    public interface ISalesTransactionService
    {
        Task<List<ResultSalesTransactionDto>> TGetPagedSalesTransactionsAsync(int page, int pageSize);
        Task<int> TGetTotalSalesTransactionCountAsync();
        Task<ResultSalesTransactionDto> TGetSalesTransactionByIdAsync(int id);
        Task TUpdateSalesTransactionAsync(UpdateSalesTransactionDto updateSalesTransactionDto);
        Task TDeleteSalesTransactionAsync(int id);
    }
}