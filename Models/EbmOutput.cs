using System.Collections.Generic;

namespace dotnet_cloud_run_hello_world.Models
{
    public class EbmOutput
    {
        public string Tin { get; set; }
        public string ClientsTin { get; set; }
        public string Mrc{ get; set; }
        public string RunNumber{ get; set; }
        public string CisDateAndTime{ get; set; }
        public List<Tax> Tax{ get; set; }
        public string SdcId{ get; set; }
        public string SdcDateAndTime{ get; set; }
        public string ReceiptLabel{ get; set; }
        public string InternalData{ get; set; }
        public string ReceiptSignature{ get; set; }
    }

    public class Tax
    {
        public decimal TaxRate { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }
    }

}