using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Numerics;
using System.Threading.Tasks;
using ExchangeRateAPI.Interfaces;
using ExchangeRateAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateAPI
{
    public class CurrencyConverter : ICurrencyConverter<decimal>
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        public CurrencyConverter(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public decimal Convert(decimal fromAmount, Currency fromCurrency, Currency toCurrency)
        {
            var fromToBaseAmount = fromAmount * _exchangeRateProvider.GetExchangeRate(fromCurrency);
            return fromToBaseAmount / _exchangeRateProvider.GetExchangeRate(toCurrency);
        }
    }
}
