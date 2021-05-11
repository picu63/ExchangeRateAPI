using System.Threading.Tasks;
using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Interfaces
{
    public interface IExchangeRateProvider
    {
        Currency BaseCurrency { get; }
        Task<decimal> GetExchangeRate(Currency currency);
    }
}
