using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRateAPI.Models
{
    [Table("RequestItems")]
    public class RequestItem
    {
        [Key]
        public long Id { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Headers { get; set; }
    }
}