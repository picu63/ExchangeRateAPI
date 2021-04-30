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

        public DbSet<ExchangeRateAPI.Models.Currency> Currencies { get; set; }
    }
}
