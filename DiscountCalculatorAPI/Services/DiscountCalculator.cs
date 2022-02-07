using DiscountCalculatorAPI.Data;
using DiscountCalculatorAPI.Interface;
using DiscountCalculatorAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountCalculatorAPI.Services
{
    public class DiscountCalculator : IDiscountCalculator
    {
        public async Task<decimal> CalculateTotalPrice(PriceRequest req)
        {
            if(req.Price == 0)
                throw new ArgumentOutOfRangeException("price", "a price can't be zero or negative!");

            if (req.Weight == 0)
                throw new ArgumentOutOfRangeException("Weight", "a Weight can't be zero or negative!");

            decimal totalPrice = req.Price * req.Weight;

            decimal discount = 0;
            if (req.Discount.HasValue)
                discount = totalPrice * (req.Discount.Value / 100m);
            return await Task.FromResult(totalPrice - discount);
        }
      
    }
}
