
namespace ExchangeRateCore
{
    public interface ICurrencyConverter<in TCurrency,TAmount>
    {
        TAmount Convert(TAmount fromAmount, TCurrency fromCurrency, TCurrency toCurrency);
    }
}