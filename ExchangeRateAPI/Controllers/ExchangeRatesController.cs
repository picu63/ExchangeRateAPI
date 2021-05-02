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
using Microsoft.Extensions.Logging;

namespace ExchangeRateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRateAPIContext _context;
        private readonly ILogger<ExchangeRatesController> _logger;
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ICurrencyConverter _currencyConverter;

        public ExchangeRatesController(ExchangeRateAPIContext context, ILogger<ExchangeRatesController> logger, IExchangeRateProvider exchangeRateProvider, ICurrencyConverter currencyConverter)
        {
            _context = context;
            _logger = logger;
            _exchangeRateProvider = exchangeRateProvider;
            _currencyConverter = currencyConverter;
        }

        // GET: api/ExchangeRates
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "amountFrom": 200,
        ///         "currencyFrom":"USD",
        ///         "currencyTo":"PLN"
        ///     }
        /// </remarks>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        [HttpPost("converter")]
        public async Task<ActionResult<decimal>> ConvertCurrency(ExchangeRate exchangeRate)
        {
            var currencyCodes = _context.Currencies.Select(c => c.Code);

            if (!currencyCodes.Contains(exchangeRate.CurrencyFrom))
            {
                return NotFound(
                    $"Currency code \"{exchangeRate.CurrencyFrom}\" not found in available curriences");
            } 

            if (!currencyCodes.Contains(exchangeRate.CurrencyTo))
            {
                return NotFound(
                    $"Currency code \"{exchangeRate.CurrencyTo}\" not found in available curriences");
            }

            return await Task.Run(()=> _currencyConverter.Convert(exchangeRate.AmountFrom, new Currency(exchangeRate.CurrencyFrom),
                new Currency(exchangeRate.CurrencyTo)));
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetAvailableCurriences()
        {
            return await _context.Currencies.ToListAsync();
        }
    }
}
