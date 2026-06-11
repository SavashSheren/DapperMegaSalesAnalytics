namespace DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos
{
    public class ResultSalesTransactionDto
    {
        public int SalesTransactionId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerEmail { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string SalesChannel { get; set; }
        public DateTime OrderDate { get; set; }
        public int DeliveryDay { get; set; }
        public int CustomerAge { get; set; }
    }
}