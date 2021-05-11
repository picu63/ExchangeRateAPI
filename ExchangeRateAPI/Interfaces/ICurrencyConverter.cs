
using System;
using System.Threading.Tasks;
using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Interfaces
{
    public interface ICurrencyConverter
    {
        Task<decimal> Convert(decimal fromAmount, Currency fromCurrency, Currency toCurrency);
    }
}