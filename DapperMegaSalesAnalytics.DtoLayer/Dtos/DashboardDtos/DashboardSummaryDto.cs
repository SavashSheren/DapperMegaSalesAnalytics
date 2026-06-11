namespace DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos
{
    public class DashboardSummaryDto
    {
        public long TotalTransactions { get; set; }
        public long TotalCustomers { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }

        public decimal CompletedRate { get; set; }
        public decimal CancelledRate { get; set; }
        public decimal ReturnedRate { get; set; }
        public decimal WebsiteRate { get; set; }
        public decimal CreditCardRate { get; set; }

        public string TopCity { get; set; } = string.Empty;
        public string TopCategory { get; set; } = string.Empty;
        public string TopSalesChannel { get; set; } = string.Empty;
    }
}