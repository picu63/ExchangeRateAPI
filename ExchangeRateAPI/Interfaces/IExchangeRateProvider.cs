using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Interfaces
{
    public interface IExchangeRateProvider
    {
        Currency BaseCurrency { get; }
        decimal GetExchangeRate(Currency currency);
    }
}
