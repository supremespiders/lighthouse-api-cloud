using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnet_cloud_run_hello_world.Models;

namespace dotnet_cloud_run_hello_world.Services
{
    public class EbmClient
    {
        private HttpClient _client;

        readonly HttpClientHandler _httpClientHandler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        public EbmClient()
        {
            _client = new HttpClient(_httpClientHandler);
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36");
        }

        public async Task<EbmOutput> Work(string signature)
        {
            var sig = signature.RemoveNonAlphaNumeric().Trim().ToUpper();
            if (sig.Length != 16) throw new MyException($"Wrong input {signature}");
            var response = await _client.GetJson<List<EbmResponse>>($"https://ebm.rra.gov.rw/backoffice//Autocomplete/ReceiptSignature?term={sig}");
            if (response.Count == 0) throw new MyException($"Can't find signature {signature}");
            var v = response[0].value;
            var embOutput = new EbmOutput()
            {
                Tin = v.TIN.Trim(),
                ClientsTin = v.ClientsTIN.Trim(),
                Mrc = v.MRC.Trim(),
                InternalData = v.SdcInternalData.Trim(),
                ReceiptLabel = $"{v.SdcReceiptTypeCounter}/{v.SdcTotalReceiptCounter} {v.SdcReceiptType}{v.SdcTransactionType}",
                SdcId = v.SdcId.Trim(),
                RunNumber = v.RunNumber.ToString(),
                ReceiptSignature = signature,
                CisDateAndTime = v.ReceiptDateTimeJson,
                SdcDateAndTime = v.SdcDateTimeJson,
                Tax =new List<Tax>()
                {
                    new Tax()
                    {
                        TaxRate = v.TaxRate1,
                        TaxableAmount = v.TaxableAmount1,
                        TaxAmount = v.TaxAmount1
                    },
                    new Tax()
                    {
                        TaxRate = v.TaxRate2,
                        TaxableAmount = v.TaxableAmount2,
                        TaxAmount = v.TaxAmount2
                    },
                    new Tax()
                    {
                        TaxRate = v.TaxRate3,
                        TaxableAmount = v.TaxableAmount3,
                        TaxAmount = v.TaxAmount3
                    },
                    new Tax()
                    {
                        TaxRate = v.TaxRate4,
                        TaxableAmount = v.TaxableAmount4,
                        TaxAmount = v.TaxAmount4
                    }
                } 
                
                //"[{\"Tax Rate\":"+v.TaxRate2.ToString("#0.00")+",\"Taxable Amount\":"+v.TaxableAmount2+",\"Tax Amount\":"+v.TaxAmount2+"}]"
            };
            return embOutput;
        }
    }
}