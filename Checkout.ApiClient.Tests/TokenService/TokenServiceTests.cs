using Checkout;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;

namespace Tests
{
    [TestFixture(Category = "TokensApi")]
    public class TokenServiceTests : BaseServiceTests
    {
        [Test]
        public void CreatePaymentToken()
        {
            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel(TestHelper.RandomData.Email);
            var response = CheckoutClient.TokenService.CreatePaymentToken(paymentTokenCreateModel);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().StartWith("pay_tok_");
        }
    }
}
