using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Models
{
    public class PriceRequest
    {
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public decimal? Discount { get; set; }
    }
}
