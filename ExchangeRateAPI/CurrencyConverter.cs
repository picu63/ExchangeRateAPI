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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateAPI
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ILogger<CurrencyConverter> _logger;
        private readonly IExchangeRateProvider _exchangeRateProvider;
        public CurrencyConverter(ILogger<CurrencyConverter> logger, IExchangeRateProvider exchangeRateProvider)
        {
            _logger = logger;
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<decimal> Convert(decimal fromAmount, Currency fromCurrency, Currency toCurrency)
        {
            try
            {
                if (fromAmount <= 0) throw new ArgumentOutOfRangeException(nameof(fromAmount), "Must be greater than zero.");
                if (fromCurrency is null) throw new ArgumentNullException(nameof(fromCurrency));
                if (toCurrency is null) throw new ArgumentNullException(nameof(toCurrency));
                _logger.LogInformation($"Converting {fromAmount} {fromCurrency.Code} to {toCurrency.Code}.");
                var fromToBaseAmount = fromAmount * await _exchangeRateProvider.GetExchangeRate(fromCurrency);
                return fromToBaseAmount / await _exchangeRateProvider.GetExchangeRate(toCurrency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while converting currency {fromAmount} of \"{fromCurrency}\" to \"{toCurrency}\"");
                throw;
            }
        }
    }
}
