using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
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
                SaveRequest($"Converting: {exchangeRate}");
                var currencyCodes = _context.Currencies.Select(c => c.Code);

                if (!currencyCodes.Contains(exchangeRate.CurrencyFrom))
                    return NotFound(
                        $"Currency code \"{exchangeRate.CurrencyFrom}\" not found in available curriences");

                if (!currencyCodes.Contains(exchangeRate.CurrencyTo))
                    return NotFound(
                        $"Currency code \"{exchangeRate.CurrencyTo}\" not found in available curriences");

                return await Task.Run(() => _currencyConverter.Convert(exchangeRate.AmountFrom, new Currency(exchangeRate.CurrencyFrom),
                    new Currency(exchangeRate.CurrencyTo)));
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
                SaveRequest("Getting available curriences");
                return await _context.Currencies.Select(c => new { c.Code, c.Name }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting available curriences");
                throw;
            }
        }
        /// <summary>
        /// Writes the query to the database as intended.
        /// </summary>
        /// <param name="description"></param>
        private async void SaveRequest(string description = "")
        {
            _logger.LogInformation("Saving request to database.");
            var method = Request.Method;
            Request.EnableBuffering();
            //To read the request stream first creating a new byte[] with the same length as the request stream.
            var buffer = new byte[Convert.ToInt32(Request.ContentLength)];
            //Copy the entire request stream into the new buffer.
            await Request.Body.ReadAsync(buffer, 0, buffer.Length);
            //Coverting the byte[] into a string using UTF8 encoding to get string body.
            var body = Encoding.UTF8.GetString(buffer);
            var url = Request.GetDisplayUrl();
            await _context.RequestItems.AddAsync(new RequestItem()
                {Method = method, Url = url, DateTime = DateTime.Now, Description = description, Body = body});
            await _context.SaveChangesAsync();
        }
    }
}
