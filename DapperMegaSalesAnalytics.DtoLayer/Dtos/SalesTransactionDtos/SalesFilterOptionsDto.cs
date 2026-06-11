namespace DapperMegaSalesAnalytics.DtoLayer.Dtos.SalesTransactionDtos
{
    public class SalesFilterOptionsDto
    {
        public List<string> Cities { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public List<string> Statuses { get; set; } = new();
        public List<string> PaymentMethods { get; set; } = new();
        public List<string> SalesChannels { get; set; } = new();
    }
}