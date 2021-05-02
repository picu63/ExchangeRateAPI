using System;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateAPI.Models
{
    public class Currency
    {
        public Currency()
        {

        }
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Cannot be null or empty", nameof(code));
            Code = code;
        }

        public Currency(string code, string name) : this(code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Cannot be null or empty", nameof(code));
            Name = name;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public override string ToString()
        {
            return Code + ((string.IsNullOrWhiteSpace(Name)) ? string.Empty : $" ({Name})");
        }
    }
}