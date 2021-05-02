using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using ExchangeRateAPI.Interfaces;
using ExchangeRateAPI.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateAPI
{
    public class NbpRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<NbpRateProvider> _logger;
        private readonly HttpClient _client = new();
        private readonly Uri _nbpBaseApiAddress = new("http://api.nbp.pl/");
        private const string TableType = "A";

        public NbpRateProvider(ILogger<NbpRateProvider> logger)
        {
            _logger = logger;
            BaseCurrency = new Currency("PLN", "Złoty");
            _client.BaseAddress = _nbpBaseApiAddress;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Currency BaseCurrency { get; }
        public decimal GetExchangeRate(Currency currency)
        {
            if (currency is null) throw new ArgumentNullException(nameof(currency));
            var uri = new Uri(_nbpBaseApiAddress, $"api/exchangerates/rates/{TableType}/{currency.Code}/?format=xml");
            _logger.LogInformation($"Downloading xml from {uri}...");
            var responseContent = new WebClient().DownloadString(uri);
            var document = new XmlDocument();
            document.LoadXml(responseContent);
            var midElement = document["ExchangeRatesSeries"]?["Rates"]?["Rate"]?["Mid"];
            return decimal.Parse(
                !string.IsNullOrWhiteSpace(midElement?.InnerText) ? 
                                            midElement.InnerText : 
                                            throw new FormatException("Cannot parse exchange rate from api NBP."), 
                new NumberFormatInfo(){NumberDecimalSeparator = "."});
        }
    }
}
