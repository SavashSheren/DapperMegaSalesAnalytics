namespace DapperMegaSalesAnalytics.DtoLayer.Dtos.DashboardDtos
{
    public class DashboardChartItemDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int Count { get; set; }
    }
}