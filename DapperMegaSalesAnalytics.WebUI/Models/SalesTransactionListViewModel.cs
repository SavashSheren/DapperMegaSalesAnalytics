using DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos;

namespace DapperMegaSalesAnalytics.WebUI.Models
{
    public class SalesTransactionListViewModel
    {
        public List<ResultSalesTransactionDto> Transactions { get; set; } = new();

        public UpdateSalesTransactionDto? EditTransaction { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public int? SearchId { get; set; }
        public string? Message { get; set; }
    }
}