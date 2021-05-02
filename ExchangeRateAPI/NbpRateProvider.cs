using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ExchangeRateAPI.Interfaces;
using ExchangeRateAPI.Models;
using Newtonsoft.Json.Linq;

namespace ExchangeRateAPI
{
    public class NbpRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _client = new();
        private const string NbpBaseApiAddress = "http://api.nbp.pl/";
        private const string TableType = "A";

        public NbpRateProvider()
        {
            BaseCurrency = new Currency("PLN", "Złoty");
            _client.BaseAddress = new Uri(NbpBaseApiAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Currency BaseCurrency { get; }
        public decimal GetExchangeRate(Currency currency)
        {
            var json = GetResponseContentAsync(new Uri($"api/exchangerates/rates/{TableType}/{currency.Code}/?format=json")).Result;
            var amountPLNValue = JArray.Parse(json)[0]["rates"]["mid"].Value<decimal>();
            return amountPLNValue;
        }

        private async Task<string> GetResponseContentAsync(Uri uri)
        {
            var response = await _client.GetAsync(uri);
            if (!response.IsSuccessStatusCode) throw new HttpListenerException((int) response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
