using Checkout;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.TokenService
{
  
    namespace Tests
    {
        [TestFixture(Category = "TokensApi")]
        public class TokenServiceTests
        {
            APIClient CheckoutClient;

            [SetUp]
            public void Init()
            { CheckoutClient = new APIClient(); }

            [Test]
            public void CreatePaymentToken()
            {
                var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel(TestHelper.RandomData.Email);
                var response = CheckoutClient.TokenService.CreatePaymentToken(paymentTokenCreateModel);

                Assert.NotNull(response);
                Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
                Assert.IsTrue(response.Model.Id.StartsWith("pay_tok_"));
            }
        }
    }

}
