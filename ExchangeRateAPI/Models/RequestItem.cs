using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateAPI.Models
{
    public class RequestItem
    {
        [Key]
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Headers { get; set; }
    }
}
