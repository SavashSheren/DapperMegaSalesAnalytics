using DapperMegaSalesAnalytics.BusinessLayer.Abstract;
using DapperMegaSalesAnalytics.DataAccessLayer.Abstract;
using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.BusinessLayer.Concrete
{
    public class SalesTransactionManager : ISalesTransactionService
    {
        private readonly ISalesTransactionDal _salesTransactionDal;

        public SalesTransactionManager(ISalesTransactionDal salesTransactionDal)
        {
            _salesTransactionDal = salesTransactionDal;
        }

        public async Task<List<ResultSalesTransactionDto>> TGetPagedSalesTransactionsAsync(int page, int pageSize)
        {
            return await _salesTransactionDal.GetPagedSalesTransactionsAsync(page, pageSize);
        }

        public async Task<int> TGetTotalSalesTransactionCountAsync()
        {
            return await _salesTransactionDal.GetTotalSalesTransactionCountAsync();
        }

        public async Task<ResultSalesTransactionDto?> TGetSalesTransactionByIdAsync(int id)
        {
            return await _salesTransactionDal.GetSalesTransactionByIdAsync(id);
        }

        public async Task TUpdateSalesTransactionAsync(UpdateSalesTransactionDto updateSalesTransactionDto)
        {
            await _salesTransactionDal.UpdateSalesTransactionAsync(updateSalesTransactionDto);
        }

        public async Task TDeleteSalesTransactionAsync(int id)
        {
            await _salesTransactionDal.DeleteSalesTransactionAsync(id);
        }
    }
}