using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRateAPI.Models
{
    [Table("ResponseItems")]
    public class ResponseItem
    {
        [Key]
        public long Id { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Headers { get; set; }
        public int? StatusCode { get; set; }
    }
}