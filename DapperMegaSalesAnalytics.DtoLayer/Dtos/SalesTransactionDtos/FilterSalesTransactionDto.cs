namespace DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos
{
    public class FilterSalesTransactionDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public int? SearchId { get; set; }
        public string? SearchTerm { get; set; }

        public string? City { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? SalesChannel { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}