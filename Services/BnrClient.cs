using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnet_cloud_run_hello_world.Models;
using HtmlAgilityPack;

namespace dotnet_cloud_run_hello_world.Services
{
    public class BnrClient
    {
        private HttpClient _client;

        readonly HttpClientHandler _httpClientHandler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        public BnrClient()
        {
            _client = new HttpClient(_httpClientHandler);
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36");
        }

        public async Task<BnrOutput> Work(string currency)
        {
            var p = new Dictionary<string, string>()
            {
                { "tx_bnrcurrencymanager_master[__referrer][@extension]", "BnrCurrencyManager" },
                { "tx_bnrcurrencymanager_master[__referrer][@vendor]", "BNR" },
                { "tx_bnrcurrencymanager_master[__referrer][@controller]", "Currency" },
                { "tx_bnrcurrencymanager_master[__referrer][@action]", "list" },
                { "tx_bnrcurrencymanager_master[currency]", currency },
                { "tx_bnrcurrencymanager_master[value]", "" },
                { "tx_bnrcurrencymanager_master[recordLimit]", "1" }
            };
            var html=await _client.PostFormData("https://www.bnr.rw/currency/exchange-rate/?tx_bnrcurrencymanager_master%5Baction%5D=list&tx_bnrcurrencymanager_master%5Bcontroller%5D=Currency", p.ToList());
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode.SelectSingleNode("//p[text()='No records found.']") != null) throw new MyException("No record found");
                var tds = doc.DocumentNode.SelectNodes($"//td[text()='{currency}']/../td");
            if (tds == null) throw new MyException("Html changed");
            if (tds.Count != 6) throw new MyException($"couldn't find the exact html match {tds.Count}");
            var date = tds[2].InnerText;
            var buyingValue = tds[3].InnerText;
            var averageValue = tds[4].InnerText;
            var sellingValue = tds[5].InnerText;

            var o = new BnrOutput()
            {
                Currency = currency,
                Date = date,
                BuyingValue = buyingValue.ParseDecimal("BuyingValue"),
                AverageValue = averageValue.ParseDecimal("AverageValue"),
                SellingValue = sellingValue.ParseDecimal("SellingValue")
            };
            return o;

        }
    }
}