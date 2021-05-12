using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateAPI.Models
{
    public class RequestResponseItem
    {
        [Key]
        public long Id { get; set; }
        public RequestItem Request { get; set; }
        public ResponseItem Response { get; set; }
    }
}
