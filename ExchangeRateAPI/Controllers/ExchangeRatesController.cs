using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExchangeRateAPI.Data;
using ExchangeRateAPI.Interfaces;
using ExchangeRateAPI.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace ExchangeRateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRateAPIContext _context;
        private readonly ILogger<ExchangeRatesController> _logger;
        private readonly ICurrencyConverter _currencyConverter;

        public ExchangeRatesController(ExchangeRateAPIContext context, ILogger<ExchangeRatesController> logger, ICurrencyConverter currencyConverter)
        {
            _context = context;
            _logger = logger;
            _currencyConverter = currencyConverter;
        }

        /// <summary>
        /// Converting given amount of currency to provided currency.
        /// </summary>
        /// <param name="exchangeRate"></param>
        /// <returns>Amount of money of given currency expectation.</returns>
        [HttpPost("converter")]
        public async Task<ActionResult<decimal>> ConvertCurrency(ExchangeRate exchangeRate)
        {
            try
            {
                _logger.LogInformation($"Converting given amount of currency to provided currency: {exchangeRate}");
                Request.EnableBuffering();
                var currencyCodes = _context.Currencies.Select(c => c.Code).ToList();

                if (!currencyCodes.Contains(exchangeRate.CurrencyFrom))
                    return NotFound(
                        $"Currency code \"{exchangeRate.CurrencyFrom}\" not found in available curriences");

                if (!currencyCodes.Contains(exchangeRate.CurrencyTo))
                    return NotFound(
                        $"Currency code \"{exchangeRate.CurrencyTo}\" not found in available curriences");

                return await _currencyConverter.Convert(exchangeRate.AmountFrom, new Currency(exchangeRate.CurrencyFrom),
                    new Currency(exchangeRate.CurrencyTo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in converting currency.");
                throw;
            }
        }

        /// <summary>
        /// Gets all the available curriences.
        /// </summary>
        /// <returns>List of code - name available currencies.</returns>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableCurriences()
        {
            try
            {
                Request.EnableBuffering();
                return await _context.Currencies.Select(c => new { c.Code, c.Name }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting available curriences");
                throw;
            }
        }
    }
}
