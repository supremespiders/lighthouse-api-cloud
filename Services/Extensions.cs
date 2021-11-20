using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using dotnet_cloud_run_hello_world.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace dotnet_cloud_run_hello_world.Services
{
    public static class Extensions
    {
        private static readonly Regex regex = new Regex("[^a-zA-Z0-9]");

        public static string RemoveNonAlphaNumeric(this string s)
        {
            return regex.Replace(s, "");
        }
        
        public static async Task<HtmlDocument> GetDoc(this HttpClient httpClient, string url, CancellationToken ct = new CancellationToken(), int maxAttempts = 1)
        {
            var doc = new HtmlDocument();
            var html = await httpClient.GetHtml(url, ct).ConfigureAwait(false);
            doc.LoadHtml(html);
            return doc;
        }

        public static decimal ParseDecimal(this string s, string what)
        {
            var b = decimal.TryParse(s, out var x);
            if (!b)
                throw new MyException($"Failed to parse {what}, found : {s}");
            return x;
        }

        public static async Task<string> GetHtml(this HttpClient httpClient, string url, CancellationToken ct = new CancellationToken(), int maxAttempts = 1)
        {
            var tries = 0;
            do
            {
                try
                {
                    var response = await httpClient.GetAsync(url, ct).ConfigureAwait(false);
                    var html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return html;
                }
                catch (WebException ex)
                {
                    var errorMessage = ex.Message;
                    try
                    {
                        errorMessage = await new StreamReader(ex.Response.GetResponseStream()).ReadToEndAsync().ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    tries++;
                    if (tries == maxAttempts)
                    {
                        throw new Exception(ex.Status + " " + ex.Message + " " + errorMessage);
                    }

                    await Task.Delay(2000, ct);
                }
            } while (true);
        }

        public static async Task<string> PostFormData(this HttpClient httpClient, string url, List<KeyValuePair<string, string>> formData, int maxAttempts = 1)
        {
            var tries = 0;
            do
            {
                try
                {
                    var req = new HttpRequestMessage() { Method = HttpMethod.Post, RequestUri = new Uri(url), Content = new FormUrlEncodedContent(formData) };
                    var response = await httpClient.SendAsync(req).ConfigureAwait(false);
                    string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return html;
                }
                catch (WebException ex)
                {
                    var errorMessage = ex.Message;
                    try
                    {
                        errorMessage = await new StreamReader(ex.Response.GetResponseStream()).ReadToEndAsync().ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    tries++;
                    if (tries == maxAttempts)
                    {
                        throw new Exception(ex.Status + " " + ex.Message + " " + errorMessage);
                    }

                    await Task.Delay(2000);
                }
            } while (true);
        }

        public static async Task<string> PostJson(this HttpClient httpClient, string url, string json, bool isPut=false, CancellationToken ct = new CancellationToken(), int maxAttempts = 1)
        {
            int tries = 0;
            do
            {
                try
                {
                    var req = new HttpRequestMessage(isPut ? HttpMethod.Put : HttpMethod.Post, url);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    req.Content = content;
                    var r = await httpClient.SendAsync(req, ct).ConfigureAwait(false);
                    var s = await r.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return (s);
                }
                catch (WebException ex)
                {
                    var errorMessage = "";
                    try
                    {
                        errorMessage = await new StreamReader(ex.Response.GetResponseStream()).ReadToEndAsync().ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    tries++;
                    if (tries == maxAttempts)
                    {
                        throw new Exception(ex.Status + " " + ex.Message + " " + errorMessage);
                    }

                    await Task.Delay(2000, ct);
                }
            } while (true);
        }

        public static async Task<T> GetJson<T>(this HttpClient httpClient, string url, CancellationToken ct = new CancellationToken(), int maxAttempts = 1) where T : class
        {
            var json = await httpClient.GetHtml(url, ct, maxAttempts).ConfigureAwait(false);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

      
        public static string BetweenStrings(this string text, string start, string end)
        {
            if (!text.Contains(start)) return null;
            var p1 = text.IndexOf(start, StringComparison.Ordinal) + start.Length;
            var p2 = text.IndexOf(end, p1, StringComparison.Ordinal);
            if (end == "") return (text.Substring(p1));
            else return text.Substring(p1, p2 - p1);
        }
    }
}