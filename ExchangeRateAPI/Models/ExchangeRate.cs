using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateAPI.Models
{
    public class ExchangeRate
    {
        public decimal AmountFrom { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
    }
}
