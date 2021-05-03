using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using ExchangeRateAPI.Models;

namespace ExchangeRateAPI.Data
{
    public class ExchangeRateAPIContext : DbContext
    {
        public ExchangeRateAPIContext (DbContextOptions<ExchangeRateAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            AddInitDefaultValues();
        }

        private void AddInitDefaultValues()
        {
            var initialCurrencies = new List<Currency>()
            {
                new("USD", "dolar amerykański"),
                new("EUR", "euro"),
                new("CHF", "frank szwajcarski"),
                new("GBP", "funt szterling"),
                new("JPY", "jen"),
                new("HUF", "forint"),
                new("CZK", "korona czeska"),
            };
            foreach (var initialCurrency in initialCurrencies
                                            .Where(initialCurrency => !Currencies.Select(c => c.Code)
                                                                        .Contains(initialCurrency.Code)))
            {
                Currencies.Add(initialCurrency);
            }
            SaveChanges();
        }

        /// <summary>
        /// Currencies 
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }

        public DbSet<RequestItem> RequestItems { get; set; }
    }
}
