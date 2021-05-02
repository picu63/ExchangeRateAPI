
using System;
using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Interfaces
{
    public interface ICurrencyConverter
    {
        decimal Convert(decimal fromAmount, Currency fromCurrency, Currency toCurrency);
    }
}