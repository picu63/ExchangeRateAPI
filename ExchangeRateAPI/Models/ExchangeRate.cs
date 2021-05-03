using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateAPI.Models
{
    /// <summary>
    /// Exchange rate for currency converter.
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Amount of currency to convert.
        /// </summary>
        /// <example>313</example>
        public decimal AmountFrom { get; set; }

        /// <summary>
        /// Currency from which the course is calculated.
        /// </summary>
        /// <example>USD</example>
        public string CurrencyFrom { get; set; }

        /// <summary>
        /// Currency to which the course is calculated.
        /// </summary>
        /// <example>EUR</example>
        public string CurrencyTo { get; set; }

        public override string ToString()
        {
            return $"From: {AmountFrom} of {CurrencyFrom}, To: {CurrencyTo}";
        }
    }
}
