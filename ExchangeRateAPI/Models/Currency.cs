using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            if(!string.IsNullOrWhiteSpace(Name))
                return Code + $" ({Name})";
            return Code;
        }
    }
}