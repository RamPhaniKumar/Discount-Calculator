using DiscountCalculatorAPI.Controllers;
using DiscountCalculatorAPI.Data;
using DiscountCalculatorAPI.Interface;
using DiscountCalculatorAPI.Models;
using DiscountCalculatorAPI.Services;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace DiscountCalculatorUnitTest
{
    public class Tests
    {
        private Mock<IUserDetails> _user;
        private IDiscountCalculator _disCalculator;
        string accesskey;
        [SetUp]
        public void Setup()
        {
            _user = new Mock<IUserDetails>();
            _disCalculator = new DiscountCalculator();
            accesskey = Guid.NewGuid().ToString();
        }

        [Test]
        public async Task Login()
        {
            var req = new LoginRequest()
            {
                UserName = "testuser",
                Password = "test"
            };
            _user.Setup(x => x.Login(req)).ReturnsAsync(accesskey);

            var contoller = new LoginController(_user.Object);

            var result = await contoller.Login(req);

            result.Value.ToString().ShouldNotBeNullOrWhiteSpace();

            //Invalid test cases
            req = new LoginRequest()
            {
                UserName = "testuser1",
                Password = "test"
            };
            _user.Setup(x => x.Login(req)).Throws(new ArgumentException());
            await Should.ThrowAsync<ArgumentException>(contoller.Login(req));
        }


        [Test]
        public async Task TotalPriceFormula()
        {
            _user.Setup(x => x.ValidateAccessKey(accesskey)).Returns(true);
            var req = new PriceRequest()
            {
                Price = 10,
                Weight = 10,
                Discount = 2
            };
            var contoller = new DiscountCalculatorController(_disCalculator,_user.Object);
            var res = await contoller.Discount(accesskey, req);


            res.Value.ShouldBe(CalculateTotalPrice(req));

            req = new PriceRequest()
            {
                Price = 10,
                Weight = 10
            };
            var result = await contoller.Discount(accesskey, req);
            result.Value.ShouldBe(CalculateTotalPrice(req));


            //Invalid test cases
            req = new PriceRequest()
            {
                Price = 0,
                Weight = 10,
                Discount = 2
            };
           await Should.ThrowAsync<ArgumentOutOfRangeException>(contoller.Discount(accesskey, req));
            req = new PriceRequest()
            {
                Price = 10,
                Weight = 0,
                Discount = 2
            };
           await Should.ThrowAsync<ArgumentOutOfRangeException>(contoller.Discount(accesskey, req));
         
        }

        private decimal CalculateTotalPrice(PriceRequest req)
        {
            decimal totalPrice = req.Price * req.Weight;
            decimal discount = 0;
            if (req.Discount.HasValue)
                discount = totalPrice * (req.Discount.Value / 100m);
            return totalPrice - discount;
        }
    }
}