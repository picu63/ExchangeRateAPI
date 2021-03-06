using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateAPI.Data;
using ExchangeRateAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateAPI.Controllers
{
    public class CurrenciesController : ControllerBase
    {
        private readonly ExchangeRateAPIContext _dbContext;
        private readonly IOptions<Currency> _mainCurrencyWrapper;

        public CurrenciesController(ILogger<CurrenciesController> logger, ExchangeRateAPIContext dbContext, IOptions<Currency> mainCurrencyWrapper)
        {
            _dbContext = dbContext;
            _mainCurrencyWrapper = mainCurrencyWrapper;
        }

        /// <summary>
        /// Gets all available curriences.
        /// </summary>
        [HttpGet("curriences")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurriences()
        {
            return await _dbContext.Currencies.ToListAsync();
        }

        /// <summary>
        /// Adds currency to context.
        /// </summary>
        /// <returns></returns>
        [HttpPost("curriences")]
        public async Task<ActionResult> AddCurrency(Currency currency)
        {
            if (_dbContext.Currencies.Select(c=>c.Code).Contains(currency.Code))
            {
                return new ObjectResult($"Given currency already contains {currency}");
            }

            await _dbContext.Currencies.AddAsync(currency);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("currencies")]
        public async Task<ActionResult> DeleteCurrency(string code)
        {
            _dbContext.Currencies.Remove(new Currency(code));
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("currencies/main")]
        public ActionResult<Currency> GetMainCurrency()
        {
            return _mainCurrencyWrapper.Value;
        }
    }
}