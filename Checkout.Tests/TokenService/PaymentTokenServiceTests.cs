using Checkout;
using Checkout.ApiServices.Tokens.ResponseModels;
using NUnit.Framework;
using System;


namespace Tests
{
    [TestFixture(Category = "TokensApi")]
    public class PaymentTokensApiTests
    {
        public string samplePaymentToken;

       
        [Test]
        public void CreatePaymentToken()
        {
            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel();

            var response = new CheckoutClient().TokenService.CreatePaymentToken(paymentTokenCreateModel);
            PaymentToken model = response.Model;

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(model.Id.StartsWith("pay_tok"));
            samplePaymentToken = model.Id;
        }
        

        
        [Test]
        public void CreatePaymentToken_Fails_IfAmountIsInvalid()
        {
            var paymentTokenCreateModel = TestHelper.GetPaymentTokenCreateModel();
            paymentTokenCreateModel.Value = -100;

            var response = new CheckoutClient().TokenService.CreatePaymentToken(paymentTokenCreateModel);
            PaymentToken model = response.Model;

            Assert.NotNull(response);
            Assert.IsTrue(response.HttpStatusCode != System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.HasError);
            Assert.IsTrue(response.Error.Message.ToLower().Contains("validation error"));

        }
        
    }
}
