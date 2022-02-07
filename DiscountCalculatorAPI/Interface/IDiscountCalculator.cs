using DiscountCalculatorAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Interface
{
    public interface IDiscountCalculator
    {
        Task<decimal> CalculateTotalPrice(PriceRequest req);
    }
}
