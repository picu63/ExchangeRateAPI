using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExchangeRateAPI.Data;
using ExchangeRateAPI.Interfaces;
using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRateAPIContext _context;
        private readonly Currency _baseCurrency;
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRatesController(ExchangeRateAPIContext context, Currency baseCurrency, IExchangeRateProvider exchangeRateProvider)
        {
            _context = context;
            _baseCurrency = baseCurrency;
            _exchangeRateProvider = exchangeRateProvider;
        }

        // GET: api/ExchangeRates
        [HttpPost]
        public async Task<ActionResult<decimal>> GetExchangeRate(ExchangeRate exchangeRate)
        {
            ICurrencyConverter<decimal> currencyConverter = new CurrencyConverter(_exchangeRateProvider);
            var result = currencyConverter.Convert(exchangeRate.Amount, new Currency(exchangeRate.CurrencyFrom),
                new Currency(exchangeRate.CurrencyTo));
            return result;
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetAvailableCurriences()
        {
            return await _context.Currencies.ToListAsync();
        }
    }
}
