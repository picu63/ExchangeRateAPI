using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateAPI.Models
{
    public class Money
    {
        public Money()
        {
            
        }

        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
    }
}
