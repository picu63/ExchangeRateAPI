
using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Interfaces
{
    public interface ICurrencyConverter<TAmount>
    {
        TAmount Convert(TAmount fromAmount, Currency fromCurrency, Currency toCurrency);
    }
}