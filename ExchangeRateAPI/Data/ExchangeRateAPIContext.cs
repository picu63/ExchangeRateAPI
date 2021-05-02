using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }

        /// <summary>
        /// TODO Zastanowić się nad wpisaniem na sztywno danych listy dostepnych walut
        /// </summary>
        public DbSet<ExchangeRateAPI.Models.Currency> Currencies { get; set; }

        //public DbSet<ExchangeRateAPI.Models.ExchangeRate> ExchangeRates { get; set; }
    }
}
