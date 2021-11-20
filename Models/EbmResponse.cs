namespace dotnet_cloud_run_hello_world.Models
{
    public class EbmResponse
    {
        public class Value
        {
            public string ReceiptDateTimeJson { get; set; }
            public string SdcDateTimeJson { get; set; }
            public int Id { get; set; }
            public object DbDateTime { get; set; }
            public object AuditDateTime { get; set; }
            public object Timestamp { get; set; }
            public string ReceiptDateTime { get; set; }
            public string TIN { get; set; }
            public string ClientsTIN { get; set; }
            public string MRC { get; set; }
            public int RunNumber { get; set; }
            public string ReceiptType { get; set; }
            public string TransactionType { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal TaxRate1 { get; set; }
            public decimal TaxableAmount1 { get; set; }
            public decimal TaxAmount1 { get; set; }
            public decimal TaxRate2 { get; set; }
            public decimal TaxableAmount2 { get; set; }
            public decimal TaxAmount2 { get; set; }
            public decimal TaxRate3 { get; set; }
            public decimal TaxableAmount3 { get; set; }
            public decimal TaxAmount3 { get; set; }
            public decimal TaxRate4 { get; set; }
            public decimal TaxableAmount4 { get; set; }
            public decimal TaxAmount4 { get; set; }
            public string SdcId { get; set; }
            public string SdcDateTime { get; set; }
            public string SdcReceiptType { get; set; }
            public string SdcTransactionType { get; set; }
            public int SdcReceiptTypeCounter { get; set; }
            public int SdcTotalReceiptCounter { get; set; }
            public string SdcInternalData { get; set; }
            public string SdcReceiptSignature { get; set; }
            public object Journal { get; set; }
        }

        public string label { get; set; }
        public Value value { get; set; }
    }
}