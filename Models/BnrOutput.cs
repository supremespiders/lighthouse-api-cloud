namespace dotnet_cloud_run_hello_world.Models
{
    public class BnrOutput
    {
        public string Currency { get; set; }
        public string Date { get; set; }
        public decimal BuyingValue { get; set; }
        public decimal AverageValue { get; set; }
        public decimal SellingValue { get; set; }
    }
}